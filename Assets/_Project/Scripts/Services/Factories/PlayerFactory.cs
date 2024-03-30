using Cysharp.Threading.Tasks;
using Leopotam.EcsLite;
using UnityEngine.AddressableAssets;
using UnrealTeam.SB.Common.Ecs;
using UnrealTeam.SB.Configs.Player;
using UnrealTeam.SB.Configs.Spawn;
using UnrealTeam.SB.GamePlay.Components;
using UnrealTeam.SB.GamePlay.Tags;
using UnrealTeam.SB.GamePlay.Views;
using UnrealTeam.SB.Services.Configs;

namespace UnrealTeam.SB.Services.Factories
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

        public async UniTask Create()
        {
            var playerConfig = _configAccess.GetSingle<PlayerConfig>();

            var instPlayer = await Addressables.InstantiateAsync(playerConfig.PrefabCharacter, _spawnPoint.transform.position,
                _spawnPoint.transform.rotation);
            
            var instCamera = await Addressables.InstantiateAsync(playerConfig.PrefabCamera);

            var entity = _ecsWorld.NewEntity();

            _ecsWorld.GetPool<PlayerTag>().Add(entity);
            _ecsWorld.GetPool<CharacterData>().Add(entity);
            
            var characterView = instPlayer.GetComponent<CharacterView>();
            _ecsWorld.GetPool<ComponentRef<CharacterView>>().Add(entity).Value = characterView;

            var cameraView = instCamera.GetComponent<CameraView>();
            _ecsWorld.GetPool<ComponentRef<CameraView>>().Add(entity).Value = cameraView;

            cameraView.FollowTransform = characterView.CameraTarget;
        }
    }
}