using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnrealTeam.SB.Common.Ecs;
using UnrealTeam.SB.GamePlay.CharacterController.Components;
using UnrealTeam.SB.GamePlay.CharacterController.Views;

namespace UnrealTeam.SB.GamePlay.CharacterController.Systems
{
    public class CharacterRotateSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<CharacterRotateAction>> _filterRotateCamera;
        private readonly EcsFilterInject<Inc<PlayerTag>> _filterMoveCamera;

        private readonly EcsPoolInject<CharacterControlData> _characterDataPool;
        private readonly EcsPoolInject<ComponentRef<CharacterView>> _characterRefPool;
        private readonly EcsPoolInject<ComponentRef<CameraView>> _cameraRefPool;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filterMoveCamera.Value)
            {
                UpdateMove(entity);
                
                UpdateRotation(entity);
            }
        }

        private void UpdateRotation(int entity)
        {
            ref var cameraView = ref _cameraRefPool.Value.Get(entity).Component;
            ref var characterData = ref _characterDataPool.Value.Get(entity);

            cameraView.UpdateRotate(characterData.LookDirection, Time.deltaTime);
        }

        private void UpdateMove(int entity)
        {
            ref var cameraView = ref _cameraRefPool.Value.Get(entity).Component;
            ref var characterView = ref _characterRefPool.Value.Get(entity).Component;

            cameraView.UpdateMove(characterView.Velocity);
        }
    }
}