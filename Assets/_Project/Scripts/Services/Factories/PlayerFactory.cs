using Cysharp.Threading.Tasks;
using Leopotam.EcsLite;
using UnityEngine.AddressableAssets;
using UnrealTeam.SB.Components;
using UnrealTeam.SB.Configs;
using UnrealTeam.SB.Spawn;
using UnrealTeam.SB.Views;

namespace UnrealTeam.SB.Factories
{
    public class PlayerFactory
    {
        private readonly IConfigAccess _configAccess;
        private readonly SpawnPoint _spawnPoint;
        private readonly EcsWorld _ecsWorld;

        public PlayerFactory(EcsWorld ecsWorld, IConfigAccess configAccess, SpawnPoint spawnPoint)
        {
            _ecsWorld = ecsWorld;
            _configAccess = configAccess;
            _spawnPoint = spawnPoint;
        }

        public async void Create()
        {
            var playerConfig = _configAccess.GetSingle<PlayerConfig>();

            var inst = await Addressables.InstantiateAsync(playerConfig.Prefab, _spawnPoint.transform.position,
                _spawnPoint.transform.rotation);

            var entity = _ecsWorld.NewEntity();

            _ecsWorld.GetPool<PlayerTag>().Add(entity);
            _ecsWorld.GetPool<CharacterData>().Add(entity);

            var cameraView = _ecsWorld.GetPool<ComponentRef<CameraView>>().Add(entity).Value =
                inst.GetComponentInChildren<CameraView>();

            cameraView.ConvertToEntity(_ecsWorld, entity);

            _ecsWorld.GetPool<ComponentRef<CharacterView>>().Add(entity).Value = inst.GetComponent<CharacterView>();
        }
    }
}