using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;
using UnrealTeam.SB.GamePlay.CharacterController.Components;
using UnrealTeam.SB.GamePlay.CharacterController.Systems;
using UnrealTeam.SB.GamePlay.Interaction.Components;
using UnrealTeam.SB.GamePlay.Interaction.Systems;
using UnrealTeam.SB.GamePlay.Mining.Components;
using UnrealTeam.SB.GamePlay.Mining.Systems;
using VContainer;

namespace UnrealTeam.SB.GameFlow
{
    public class EcsService : IDisposable
    {
        private readonly IObjectResolver _objectResolver;

        private EcsWorld _ecsWorld;

        private IEcsSystems _updateSystems;
        private IEcsSystems _fixedUpdateSystems;
        private IEcsSystems _lateUpdateSystems;

        public EcsService(EcsWorld ecsWorld, IObjectResolver objectResolver)
        {
            _ecsWorld = ecsWorld;
            _objectResolver = objectResolver;
        }

        public void Init()
            => InitSystems();

        public void Tick()
            => _updateSystems?.Run();

        public void LateTick() 
            => _lateUpdateSystems?.Run();

        public void FixedTick()
            => _fixedUpdateSystems?.Run();

        public void Dispose()
            => DisposeWorld();


        private void InitSystems()
        {
            _updateSystems = new EcsSystems(_ecsWorld);
            _lateUpdateSystems = new EcsSystems(_ecsWorld);
            _fixedUpdateSystems = new EcsSystems(_ecsWorld);

            _updateSystems
                .Add(_objectResolver.Resolve<CharacterInputSystem>())
                .Add(_objectResolver.Resolve<CharacterMoveSystem>())
                
                
                .Add(_objectResolver.Resolve<InteractionSystem>())
                .Add(_objectResolver.Resolve<UseInteractedSystem>())
                .Add(_objectResolver.Resolve<OutlineInteractedSystem>())
                .Add(_objectResolver.Resolve<DrawInteractionUiSystem>())
                
                
                .Add(_objectResolver.Resolve<MiningStationInputSystem>())
                .Add(_objectResolver.Resolve<MiningStationEnterSystem>())
                .Add(_objectResolver.Resolve<MiningStationLeaveSystem>())
                .Add(_objectResolver.Resolve<MiningLaserRotationSystem>())
                .Add(_objectResolver.Resolve<MiningPlatformRotationSystem>())
                
                .DelHere<CharacterMoveAction>()
                .DelHere<CharacterUseAction>()
                .DelHere<UsedObjectAction>()
                .DelHere<EndInteractAction>()
                .DelHere<MiningStationLeaveAction>()
                .DelHere<MiningLaserRotationAction>()
                .DelHere<MiningPlatformRotationAction>()
                
#if UNITY_EDITOR
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
                
                .Inject()
                .Init();

            
            _lateUpdateSystems
                .Add(_objectResolver.Resolve<CharacterRotateSystem>())
                
                .DelHere<CharacterRotateAction>()
                
                .Inject()
                .Init();

            
            _fixedUpdateSystems
                .Inject()
                .Init();
        }

        private void DisposeWorld()
        {
            if (_updateSystems != null)
            {
                _updateSystems.Destroy();
                _updateSystems = null;
            }

            if (_fixedUpdateSystems != null)
            {
                _fixedUpdateSystems.Destroy();
                _fixedUpdateSystems = null;
            }

            if (_ecsWorld != null)
            {
                _ecsWorld.Destroy();
                _ecsWorld = null;
            }
        }
    }
}