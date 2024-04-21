using Fusion;
using UnityEngine;

namespace UnrealTeam.SB.Common.Game
{
    public class SyncTransform : NetworkBehaviour
    {
        [SerializeField] private Transform _transform;

        private Quaternion _targetRotation;
        private Vector3 _rotationOffset;


        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void RotateRpc(Vector3 rotationOffset)
        {
            _rotationOffset = rotationOffset;
            _targetRotation = _transform.localRotation * Quaternion.Euler(_rotationOffset);
        }

        public override void Spawned()
        {
            _targetRotation = _transform.localRotation;
        }

        public override void FixedUpdateNetwork()
        {
            if (!HasStateAuthority)
                return;
            
            if (_transform.localRotation == _targetRotation)
                return;
            
            _transform.Rotate(_rotationOffset * Runner.DeltaTime);
        }
    }
}