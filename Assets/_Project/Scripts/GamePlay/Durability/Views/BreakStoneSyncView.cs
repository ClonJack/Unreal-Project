using DestroyIt;
using Fusion;
using UnityEngine;
using UnrealTeam.SB.Common.Utils;
using UnrealTeam.SB.GamePlay.Common.Views;

namespace UnrealTeam.SB.GamePlay.Durability.Views
{
    public class BreakStoneSyncView : SyncNetworkBehaviour
    {
        [SerializeField] private DurabilitySyncView _durabilitySyncView;
        [SerializeField] private Destructible _destructible;
        [SerializeField] private GameObject[] _replaceOnDestroy = {};
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
            _destructible.Destroy();
            if (HasStateAuthority)
            {
                var replaceWithPrefab = RandomUtils.PickOne(_replaceOnDestroy);
                var objectId = Runner.Spawn(replaceWithPrefab, transform.position, Quaternion.identity).Id;
                InjectByNetworkIdRpc(objectId);
            }
        }
    }
}