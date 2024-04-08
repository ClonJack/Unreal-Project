﻿using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnrealTeam.SB.Configs.Player;
using UnrealTeam.SB.Configs.Spawn;
using UnrealTeam.SB.GamePlay.Network;
using UnrealTeam.SB.Services.Configs;
using VContainer;
using VContainer.Unity;

namespace UnrealTeam.SB.Services.Factories
{
    public class PlayerFactory
    {
        private readonly IConfigAccess _configAccess;
        private readonly SpawnPoint _spawnPoint;
        private readonly IObjectResolver _objectResolver;
        private readonly NetworkStateMachine _networkStateMachine;

        public PlayerFactory(IConfigAccess configAccess, SpawnPoint spawnPoint, IObjectResolver objectResolver,
            NetworkStateMachine networkStateMachine)
        {
            _configAccess = configAccess;
            _spawnPoint = spawnPoint;
            _objectResolver = objectResolver;
            _networkStateMachine = networkStateMachine;
        }

        public async UniTask Create()
        {
            var playerConfig = _configAccess.GetSingle<PlayerConfig>();

            var operationPlayer = await Addressables.LoadAssetAsync<GameObject>(playerConfig.PlayerPrefab);

            _networkStateMachine.OnPlayerJoin += (async (runner, playerRef) =>
            {
                if (playerRef != runner.LocalPlayer) return;

                var networkObject = runner.Spawn(operationPlayer, Vector3.zero, Quaternion.identity, playerRef);
                
                _objectResolver.InjectGameObject(networkObject.gameObject);
            });
        }
    }
}