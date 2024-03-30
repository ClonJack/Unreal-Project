using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnrealTeam.SB.GamePlay.Components;

namespace UnrealTeam.SB.GamePlay.Systems.Interaction
{
    public class CleanupRaycastedObjectsSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<RaycastedObjectComponent>> _filter;
        private readonly EcsPoolInject<RaycastedObjectComponent> _raycastedObjectPool;
        
        
        public void Run(IEcsSystems systems)
        {
            foreach (int raycastedObject in _filter.Value) 
                CleanupRaycastedObjects(raycastedObject);
        }

        private void CleanupRaycastedObjects(int raycastedObject) 
            => _raycastedObjectPool.Value.Get(raycastedObject).Raycasted = false;
    }
}