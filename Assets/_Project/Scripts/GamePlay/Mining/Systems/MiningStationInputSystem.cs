using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnrealTeam.SB.Additional.Enums;
using UnrealTeam.SB.Common.Ecs;
using UnrealTeam.SB.GamePlay.CharacterController.Components;
using UnrealTeam.SB.GamePlay.Mining.Components;
using UnrealTeam.SB.GamePlay.Mining.Views;
using UnrealTeam.SB.Services.InputControl.Interfaces;

namespace UnrealTeam.SB.GamePlay.Mining.Systems
{
    public class MiningStationInputSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<StationControlledMarker>> _controlledStationFilter;
        private readonly EcsPoolInject<ComponentRef<MiningStationSyncView>> _stationSyncPool;
        private readonly EcsPoolInject<PlayerControlData> _playerControlPool;
        private readonly EcsPoolInject<MiningLaserRotationAction> _laserRotationActionPool;
        private readonly EcsPoolInject<MiningStationLeaveAction> _stationLeaveActionPool;
        private readonly EcsPoolInject<MiningLaserWarmAction> _stationWarmActionPool;
        private readonly IInputService _inputService;

        
        public MiningStationInputSystem(IInputService inputService)
        {
            _inputService = inputService;
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var stationEntity in _controlledStationFilter.Value)
                HandleStationInputs(stationEntity);
        }

        private void HandleStationInputs(int stationEntity)
        {
            var playerEntity = _stationSyncPool.Value.Get(stationEntity).Component.ControlledBy;
            if (playerEntity < 0)
                return;
            
            ref var playerControlData = ref _playerControlPool.Value.Get(playerEntity);
            if (playerControlData.CurrentState != PlayerControlState.MiningStation)
                throw new InvalidOperationException();

            if (_inputService.MoveAxisX.IsHold())
                _laserRotationActionPool.Value.Add(stationEntity).ValueX = _inputService.MoveAxisX.Value();

            if (_inputService.Use.IsPressed())
                _stationLeaveActionPool.Value.Add(stationEntity);

            if (_inputService.Mouse.IsHold())
                _stationWarmActionPool.Value.Add(stationEntity);
        }
    }
}