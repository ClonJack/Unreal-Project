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

        [Header("Interactable")] public float InteractionDistance;
        public LayerMask InteractionLayer;

        public Vector3 PlanarDirection { get; set; }

        private bool _distanceIsObstructed;
        private float _currentDistance;
        private float _targetVerticalAngle;
        private Vector3 _currentFollowPosition;

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

            _currentFollowPosition = FollowTransform.position;
        }
        
        public void SetFollowTransform(Transform transform)
        {
            FollowTransform = transform;

            PlanarDirection = FollowTransform.forward;

            _currentFollowPosition = FollowTransform.position;
        }

        public void UpdateRotateAndPosition(float deltaTime, Vector3 rotationInput)
        {
            if (FollowTransform)
            {
                var rotationFromInput = Quaternion.Euler(FollowTransform.up * (rotationInput.x * RotationSpeed));
                PlanarDirection = rotationFromInput * PlanarDirection;
                PlanarDirection = Vector3.Cross(FollowTransform.up, Vector3.Cross(PlanarDirection, FollowTransform.up));
                var planarRot = Quaternion.LookRotation(PlanarDirection, FollowTransform.up);

                _targetVerticalAngle -= (rotationInput.y * RotationSpeed);
                _targetVerticalAngle = Mathf.Clamp(_targetVerticalAngle, MinVerticalAngle, MaxVerticalAngle);

                var verticalRot = Quaternion.Euler(_targetVerticalAngle, 0, 0);
                var targetRotation = Quaternion.Lerp(transform.rotation, planarRot * verticalRot,
                    1f - Mathf.Exp(-RotationSharpness * deltaTime));

                transform.rotation = targetRotation;

                _currentFollowPosition = Vector3.Lerp(_currentFollowPosition, FollowTransform.position,
                    1f - Mathf.Exp(-FollowingSharpness * deltaTime));

                transform.position = _currentFollowPosition;
            }
        }
    }
}