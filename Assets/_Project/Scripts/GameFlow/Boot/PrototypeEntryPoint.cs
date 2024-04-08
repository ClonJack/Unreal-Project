using System;
using Cysharp.Threading.Tasks;
using UnrealTeam.SB.Services.Factories;
using VContainer.Unity;

namespace UnrealTeam.SB.GameFlow.Boot
{
    public class PrototypeEntryPoint : IDisposable, ITickable, IFixedTickable, ILateTickable
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

        public async UniTask Execute()
        {
            _ecsService.Init();
            
            await _playerFactory.Create();
        }

        public void Tick()
            => _ecsService.Tick();

        public void LateTick()
            => _ecsService.LateTick();

        public void FixedTick()
            => _ecsService.FixedTick();

        public void Dispose()
            => _ecsService.Dispose();
    }
}