using Fusion;
using Leopotam.EcsLite;
using TriInspector;
using UnityEngine;
using UnrealTeam.SB.Common.Ecs.Binders;
using UnrealTeam.SB.Common.Extensions;
using UnrealTeam.SB.GamePlay.Interaction.Components;
using VContainer;

namespace UnrealTeam.SB.GamePlay.Mining.Views
{
    public class MiningStationSyncView : NetworkBehaviour
    {
        [Networked]
        [field: ShowInInspector, Fusion.ReadOnly]
        public int ControlledBy { get; set; } = -1;

        [SerializeField] private EcsEntityProvider _entityProvider;

        private EcsWorld _ecsWorld;
        private ChangeDetector _changeDetector;


        [Inject]
        public void Construct(EcsWorld ecsWorld)
        {
            _ecsWorld = ecsWorld;
        }

        public override void Spawned()
        {
            _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
            OnControlledEntityChanged();
        }

        public override void Render()
        {
            foreach (var change in _changeDetector.DetectChanges(this))
            {
                switch (change)
                {
                    case nameof(ControlledBy):
                        OnControlledEntityChanged();
                        break;
                }
            }
        }

        private void OnControlledEntityChanged()
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