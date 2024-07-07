using Fusion;
using UnityEngine;

namespace UnrealTeam.SB.GamePlay.Common.Views
{
    [RequireComponent(typeof(NetworkTransform))]
    public class SyncRotationView : NetworkBehaviour
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

        public void Update()
        {
            if (!HasStateAuthority)
                return;

            if (Quaternion.Angle(_transform.localRotation, _targetRotation) < 1f)
                return;

            _transform.rotation *= Quaternion.Euler(_rotationOffset * Time.deltaTime);
        }
    }
}