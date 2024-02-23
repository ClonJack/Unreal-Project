using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnrealTeam.SB.Systems;
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
                .Add(_objectResolver.Resolve<PlayerInputSystem>())
                .Add(_objectResolver.Resolve<CharacterMoveSystem>())
                
#if UNITY_EDITOR
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
                .Inject()
                .Init();

            _lateUpdateSystems
                .Add(_objectResolver.Resolve<CharacterRotateSystem>())
                .Inject()
                .Init();

            _fixedUpdateSystems.
                Inject()
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