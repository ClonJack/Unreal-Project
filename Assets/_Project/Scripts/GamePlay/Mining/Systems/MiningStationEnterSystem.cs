using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnrealTeam.SB.Additional.Enums;
using UnrealTeam.SB.Common.Ecs;
using UnrealTeam.SB.GamePlay.CharacterController.Components;
using UnrealTeam.SB.GamePlay.CharacterController.Views;
using UnrealTeam.SB.GamePlay.Interaction.Components;
using UnrealTeam.SB.GamePlay.Mining.Components;
using UnrealTeam.SB.GamePlay.Mining.Views;

namespace UnrealTeam.SB.GamePlay.Mining.Systems
{
    public class MiningStationEnterSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<UsedObjectAction, ComponentRef<MiningStationSyncView>>> _filter;
        private readonly EcsPoolInject<ComponentRef<MiningStationSyncView>> _stationSyncPool;
        private readonly EcsPoolInject<CharacterTeleportRequest> _teleportRequestPool;
        private readonly EcsPoolInject<UsedObjectAction> _usedActionPool;
        private readonly EcsPoolInject<MiningStationControlledMarker> _stationControlledPool;
        private readonly EcsPoolInject<PlayerControlData> _playerControlPool;
        private readonly EcsPoolInject<ControllableStationPlace> _controllableStationPlacePool;
        private readonly EcsPoolInject<ComponentRef<CharacterView>> _characterViewPool;


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
            
            ref var controllableStationPlace = ref _controllableStationPlacePool.Value.Get(stationEntity);

            if (controllableStationPlace.Collider != null)
                controllableStationPlace.Collider.enabled = false;

            stationSyncView.ChangeControlledByRpc(playerEntity);
            stationSyncView.ChangePlayerIdRpc(stationSyncView.Runner.LocalPlayer.PlayerId);
            _stationControlledPool.Value.Add(stationEntity);

            if (_controllableStationPlacePool.Value.Has(stationEntity))
            {
                ref var controllablePlace = ref _controllableStationPlacePool.Value.Get(stationEntity);
                controllablePlace.LastPosition =
                    _characterViewPool.Value.Get(playerEntity).Component.transform.position;
                _teleportRequestPool.Value.Add(playerEntity).StationEntity = stationEntity;
            }

            playerControlData.CurrentState = PlayerControlState.MiningStation;
        }
    }
}