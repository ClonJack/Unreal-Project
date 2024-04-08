using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnrealTeam.SB.Configs.Player;
using UnrealTeam.SB.Configs.Spawn;
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

        public PlayerFactory(IConfigAccess configAccess, SpawnPoint spawnPoint, IObjectResolver objectResolver)
        {
            _configAccess = configAccess;
            _spawnPoint = spawnPoint;
            _objectResolver = objectResolver;
        }

        public async UniTask Create()
        {
            var playerConfig = _configAccess.GetSingle<PlayerConfig>();

            var instancePlayer = await Addressables.InstantiateAsync(playerConfig.PlayerPrefab,
                _spawnPoint.transform.position,
                _spawnPoint.transform.rotation);

            _objectResolver.InjectGameObject(instancePlayer);
        }
    }
}