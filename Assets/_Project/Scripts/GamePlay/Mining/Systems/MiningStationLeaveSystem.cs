using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnrealTeam.SB.Common.Ecs;
using UnrealTeam.SB.Common.Enums;
using UnrealTeam.SB.GamePlay.CharacterController.Components;
using UnrealTeam.SB.GamePlay.Mining.Components;
using UnrealTeam.SB.GamePlay.Mining.Views;

namespace UnrealTeam.SB.GamePlay.Mining.Systems
{
    public class MiningStationLeaveSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<MiningStationLeaveAction, MiningStationControlledMarker>> _filter;
        private readonly EcsPoolInject<ComponentRef<MiningStationSyncView>> _stationSyncPool;
        private readonly EcsPoolInject<MiningStationControlledMarker> _stationControlledPool;
        private readonly EcsPoolInject<PlayerControlData> _playerControlPool;

        
        public void Run(IEcsSystems systems)
        {
            foreach (var stationEntity in _filter.Value) 
                LeaveStation(stationEntity);
        }

        private void LeaveStation(int stationEntity)
        {
            var stationSyncView = _stationSyncPool.Value.Get(stationEntity).Component;
            var playerEntity = stationSyncView.ControlledBy;
            if (playerEntity < 0)
                throw new InvalidOperationException();
            
            stationSyncView.ControlledBy = -1;
            stationSyncView.Object.ReleaseStateAuthority();
            _stationControlledPool.Value.Del(stationEntity);

            ref var playerControlData = ref _playerControlPool.Value.Get(playerEntity);
            playerControlData.CurrentState = PlayerControlState.Character;
        }
    }
}