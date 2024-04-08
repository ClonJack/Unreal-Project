using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnrealTeam.SB.Common.Ecs;
using UnrealTeam.SB.GamePlay.Interaction.Components;

namespace UnrealTeam.SB.GamePlay.Interaction.Systems
{
    public class OutlineInteractedSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<InteractAction, ComponentRef<QuickOutline>>> _interactFilter;
        private readonly EcsFilterInject<Inc<EndInteractAction, ComponentRef<QuickOutline>>> _endInteractFilter;
        private readonly EcsPoolInject<ComponentRef<QuickOutline>> _outlineRefPool;
        
        
        public void Run(IEcsSystems systems)
        {
            foreach (int interactEntity in _interactFilter.Value) 
                _outlineRefPool.Value.Get(interactEntity).Component.enabled = true;
            
            foreach (int endInteractEntity in _endInteractFilter.Value) 
                _outlineRefPool.Value.Get(endInteractEntity).Component.enabled = false;
        }
    }
}