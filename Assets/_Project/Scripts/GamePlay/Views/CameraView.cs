using UnityEngine;

namespace UnrealTeam.SB.Views
{
    public class CameraView : MonoBehaviour
    {
        [Range(-90f, 85)] [SerializeField] private float _defaultVerticalAngle = 20f;
        [Range(-90f, 85)] [SerializeField] private float _minVerticalAngle = -85;
        [Range(-90f, 85)] [SerializeField] private float _maxVerticalAngle = 85;

        [SerializeField] private float _rotationSpeed = 1f;
        [SerializeField] private float _rotationSharpness = 100;

        private bool _distanceIsObstructed;
        private float _currentDistance;
        private float _targetVerticalAngle;

        private Transform _followTransform;
        private Vector3 _planarDirection;

        private void OnValidate()
        {
            _defaultVerticalAngle = Mathf.Clamp(_defaultVerticalAngle, _minVerticalAngle, _maxVerticalAngle);
        }
        
        public void SetFollowTransform(Transform t)
        {
            _followTransform = t;
            _planarDirection = _followTransform.forward;
        }

        public void UpdateWithInput(float deltaTime, Vector3 rotationInput)
        {
            if (!_followTransform) return;
            
            var rotationFromInput = Quaternion.Euler(_followTransform.up * (rotationInput.x * _rotationSpeed));

            _planarDirection = Vector3.ProjectOnPlane(rotationFromInput * _planarDirection, _followTransform.up);

            _targetVerticalAngle -= (rotationInput.y * _rotationSpeed);
            _targetVerticalAngle = Mathf.Clamp(_targetVerticalAngle, _minVerticalAngle, _maxVerticalAngle);

            var verticalRot = Quaternion.Euler(_targetVerticalAngle, 0, 0);
            var planarRot = Quaternion.LookRotation(_planarDirection, _followTransform.up);
            var targetRotation = Quaternion.Slerp(transform.rotation, planarRot * verticalRot,
                1f - Mathf.Exp(-_rotationSharpness * deltaTime));

            transform.rotation = targetRotation;
        }
    }
}