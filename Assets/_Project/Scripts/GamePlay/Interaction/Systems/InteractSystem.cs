using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnrealTeam.SB.Common.Ecs;
using UnrealTeam.SB.Common.Ecs.Binders;
using UnrealTeam.SB.Common.Extensions;
using UnrealTeam.SB.GamePlay.CharacterController.Views;
using UnrealTeam.SB.GamePlay.Interaction.Components;

namespace UnrealTeam.SB.GamePlay.Interaction.Systems
{
    public class InteractSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<ComponentRef<CameraView>>> _cameraFilter;
        private readonly EcsFilterInject<Inc<InteractAction>> _actionFilter;
        private readonly EcsPoolInject<ComponentRef<CameraView>> _cameraPool;
        private readonly EcsPoolInject<InteractAction> _actionPool;
        private readonly EcsPoolInject<EndInteractAction> _endActionPool;
        
        
        public void Run(IEcsSystems systems)
        {
            foreach (int actionEntity in _actionFilter.Value)
            {
                _actionPool.Value.Del(actionEntity);
                _endActionPool.Value.Add(actionEntity);
            }
            
            foreach (int interactCamera in _cameraFilter.Value) 
                RaycastObjectsFromCamera(interactCamera);
        }

        private void RaycastObjectsFromCamera(int raycastCamera)
        {
            var cameraView = _cameraPool.Value.Get(raycastCamera).Component;
            var cameraTransform = cameraView.transform;
            var cameraDirection = cameraTransform.forward;
            
            Debug.DrawRay(cameraTransform.position, cameraDirection * cameraView.InteractionDistance, Color.green);
            
            if (Physics.Raycast(cameraTransform.position, cameraDirection, out var hit, cameraView.InteractionDistance, cameraView.InteractionLayer))
            { 
                var entity = hit.transform.GetComponent<EcsEntityProvider>().Entity;
                
                _endActionPool.Value.SafeDel(entity);
                _actionPool.Value.Add(entity);
            }
        }
    }
}