using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnrealTeam.SB.Common.Ecs;
using UnrealTeam.SB.Common.Ecs.Providers;
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
            CameraView cameraView = _cameraPool.Value.Get(raycastCamera).Component;
            Transform cameraTransform = cameraView.transform;
            Vector3 cameraDirection = cameraTransform.TransformDirection(Vector3.forward);
            
            Debug.DrawRay(cameraTransform.position, cameraDirection * cameraView.InteractionDistance, Color.green);
            
            if (Physics.Raycast(cameraTransform.position, cameraDirection, out RaycastHit hit, cameraView.InteractionDistance, cameraView.InteractionLayer))
            { 
                int entity = hit.transform.GetComponent<EcsEntityProvider>().Entity;
                _endInteractPool.Value.SafeDel(entity);
                _interactPool.Value.Add(entity);
            }
        }
    }
}