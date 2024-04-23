using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnrealTeam.SB.Common.Ecs;
using UnrealTeam.SB.GamePlay.Durability.Components;
using UnrealTeam.SB.GamePlay.Durability.Views;

namespace UnrealTeam.SB.GamePlay.Durability.Systems
{
    public class DurabilityChangeSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<DurabilityChangeRequest>> _requestFilter;
        private readonly EcsPoolInject<DurabilityChangeRequest> _changeRequestPool;
        private readonly EcsPoolInject<ComponentRef<DurabilitySyncView>> _syncViewPool;
        
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _requestFilter.Value)
            {
                ref var changeRequest = ref _changeRequestPool.Value.Get(entity);
                var durabilityView = _syncViewPool.Value.Get(entity).Component;
                
                durabilityView.ChangeDurabilityRpc(changeRequest.DurabilityDiff);
            }
        }
    }
}