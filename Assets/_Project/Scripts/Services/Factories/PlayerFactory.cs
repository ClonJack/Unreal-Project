using System;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnrealTeam.SB.Configs.Player;
using UnrealTeam.SB.Services.Configs;
using UnrealTeam.SB.Services.Network;
using VContainer;
using VContainer.Unity;

namespace UnrealTeam.SB.Services.Factories
{
    public class PlayerFactory : IDisposable
    {
        private readonly IConfigAccess _configAccess;
        private readonly IObjectResolver _objectResolver;
        private readonly NetworkStateMachine _networkStateMachine;
        private GameObject _playerPrefab;

        
        public PlayerFactory(
            IConfigAccess configAccess, 
            IObjectResolver objectResolver,
            NetworkStateMachine networkStateMachine)
        {
            _configAccess = configAccess;
            _objectResolver = objectResolver;
            _networkStateMachine = networkStateMachine;
        }

        public async UniTask CreatePlayersOnJoin()
        {
            var playerConfig = _configAccess.GetSingle<PlayerConfig>();
            _playerPrefab = await Addressables.LoadAssetAsync<GameObject>(playerConfig.PlayerPrefab);
            _networkStateMachine.OnPlayerJoin += CreateJoinedPlayer;
        }

        public void Dispose()
        {
            _networkStateMachine.OnPlayerJoin -= CreateJoinedPlayer;
        }

        private void CreateJoinedPlayer(NetworkRunner runner, PlayerRef playerRef)
        {
            if (playerRef != runner.LocalPlayer) 
                return;

            var networkObject = runner.Spawn(_playerPrefab, Vector3.zero, Quaternion.identity, playerRef);
            runner.SetPlayerObject(playerRef, networkObject);

            _objectResolver.InjectGameObject(networkObject.gameObject);
        }
    }
}