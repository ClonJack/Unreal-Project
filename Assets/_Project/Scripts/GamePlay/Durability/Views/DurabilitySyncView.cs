using System;
using System.Collections.Generic;
using Fusion;
using Leopotam.EcsLite;
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
        [SerializeField] private EcsEntityProvider _entityProvider;

        
        [field: HideInInspector]
        [Networked] public float Durability { get; private set; }
        
        [field: HideInInspector]
        [Networked] public float MaxDurability { get; private set; }
        
        [field: HideInInspector]
        [Networked] public bool IsInitialized { get; private set; }
        
        
        private EcsPool<DurabilityChangedEvent> _changedEventPool;

        public event Action ObjectSpawned;
        public event Action ZeroReached;


        [Inject]
        public void Construct(EcsWorld ecsWorld)
        {
            _changedEventPool = ecsWorld.GetPool<DurabilityChangedEvent>();
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void ChangeDurabilityRpc(float diff)
        {
            if (!IsInitialized || Durability == 0)
                return;
                
            Durability = Mathf.Clamp(Durability + diff, 0, MaxDurability);
            if (Durability == 0)
                ZeroReached?.Invoke();
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void InitDurabilityRpc(float value)
        {
            if (IsInitialized || value == 0)
                return;
            
            MaxDurability = value;
            Durability = value;
            IsInitialized = true;
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