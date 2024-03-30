using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnrealTeam.SB.Common.Ecs;
using UnrealTeam.SB.Common.Ecs.Providers;
using UnrealTeam.SB.GamePlay.Components;
using UnrealTeam.SB.GamePlay.Views;

namespace UnrealTeam.SB.GamePlay.Systems.Interaction
{
    public class RaycastObjectsSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<ComponentRef<CameraView>>> _filter;
        private readonly EcsPoolInject<ComponentRef<CameraView>> _cameraPool;
        private readonly EcsPoolInject<RaycastedObjectComponent> _raycastedObjectPool;
        
        
        public void Run(IEcsSystems systems)
        {
            foreach (int raycastCamera in _filter.Value) 
                RaycastObjectsFromCamera(raycastCamera);
        }

        private void RaycastObjectsFromCamera(int raycastCamera)
        {
            CameraView cameraView = _cameraPool.Value.Get(raycastCamera).Component;
            Transform cameraTransform = cameraView.transform;
            
            Vector3 cameraDirection = cameraTransform.TransformDirection(Vector3.forward);
            Debug.DrawRay(cameraTransform.position, cameraDirection * cameraView.InteractionDistance, Color.green);
            if (Physics.Raycast(cameraTransform.position, cameraDirection, out RaycastHit hit, cameraView.InteractionDistance, cameraView.InteractionLayer))
            { 
                int raycastedEntity = hit.transform.GetComponent<EcsEntityProvider>().Entity;
                _raycastedObjectPool.Value.Get(raycastedEntity).Raycasted = true;
            }
        }
    }
}