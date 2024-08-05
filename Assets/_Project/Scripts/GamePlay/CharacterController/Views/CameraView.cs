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
        
        private float _targetVerticalAngle;
        private  float _minDistanceSqr = 0.0625f;
        private  float _minVelocity = 0.01f;

        
        private void OnValidate()
        {
            DefaultVerticalAngle = Mathf.Clamp(DefaultVerticalAngle, MinVerticalAngle, MaxVerticalAngle);
        }

        private void Awake()
        {
            _targetVerticalAngle = 0f;

            transform.position = FollowTransform.position + FollowTransform.forward * StartOffset;
        }

        private void Start()
        {
            TeleportToTarget();
        }

        public void UpdateRotate(Vector3 rotationInput, float deltaTime)
        {
            if (FollowTransform == null) return;

            if (PlanarDirection == Vector3.zero)
                PlanarDirection = FollowTransform.forward;

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
            var currentPosition = transform.position;
            var targetPosition = FollowTransform.position;

            var direction = (targetPosition - currentPosition).normalized;

            var distanceSqr = (targetPosition - currentPosition).sqrMagnitude;

            if (distanceSqr > _minDistanceSqr && velocity.sqrMagnitude == 0)
            {
                transform.position = targetPosition;
                return;
            }

            var clampedVelocity = Mathf.Clamp(velocity.magnitude, _minVelocity, float.MaxValue);

            transform.position = currentPosition + direction * clampedVelocity * Time.deltaTime;
        }

        public void TeleportToTarget()
        {
            transform.position = FollowTransform.position;
            transform.rotation = FollowTransform.rotation;
        }
    }
}