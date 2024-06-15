using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;
using UnrealTeam.SB.GamePlay.CharacterController.Components;
using UnrealTeam.SB.GamePlay.CharacterController.Systems;
using UnrealTeam.SB.GamePlay.Common.Systems;
using UnrealTeam.SB.GamePlay.Durability.Components;
using UnrealTeam.SB.GamePlay.Durability.Systems;
using UnrealTeam.SB.GamePlay.Interaction.Components;
using UnrealTeam.SB.GamePlay.Interaction.Systems;
using UnrealTeam.SB.GamePlay.Mining.Components;
using UnrealTeam.SB.GamePlay.Mining.Systems;
using VContainer;

namespace UnrealTeam.SB.GameFlow.Game
{
    public class EcsLoop : IDisposable
    {
        private readonly IObjectResolver _objectResolver;

        private EcsWorld _ecsWorld;

        private IEcsSystems _updateSystems;
        private IEcsSystems _fixedUpdateSystems;
        private IEcsSystems _lateUpdateSystems;

        public EcsLoop(EcsWorld ecsWorld, IObjectResolver objectResolver)
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
                .Add(ResolveSystem<CharacterInputSystem>())
                .Add(ResolveSystem<CharacterMoveSystem>())
                .Add(ResolveSystem<InteractionSystem>())
                .Add(ResolveSystem<UseInteractedSystem>())
                .Add(ResolveSystem<OutlineInteractedSystem>())
                .Add(ResolveSystem<DrawInteractionUiSystem>())
                .Add(ResolveSystem<MiningStationInputSystem>())
                .Add(ResolveSystem<MiningStationEnterSystem>())
                .Add(ResolveSystem<MiningStationLeaveSystem>())
                .Add(ResolveSystem<MiningLaserRotationSystem>())
                .Add(ResolveSystem<MiningPlatformRotationSystem>())
                .Add(ResolveSystem<MiningLaserWarmSystem>())
                .Add(ResolveSystem<MiningLaserDrawSystem>())
                .Add(ResolveSystem<DurabilityChangeSystem>())
                .Add(ResolveSystem<DurabilityDrawUiSystem>())
                .DelHere<CharacterUseAction>()
                .DelHere<UsedObjectAction>()
                .DelHere<EndInteractAction>()
                .DelHere<MiningStationLeaveAction>()
                .DelHere<MiningLaserRotationAction>()
                .DelHere<MiningPlatformRotationAction>()
                .DelHere<MiningLaserWarmAction>()
                .DelHere<MiningLaserWarmedEvent>()
                .DelHere<MiningLaserCooledEvent>()
                .DelHere<DurabilityChangeRequest>()
                .DelHere<DurabilityChangedEvent>()
#if UNITY_EDITOR
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif

                .Inject()
                .Init();


            _lateUpdateSystems
                .Add(ResolveSystem<CharacterRotateSystem>())
                .Add(ResolveSystem<LookAtCameraSystem>())
                .DelHere<CharacterRotateAction>()
                .DelHere<CharacterMoveAction>()
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

        private TSystem ResolveSystem<TSystem>()
            where TSystem : IEcsSystem
            => _objectResolver.Resolve<TSystem>();
    }
}