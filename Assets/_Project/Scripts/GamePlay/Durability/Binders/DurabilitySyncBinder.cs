using UnityEngine;
using UnrealTeam.SB.Common.Ecs.Binders;
using UnrealTeam.SB.Configs.Mining;
using UnrealTeam.SB.GamePlay.Durability.Views;

namespace UnrealTeam.SB.GamePlay.Durability.Binders
{
    public class DurabilitySyncBinder : EcsComponentRefBinder<DurabilitySyncView>
    {
        [SerializeField] private DurabilityConfig _config;
        
        
        protected override void InitView(DurabilitySyncView component)
        {
            component.ObjectSpawned += OnNetworkObjectSpawned;
            return;

            void OnNetworkObjectSpawned()
            {
                component.ObjectSpawned -= OnNetworkObjectSpawned;
                if (component.HasStateAuthority)
                    component.InitDurabilityRpc(_config.MaxDurability);
            }
        }
    }
}