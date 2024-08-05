using UnityEngine;
using UnrealTeam.SB.Common.Ecs.Binders;
using UnrealTeam.SB.GamePlay.Durability.Views;

namespace UnrealTeam.SB.GamePlay.Durability.Binders
{
    public class DurabilitySyncBinder : EcsComponentRefBinder<DurabilitySyncView>
    {
        [SerializeField] private float _durability;
        
        
        protected override void InitView(DurabilitySyncView component)
        {
            component.ObjectSpawned += OnNetworkObjectSpawned;
            return;

            void OnNetworkObjectSpawned()
            {
                component.ObjectSpawned -= OnNetworkObjectSpawned;
                if (component.HasStateAuthority)
                    component.InitDurabilityRpc(_durability);
            }
        }
    }
}