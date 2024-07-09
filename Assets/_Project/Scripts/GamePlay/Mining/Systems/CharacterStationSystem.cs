using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnrealTeam.SB.Common.Ecs;
using UnrealTeam.SB.GamePlay.CharacterController.Views;
using UnrealTeam.SB.GamePlay.Mining.Components;

namespace UnrealTeam.SB.GamePlay.Mining.Systems
{
    public class CharacterStationSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<CharacterEnterRequest, ComponentRef<CharacterView>>> _filterEnter;
        private readonly EcsFilterInject<Inc<CharacterExitRequest, ComponentRef<CharacterView>>> _filterExit;
        private readonly EcsPoolInject<CharacterEnterRequest> _characterEnterRequestPool;
        private readonly EcsPoolInject<CharacterExitRequest> _characterExitRequestPool;
        private readonly EcsPoolInject<ComponentRef<CharacterView>> _characterViewPool;
        private readonly EcsPoolInject<ControllableStationPlace> _controllableStationPlacePool;

        public void Run(IEcsSystems systems)
        {
            foreach (var characterEntity in _filterEnter.Value)
            {
                ProcessEnterRequest(characterEntity);
            }

            foreach (var characterEntity in _filterExit.Value)
            {
                ProcessExitRequest(characterEntity);
            }
        }

        private void ProcessEnterRequest(int entity)
        {
            ref var stationEntity = ref _characterEnterRequestPool.Value.Get(entity).StationEntity;
            HandleStationAction(entity, stationEntity, true);
        }

        private void ProcessExitRequest(int entity)
        {
            ref var stationEntity = ref _characterExitRequestPool.Value.Get(entity).StationEntity;
            HandleStationAction(entity, stationEntity, false);
        }

        private void HandleStationAction(int characterEntity, int stationEntity, bool enterStation)
        {
            ref var characterView = ref _characterViewPool.Value.Get(characterEntity).Component;
            ref var controllableStationPlace = ref _controllableStationPlacePool.Value.Get(stationEntity);

            if (controllableStationPlace.Collider != null)
                controllableStationPlace.Collider.enabled = !enterStation;

            if (enterStation)
            {
                characterView.EnterStation(controllableStationPlace.SitPoint);
                return;
            }

            characterView.ExitStation(controllableStationPlace.LastPlacePlayer.Position,
                controllableStationPlace.LastPlacePlayer.Rotate);
        }
    }
}