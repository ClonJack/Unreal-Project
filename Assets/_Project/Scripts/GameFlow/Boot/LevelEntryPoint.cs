using System;
using Services.Factories;
using VContainer.Unity;

namespace GameFlow
{
    public class LevelEntryPoint : IDisposable, ITickable, IFixedTickable
    {
        private readonly PlayerFactory _playerFactory;
        private readonly EcsLoop _ecsLoop;


        public LevelEntryPoint(
            EcsLoop ecsLoop,
            PlayerFactory playerFactory)
        {
            _playerFactory = playerFactory;
            _ecsLoop = ecsLoop;
        }

        public void Execute()
        {
            _ecsLoop.Init();
            SpawnEntities();
        }

        public void Tick()
            => _ecsLoop.Tick();

        public void FixedTick()
            => _ecsLoop.FixedTick();

        public void Dispose()
            => _ecsLoop.Dispose();
        

        private void SpawnEntities()
        {
            _playerFactory.Create();
        }
    }
}