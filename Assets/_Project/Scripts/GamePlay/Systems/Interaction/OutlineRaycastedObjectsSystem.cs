using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnrealTeam.SB.Common.Ecs;
using UnrealTeam.SB.GamePlay.Components;

namespace UnrealTeam.SB.GamePlay.Systems.Interaction
{
    public class OutlineRaycastedObjectsSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<RaycastedObjectComponent>> _filter;
        private readonly EcsPoolInject<RaycastedObjectComponent> _raycastedObjectPool;
        private readonly EcsPoolInject<ComponentRef<QuickOutline>> _outlineRefPool;
        
        
        public void Run(IEcsSystems systems)
        {
            foreach (int raycastedObject in _filter.Value)
                OutlineAndCleanObjects(raycastedObject);
        }

        private void OutlineAndCleanObjects(int raycastedObject)
        {
            if (!_outlineRefPool.Value.Has(raycastedObject))
                return;

            bool isRaycasted = _raycastedObjectPool.Value.Get(raycastedObject).Raycasted;
            _outlineRefPool.Value.Get(raycastedObject).Component.enabled = isRaycasted;
        }
    }
}