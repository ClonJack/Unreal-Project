using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace UnrealTeam.SB.GamePlay.Common.Views
{
    public abstract class SyncNetworkBehaviour : NetworkBehaviour
    {
        [Header("Sync Settings"), Space(1)]
        [SerializeField] private ChangeDetector.Source _stateSource = ChangeDetector.Source.SimulationState;
        [SerializeField] private bool _invokeActionsOnSpawn = true;
        [HideInInspector]
        [SerializeField] private bool _hasNetworkedFields;
        
        
        private readonly Dictionary<string, Action> _networkedChangeActionsMap = new();
        private ChangeDetector _changeDetector;
        private IObjectResolver _objectResolver;


        [Inject]
        public void Construct(IObjectResolver objectResolver)
        {
            _objectResolver = objectResolver;
        }
        
        private void Awake()
        {
            InitNetworkedActions(_networkedChangeActionsMap);
        }

        public override void Spawned()
        {
            if (!_hasNetworkedFields)
                return;
            
            _changeDetector = GetChangeDetector(_stateSource);
            TryInvokeOnSpawn();
        }

        public override void Render()
        {
            if (!_hasNetworkedFields)
                return;
            
            foreach (var networkedName in _changeDetector.DetectChanges(this))
            {
                if (!_networkedChangeActionsMap.TryGetValue(networkedName, out var changeAction))
                    continue;
                
                changeAction.Invoke();
            }
        }
        
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        protected void InjectByNetworkIdRpc(NetworkId objectId)
        {
            var networkObject = Runner.FindObject(objectId);
            _objectResolver.InjectGameObject(networkObject.gameObject);
        }

        private void TryInvokeOnSpawn()
        {
            if (!_invokeActionsOnSpawn) 
                return;

            foreach (var changeAction in _networkedChangeActionsMap.Values) 
                changeAction.Invoke();
        }

        protected virtual void InitNetworkedActions(Dictionary<string, Action> networkedChangeActionsMap){}
    }
}