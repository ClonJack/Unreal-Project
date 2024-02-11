using System;
using UnrealTeam.SB.Factories;
using VContainer.Unity;

namespace UnrealTeam.SB.GameFlow
{
    public class PrototypeEntryPoint : IDisposable, ITickable, IFixedTickable
    {
        private readonly PlayerFactory _playerFactory;
        private readonly EcsLoop _ecsLoop;

        public PrototypeEntryPoint(
            EcsLoop ecsLoop,
            PlayerFactory playerFactory)
        {
            _playerFactory = playerFactory;
            _ecsLoop = ecsLoop;
        }

        public void Execute()
        {
            _ecsLoop.Init();

            _playerFactory.Create();
        }

        public void Tick()
            => _ecsLoop.Tick();

        public void FixedTick()
            => _ecsLoop.FixedTick();

        public void Dispose()
            => _ecsLoop.Dispose();
    }
}