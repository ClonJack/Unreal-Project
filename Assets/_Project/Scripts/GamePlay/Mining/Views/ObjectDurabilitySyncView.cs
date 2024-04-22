using System;
using Fusion;
using TriInspector;
using UnityEngine;
using UnrealTeam.SB.GamePlay.Common.Views;

namespace UnrealTeam.SB.GamePlay.Mining.Views
{
    public class ObjectDurabilitySyncView : SyncNetworkBehaviour
    {
        [Networked] [field: ShowInInspector, Fusion.ReadOnly]
        public float Durability { get; private set; }

        [Networked] [field: ShowInInspector, Fusion.ReadOnly]
        public float MaxDurability { get; private set; }


        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void ReduceDurabilityRpc(float value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException();
            
            Durability = Mathf.Clamp(Durability - value, 0, MaxDurability);
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void InitDurabilityRpc(float value)
        {
            MaxDurability = value;
            Durability = value;
        }
    }
}