using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnrealTeam.SB.Common.Ecs;
using UnrealTeam.SB.GamePlay.CharacterController.Views;
using UnrealTeam.SB.GamePlay.Interaction.Components;
using UnrealTeam.SB.GamePlay.Mining.Components;

namespace UnrealTeam.SB.GamePlay.Mining.Systems
{
    public class CharacterTeleportSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<CharacterTeleportRequest, ComponentRef<CharacterView>>> _filter;
        private readonly EcsPoolInject<CharacterTeleportRequest> _teleportRequestPool;
        private readonly EcsPoolInject<ComponentRef<CharacterView>> _characterViewPool;
        private readonly EcsPoolInject<ComponentRef<CameraView>> _cameraViewPool;
        private readonly EcsPoolInject<ControllableStationPlace> _controllableStationPlacePool;


        public void Run(IEcsSystems systems)
        {
            foreach (var characterEntity in _filter.Value)
            {
                Teleport(characterEntity);
            }
        }

        private void Teleport(int characterEntity)
        {
            var characterView = _characterViewPool.Value.Get(characterEntity).Component;
            var stationEntity = _teleportRequestPool.Value.Get(characterEntity).StationEntity;
            ref var controllableStationPlace = ref _controllableStationPlacePool.Value.Get(stationEntity);

            characterView.TeleportTo(controllableStationPlace.SitPoint);
            characterView.transform.localRotation = Quaternion.identity;
        }
    }
}