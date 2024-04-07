using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnrealTeam.SB.GamePlay.Components;

namespace UnrealTeam.SB.GamePlay.Systems.Interaction
{
    public class CleanupInteractSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<InteractAction>> _interactFilter;
        private readonly EcsFilterInject<Inc<EndInteractAction>> _endInteractFilter;
        private readonly EcsPoolInject<InteractAction> _interactPool;
        private readonly EcsPoolInject<EndInteractAction> _endInteractPool;
        
        
        public void Run(IEcsSystems systems)
        {
            foreach (int endInteractEntity in _endInteractFilter.Value) 
                _endInteractPool.Value.Del(endInteractEntity);
            
            foreach (int interactEntity in _interactFilter.Value)
            {
                _interactPool.Value.Del(interactEntity);
                _endInteractPool.Value.Add(interactEntity);
            }
        }
    }
}