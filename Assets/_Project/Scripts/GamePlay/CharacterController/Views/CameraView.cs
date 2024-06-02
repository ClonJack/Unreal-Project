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
        public float FollowingSharpness = 10000f;

        [field: Header("Interaction")]
        [field: SerializeField]
        public float InteractionDistance { get; private set; }

        public float Speed;
        public float StoppingDistance = 0.1f;
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

            PlanarDirection = Vector3.forward;
        }

        private void Start()
        {
            PlanarDirection = FollowTransform.forward;
        }

        public void UpdateRotate(Vector3 rotationInput, float deltaTime)
        {
            if (FollowTransform)
            {
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
        }

        public void UpdateMove(Vector3 velocity)
        {
            if (FollowTransform)
            {
                var targetPosition = FollowTransform.position;
                var currentPosition = transform.position;

                var direction = targetPosition - currentPosition;

                var newPosition = currentPosition + direction.normalized * velocity.magnitude * Time.deltaTime;

                transform.position = newPosition;
            }
        }
    }
}