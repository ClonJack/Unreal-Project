using DestroyIt;
using Fusion;
using UnityEngine;
using UnrealTeam.SB.Common.Utils;
using UnrealTeam.SB.Services.Network;
using VContainer;
using VContainer.Unity;

namespace UnrealTeam.SB.GamePlay.Durability.Views
{
    public class BreakStoneSyncView : NetworkBehaviour
    {
        [SerializeField] private DurabilitySyncView _durabilitySyncView;
        [SerializeField] private Destructible _destructible;
        [SerializeField] private GameObject[] _replaceOnDestroy = {};
        [SerializeField] private float _stoneMass;
        [SerializeField] private int _minFragmentsCount;
        [SerializeField] private int _maxFragmentsCount;
        
        private IObjectResolver _objectResolver;
        private NetworkStateMachine _networkStateMachine;


        [Inject]
        public void Construct(IObjectResolver objectResolver, NetworkStateMachine networkStateMachine)
        {
            _networkStateMachine = networkStateMachine;
            _objectResolver = objectResolver;
        }
        
        public override void Spawned()
        {
            base.Spawned();
            _durabilitySyncView.ZeroReached += BreakStoneRpc;
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        private void BreakStoneRpc()
        {
            _destructible.Destroy();
            GameObject replacedObject = null;

            if (HasStateAuthority)
            {
                var replaceWithPrefab = RandomUtils.PickOne(_replaceOnDestroy);
                var networkRunner = _networkStateMachine.NetworkRunner;
                var objectId = networkRunner.Spawn(replaceWithPrefab, transform.position, Quaternion.identity).Id;
                InjectInReplacedObjectRpc(objectId);
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void InjectInReplacedObjectRpc(NetworkId objectId)
        {
            var networkObject = _networkStateMachine.NetworkRunner.FindObject(objectId);
            _objectResolver.InjectGameObject(networkObject.gameObject);
        }
    }
}