using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnrealTeam.SB.Common.Ecs;
using UnrealTeam.SB.Common.Ecs.Binders;
using UnrealTeam.SB.Common.Ecs.Extensions;
using UnrealTeam.SB.Common.Extensions;
using UnrealTeam.SB.GamePlay.CharacterController.Views;
using UnrealTeam.SB.GamePlay.Interaction.Components;

namespace UnrealTeam.SB.GamePlay.Interaction.Systems
{
    public class InteractionSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<ComponentRef<CameraView>>> _cameraFilter;
        private readonly EcsFilterInject<Inc<InteractAction>> _actionFilter;
        private readonly EcsPoolInject<ComponentRef<CameraView>> _cameraPool;
        private readonly EcsPoolInject<InteractAction> _actionPool;
        private readonly EcsPoolInject<EndInteractAction> _endActionPool;
        private readonly EcsPoolInject<NotInteractableObjectTag> _notInteractablePool;
        
        
        public void Run(IEcsSystems systems)
        {
            foreach (var actionEntity in _actionFilter.Value)
            {
                _actionPool.Value.Del(actionEntity);
                _endActionPool.Value.Add(actionEntity);
            }
            
            foreach (var interactor in _cameraFilter.Value) 
                RaycastObjectsFromCamera(interactor);
        }

        private void RaycastObjectsFromCamera(int interactor)
        {
            var cameraView = _cameraPool.Value.Get(interactor).Component;
            var cameraTransform = cameraView.transform;
            var cameraDirection = cameraTransform.forward;

            var isRaycasted = Physics.Raycast(cameraTransform.position, cameraDirection, out var hit, cameraView.InteractionDistance, cameraView.InteractableLayer);
            if (!isRaycasted) 
                return;
            
            var interactedObjectEntity = hit.transform.GetComponent<EcsEntityProvider>().Entity;
            if (_notInteractablePool.Value.Has(interactedObjectEntity))
                return;
            
            _endActionPool.Value.SafeDel(interactedObjectEntity);
            _actionPool.Value.Add(interactedObjectEntity).InteractedBy = interactor;
        }
    }
}