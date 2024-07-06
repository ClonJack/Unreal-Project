using System;
using System.Collections.Generic;
using Fusion;
using Leopotam.EcsLite;
using TriInspector;
using UnityEngine;
using UnrealTeam.SB.Common.Ecs.Binders;
using UnrealTeam.SB.Common.Ecs.Extensions;
using UnrealTeam.SB.GamePlay.Common.Views;
using UnrealTeam.SB.GamePlay.Interaction.Components;
using UnrealTeam.SB.GamePlay.Mining.Components;
using UnrealTeam.SB.Services.Network;
using VContainer;

namespace UnrealTeam.SB.GamePlay.Mining.Views
{
    public class MiningStationSyncView : SyncNetworkBehaviour
    {
        [Networked] [field: ShowInInspector, Fusion.ReadOnly]
        public int ControlledBy { get; private set; } = -1;

        [Networked] [field: ShowInInspector, Fusion.ReadOnly]
        public int PlayerId { get; private set; } = -1;

        
        [SerializeField] 
        private EcsEntityProvider _entityProvider;

        private NetworkStateMachine _networkStateMachine;
        private EcsPool<NotInteractableObjectTag> _notInteractablePool;
        private EcsPool<MiningLaserWarmedEvent> _laserWarmedEventPool;
        private EcsPool<MiningLaserCooledEvent> _laserCooledEventPool;


        [Inject]
        public void Construct(EcsWorld ecsWorld, NetworkStateMachine networkStateMachine)
        {
            _networkStateMachine = networkStateMachine;
            _notInteractablePool = ecsWorld.GetPool<NotInteractableObjectTag>();
            _laserWarmedEventPool = ecsWorld.GetPool<MiningLaserWarmedEvent>();
            _laserCooledEventPool = ecsWorld.GetPool<MiningLaserCooledEvent>();
        }

        public override void Spawned()
        {
            base.Spawned();
            _networkStateMachine.OnPlayerLeave += OnPlayerLeft;
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            _networkStateMachine.OnPlayerLeave -= OnPlayerLeft;
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void ChangeControlledByRpc(int playerEntity)
            => ControlledBy = playerEntity;

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void ChangePlayerIdRpc(int playerId)
            => PlayerId = playerId;

        [Rpc(RpcSources.All, RpcTargets.All)]
        public void ChangeLaserPowerRpc(float powerCoefficient)
        {
            var stationEntity = _entityProvider.Entity;

            if (powerCoefficient == 0)
                _laserCooledEventPool.GetOrAdd(stationEntity);
            else if (powerCoefficient > 1)
                throw new InvalidOperationException();
            else
                _laserWarmedEventPool.GetOrAdd(stationEntity).PowerCoefficient = powerCoefficient;
        }

        protected override void InitNetworkedActions(Dictionary<string, Action> networkedChangeActionsMap)
        {
            networkedChangeActionsMap.Add(nameof(ControlledBy), OnControlledByChanged);
        }

        private void OnControlledByChanged()
        {
            var stationEntity = _entityProvider.Entity;

            if (ControlledBy < 0)
                _notInteractablePool.SafeDel(stationEntity);
            else
                _notInteractablePool.GetOrAdd(stationEntity);
        }

        private void OnPlayerLeft(NetworkRunner runner, PlayerRef leftPlayer)
        {
            if (PlayerId == -1 || leftPlayer.PlayerId != PlayerId)
                return;
            
            ChangeControlledByRpc(-1);
            ChangePlayerIdRpc(-1);
        }
    }
}