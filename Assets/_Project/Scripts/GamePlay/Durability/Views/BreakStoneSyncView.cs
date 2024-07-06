using DestroyIt;
using Fusion;
using UnityEngine;

namespace UnrealTeam.SB.GamePlay.Durability.Views
{
    public class BreakStoneSyncView : NetworkBehaviour
    {
        [SerializeField] private DurabilitySyncView _durabilitySyncView;
        [SerializeField] private Destructible _destructible;
        [SerializeField] private float _stoneMass;
        [SerializeField] private int _minFragmentsCount;
        [SerializeField] private int _maxFragmentsCount;
        
        
        public override void Spawned()
        {
            base.Spawned();
            _durabilitySyncView.ZeroReached += BreakStoneRpc;
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        private void BreakStoneRpc()
        {
            if (HasStateAuthority)
                _destructible.Destroy();
            else
                _destructible.Destroy(true);
        }
    }
}