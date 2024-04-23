using System;
using System.Collections.Generic;
using Fusion;
using Leopotam.EcsLite;
using TriInspector;
using UnityEngine;
using UnrealTeam.SB.Common.Ecs.Binders;
using UnrealTeam.SB.Common.Ecs.Extensions;
using UnrealTeam.SB.GamePlay.Common.Views;
using UnrealTeam.SB.GamePlay.Durability.Components;
using VContainer;

namespace UnrealTeam.SB.GamePlay.Durability.Views
{
    public class DurabilitySyncView : SyncNetworkBehaviour
    {
        [Networked] [field: ShowInInspector, Fusion.ReadOnly]
        public float Durability { get; private set; }

        [Networked] [field: ShowInInspector, Fusion.ReadOnly]
        public float MaxDurability { get; private set; }

        
        [SerializeField] 
        private EcsEntityProvider _entityProvider;
        
        private EcsPool<DurabilityChangedEvent> _changedEventPool;

        public event Action ObjectSpawned;


        [Inject]
        public void Construct(EcsWorld ecsWorld)
        {
            _changedEventPool = ecsWorld.GetPool<DurabilityChangedEvent>();
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void ChangeDurabilityRpc(float value)
        {
            if (MaxDurability == 0)
                throw new InvalidOperationException();
            
            Durability = Mathf.Clamp(Durability + value, 0, MaxDurability);
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void InitDurabilityRpc(float value)
        {
            MaxDurability = value;
            Durability = value;
        }

        public override void Spawned()
        {
            base.Spawned();
            ObjectSpawned?.Invoke();
        }

        protected override void InitNetworkedActions(Dictionary<string, Action> networkedChangeActionsMap)
        {
            networkedChangeActionsMap.Add(nameof(Durability), OnDurabilityChanged);
        }

        private void OnDurabilityChanged()
        {
            ref var changedEvent = ref _changedEventPool.GetOrAdd(_entityProvider.Entity);
            changedEvent.Durability = Durability;
            changedEvent.MaxDurability = MaxDurability;
        }
    }
}