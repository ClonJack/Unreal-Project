using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnrealTeam.SB.Common.Ecs;
using UnrealTeam.SB.Common.Enums;
using UnrealTeam.SB.GamePlay.CharacterController.Components;
using UnrealTeam.SB.GamePlay.Interaction.Components;
using UnrealTeam.SB.GamePlay.Mining.Components;
using UnrealTeam.SB.GamePlay.Mining.Views;

namespace UnrealTeam.SB.GamePlay.Mining.Systems
{
    public class MiningStationEnterSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<UsedObjectAction, ComponentRef<MiningStationSyncView>>> _filter;
        private readonly EcsPoolInject<ComponentRef<MiningStationSyncView>> _stationSyncPool;
        private readonly EcsPoolInject<UsedObjectAction> _usedActionPool;
        private readonly EcsPoolInject<MiningStationControlledMarker> _stationControlledPool;
        private readonly EcsPoolInject<PlayerControlData> _playerControlPool;

        
        public void Run(IEcsSystems systems)
        {
            foreach (var stationEntity in _filter.Value) 
                TryUseStation(stationEntity);
        }

        private void TryUseStation(int stationEntity)
        {
            var stationSyncView = _stationSyncPool.Value.Get(stationEntity).Component;
            if (stationSyncView.ControlledBy >= 0)
                return;
                    
            var playerEntity = _usedActionPool.Value.Get(stationEntity).UsedBy;
            ref var playerControlData = ref _playerControlPool.Value.Get(playerEntity);

            stationSyncView.Object.ReleaseStateAuthority();
            stationSyncView.Object.RequestStateAuthority();
            stationSyncView.ControlledBy = playerEntity;
            _stationControlledPool.Value.Add(stationEntity);
            
            playerControlData.CurrentState = PlayerControlState.MiningStation;
        }
    }
}