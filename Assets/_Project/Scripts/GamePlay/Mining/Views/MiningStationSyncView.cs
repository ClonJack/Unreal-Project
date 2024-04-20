using System;
using System.Collections.Generic;
using Fusion;
using Leopotam.EcsLite;
using TriInspector;
using UnityEngine;
using UnrealTeam.SB.Common.Ecs.Binders;
using UnrealTeam.SB.Common.Extensions;
using UnrealTeam.SB.Common.Game;
using UnrealTeam.SB.GamePlay.Interaction.Components;
using VContainer;

namespace UnrealTeam.SB.GamePlay.Mining.Views
{
    public class MiningStationSyncView : SyncNetworkBehaviour
    {
        [Networked]
        [field: ShowInInspector, Fusion.ReadOnly]
        public int ControlledBy { get; set; } = -1;

        [SerializeField] private EcsEntityProvider _entityProvider;

        private EcsWorld _ecsWorld;


        [Inject]
        public void Construct(EcsWorld ecsWorld)
        {
            _ecsWorld = ecsWorld;
        }

        protected override void InitNetworkedActions(Dictionary<string, Action> networkedChangeActionsMap)
        {
            networkedChangeActionsMap.Add(nameof(ControlledBy), OnControlledByChanged);
        }

        private void OnControlledByChanged()
        {
            var notInteractablePool = _ecsWorld.GetPool<NotInteractableObjectTag>();
            var stationEntity = _entityProvider.Entity;

            if (ControlledBy < 0)
                notInteractablePool.SafeDel(stationEntity);
            else
                notInteractablePool.GetOrAdd(stationEntity);
        }
    }
}