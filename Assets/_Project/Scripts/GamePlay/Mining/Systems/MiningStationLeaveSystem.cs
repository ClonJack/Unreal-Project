using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnrealTeam.SB.Additional.Enums;
using UnrealTeam.SB.Common.Ecs;
using UnrealTeam.SB.GamePlay.CharacterController.Components;
using UnrealTeam.SB.GamePlay.CharacterController.Views;
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
        private readonly EcsPoolInject<ControllableStationPlace> _controllableStationPlacePool;
        private readonly EcsPoolInject<ComponentRef<CharacterView>> _characterViewPool;

        
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
            
            stationSyncView.ChangeControlledByRpc(-1);
            stationSyncView.ChangePlayerIdRpc(-1);
            _stationControlledPool.Value.Del(stationEntity);

            ref var controllablePlace = ref _controllableStationPlacePool.Value.Get(stationEntity);

            ref var controllableStationPlace = ref _controllableStationPlacePool.Value.Get(stationEntity);

            if (controllableStationPlace.Collider != null)
                controllableStationPlace.Collider.enabled = true;

            ref var playerControlData = ref _playerControlPool.Value.Get(playerEntity);
            playerControlData.CurrentState = PlayerControlState.Character;
            _characterViewPool.Value.Get(playerEntity).Component.TeleportTo(controllablePlace.LastPosition);
        }
    }
}