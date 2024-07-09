using UnityEngine;

namespace UnrealTeam.SB.GamePlay.CharacterController.Views
{
    public class CameraView : MonoBehaviour
    {
        [field: Header("Ref")]
        [field: SerializeField]
        public Transform FollowTransform { get; private set; }

        [Header("Rotation")] [Range(-90f, 90f)]
        public float DefaultVerticalAngle = 20f;

        [Range(-90f, 90f)] public float MinVerticalAngle = -90f;
        [Range(-90f, 90f)] public float MaxVerticalAngle = 90f;

        public float RotationSpeed = 1f;
        public float RotationSharpness = 10000f;

        [field: Header("Position")]
        [field: SerializeField]
        public float StartOffset { get; set; }

        [field: Header("Interaction")]
        [field: SerializeField]
        public float InteractionDistance { get; private set; }

        [field: SerializeField] public LayerMask InteractableLayer { get; private set; }

        public Vector3 PlanarDirection { get; set; }

        private bool _distanceIsObstructed;
        private float _currentDistance;
        private float _targetVerticalAngle;

        private void OnValidate()
        {
            DefaultVerticalAngle = Mathf.Clamp(DefaultVerticalAngle, MinVerticalAngle, MaxVerticalAngle);
        }

        private void Awake()
        {
            _targetVerticalAngle = 0f;

            PlanarDirection = FollowTransform.forward;

            transform.position = FollowTransform.position + FollowTransform.forward * StartOffset;
        }

        public void UpdateRotate(Vector3 rotationInput, float deltaTime)
        {
            if (FollowTransform == null) return;

            var rotationFromInput =
                Quaternion.Euler(FollowTransform.up * (rotationInput.x * deltaTime * RotationSpeed));
            PlanarDirection = rotationFromInput * PlanarDirection;
            PlanarDirection = Vector3.Cross(FollowTransform.up, Vector3.Cross(PlanarDirection, FollowTransform.up));
            var planarRot = Quaternion.LookRotation(PlanarDirection, FollowTransform.up);

            _targetVerticalAngle -= (rotationInput.y * deltaTime * RotationSpeed);
            _targetVerticalAngle = Mathf.Clamp(_targetVerticalAngle, MinVerticalAngle, MaxVerticalAngle);

            var verticalRot = Quaternion.Euler(_targetVerticalAngle, 0, 0);
            var targetRotation = Quaternion.Lerp(transform.rotation, planarRot * verticalRot,
                1f - Mathf.Exp(-RotationSharpness * deltaTime));

            transform.rotation = targetRotation;
        }

        public void UpdateMove(Vector3 velocity)
        {
            if (FollowTransform == null || velocity == Vector3.zero)
                return;

            var targetPosition = FollowTransform.position;
            var currentPosition = transform.position;

            var direction = targetPosition - currentPosition;

            var newPosition = currentPosition + direction.normalized * velocity.magnitude * Time.deltaTime;

            transform.position = newPosition;
        }

        public void TeleportToTarget()
        {
            transform.position = FollowTransform.position;
            transform.rotation = FollowTransform.rotation;
        }

        public void TeleportToTarget(Transform point)
        {
            transform.position = point.position;
            transform.rotation = point.rotation;
        }
    }
}