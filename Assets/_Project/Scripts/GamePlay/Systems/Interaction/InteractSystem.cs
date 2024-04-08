using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnrealTeam.SB.Common.Ecs;
using UnrealTeam.SB.Common.Ecs.Binders;
using UnrealTeam.SB.Common.Extensions;
using UnrealTeam.SB.GamePlay.Components;
using UnrealTeam.SB.GamePlay.Views;

namespace UnrealTeam.SB.GamePlay.Systems.Interaction
{
    public class InteractSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<ComponentRef<CameraView>>> _filter;
        private readonly EcsPoolInject<ComponentRef<CameraView>> _cameraPool;
        private readonly EcsPoolInject<InteractAction> _interactPool;
        private readonly EcsPoolInject<EndInteractAction> _endInteractPool;
        
        
        public void Run(IEcsSystems systems)
        {
            foreach (int interactCamera in _filter.Value) 
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
                
                _endInteractPool.Value.SafeDel(entity);
                _interactPool.Value.Add(entity);
            }
        }
    }
}