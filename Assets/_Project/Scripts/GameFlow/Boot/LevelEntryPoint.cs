using System;
using Cysharp.Threading.Tasks;
using Services.Loading;
using UnrealTeam.SB.Factories;
using UnrealTeam.SB.GameFlow;
using VContainer.Unity;

namespace GameFlow
{
    public class LevelEntryPoint : IDisposable, ITickable, IFixedTickable, ILateTickable
    {
        private readonly PlayerFactory _playerFactory;
        private readonly EcsService _ecsService;
        private readonly LoadingCurtain _loadingCurtain;


        public LevelEntryPoint(
            EcsService ecsService,
            PlayerFactory playerFactory,
            LoadingCurtain loadingCurtain)
        {
            _loadingCurtain = loadingCurtain;
            _playerFactory = playerFactory;
            _ecsService = ecsService;
        }

        public async UniTask ExecuteAsync()
        {
            _ecsService.Init();
            SpawnEntities();
            await _loadingCurtain.HideAsync();
        }

        public void Tick()
            => _ecsService.Tick();

        public void LateTick()
            => _ecsService.LateTick();

        public void FixedTick()
            => _ecsService.FixedTick();

        public void Dispose()
            => _ecsService.Dispose();


        private void SpawnEntities()
        {
            _playerFactory.Create();
        }
    }
}