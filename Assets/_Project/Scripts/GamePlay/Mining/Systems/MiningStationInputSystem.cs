using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnrealTeam.SB.Common.Ecs;
using UnrealTeam.SB.Common.Enums;
using UnrealTeam.SB.GamePlay.CharacterController.Components;
using UnrealTeam.SB.GamePlay.Mining.Components;
using UnrealTeam.SB.GamePlay.Mining.Views;
using UnrealTeam.SB.Services.InputControl.Interfaces;

namespace UnrealTeam.SB.GamePlay.Mining.Systems
{
    public class MiningStationInputSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<MiningStationControlledTag>> _filter;
        private readonly EcsPoolInject<ComponentRef<MiningStationSyncView>> _stationSyncRefPool;
        private readonly EcsPoolInject<PlayerControlData> _playerControlPool;
        private readonly EcsPoolInject<RotateMiningLaserAction> _rotateLaserAction;
        private readonly IInputService _inputService;

        
        public MiningStationInputSystem(IInputService inputService)
        {
            _inputService = inputService;
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (int stationEntity in _filter.Value)
                HandleStationInputs(stationEntity);
        }

        private void HandleStationInputs(int stationEntity)
        {
            var stationSyncView = _stationSyncRefPool.Value.Get(stationEntity).Component;
            int playerEntity = stationSyncView.ControlledBy;
            if (playerEntity < 0)
                throw new InvalidOperationException();
            
            ref var playerControlData = ref _playerControlPool.Value.Get(playerEntity);
            if (playerControlData.CurrentState != PlayerControlState.MiningStation)
                throw new InvalidOperationException();

            if (_inputService.MoveAxisX.IsHold())
                _rotateLaserAction.Value.Add(stationEntity).ValueX = _inputService.MoveAxisX.Value();
        }
    }
}