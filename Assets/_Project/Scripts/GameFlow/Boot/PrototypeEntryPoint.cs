using System;
using UnityEngine;
using UnrealTeam.SB.Factories;
using VContainer.Unity;

namespace UnrealTeam.SB.GameFlow
{
    public class PrototypeEntryPoint : IStartable, IDisposable, ITickable, IFixedTickable, ILateTickable
    {
        private readonly PlayerFactory _playerFactory;
        private readonly EcsService _ecsService;

        public PrototypeEntryPoint(
            EcsService ecsService,
            PlayerFactory playerFactory)
        {
            _playerFactory = playerFactory;
            _ecsService = ecsService;
        }

        public void Start()
        {
            _ecsService.Init();

            _playerFactory.Create();
        }

        public void Tick()
            => _ecsService.Update();

        public void LateTick()
            => _ecsService.LateUpdate();

        public void FixedTick()
            => _ecsService.FixedTick();

        public void Dispose()
            => _ecsService.Dispose();
    }
}