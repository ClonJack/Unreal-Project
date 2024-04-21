using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;

namespace UnrealTeam.SB.Common.Game
{
    public class SyncNetworkBehaviour : NetworkBehaviour
    {
        [SerializeField] private ChangeDetector.Source _stateSource = ChangeDetector.Source.SimulationState;
        [SerializeField] private bool _invokeActionsOnSpawn = true;
        
        private readonly Dictionary<string, Action> _networkedChangeActionsMap = new();
        private ChangeDetector _changeDetector;


        private void Awake()
        {
            InitNetworkedActions(_networkedChangeActionsMap);
        }

        public override void Spawned()
        {
            _changeDetector = GetChangeDetector(_stateSource);
            TryInvokeOnSpawn();
        }

        public override void Render()
        {
            foreach (var networkedName in _changeDetector.DetectChanges(this))
            {
                if (!_networkedChangeActionsMap.TryGetValue(networkedName, out Action changeAction))
                {
                    Debug.LogWarning($"Change action on Networked Var {networkedName} is not registered. Script {name}");
                    continue;
                }
                
                changeAction.Invoke();
            }
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