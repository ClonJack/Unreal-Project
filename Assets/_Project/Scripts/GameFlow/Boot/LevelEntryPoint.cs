using System;
using Cysharp.Threading.Tasks;
using Services.Factories;
using Services.Loading;
using VContainer.Unity;

namespace GameFlow
{
    public class LevelEntryPoint : IDisposable, ITickable, IFixedTickable
    {
        private readonly PlayerFactory _playerFactory;
        private readonly EcsLoop _ecsLoop;
        private readonly LoadingCurtain _loadingCurtain;


        public LevelEntryPoint(
            EcsLoop ecsLoop,
            PlayerFactory playerFactory,
            LoadingCurtain loadingCurtain)
        {
            _loadingCurtain = loadingCurtain;
            _playerFactory = playerFactory;
            _ecsLoop = ecsLoop;
        }

        public async UniTask ExecuteAsync()
        {
            _ecsLoop.Init();
            SpawnEntities();
            await _loadingCurtain.HideAsync();
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