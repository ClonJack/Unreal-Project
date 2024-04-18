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
        private readonly EcsFilterInject<Inc<CharacterRotateAction>> _filter;
        private readonly EcsPoolInject<CharacterControlData> _characterDataPool;
        private readonly EcsPoolInject<ComponentRef<CharacterView>> _characterRefPool;
        private readonly EcsPoolInject<ComponentRef<CameraView>> _cameraRefPool;
        

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value) 
                UpdateRotation(entity);
        }

        private void UpdateRotation(int entity)
        {
            ref var cameraView = ref _cameraRefPool.Value.Get(entity).Component;
            ref var characterData = ref _characterDataPool.Value.Get(entity);
            
            cameraView.UpdateRotateAndPosition(characterData.LookDirection, Time.deltaTime);
        }
    }
}