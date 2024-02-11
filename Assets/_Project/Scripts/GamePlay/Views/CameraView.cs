using Leopotam.EcsLite;
using UnityEngine;
using UnrealTeam.SB.Components;

namespace UnrealTeam.SB.Views
{
    public class CameraView : MonoBehaviour
    {
        [SerializeField] private Transform _followTransform;
        [SerializeField] private Transform _targetRotate;

        [Range(-90f, 85)] [SerializeField] private float _defaultVerticalAngle = 20f;
        [Range(-90f, 85)] [SerializeField] private float _minVerticalAngle = -85;
        [Range(-90f, 85)] [SerializeField] private float _maxVerticalAngle = 85;

        [SerializeField] private float _rotationSpeed = 1f;
        [SerializeField] private float _rotationSharpness = 100;

        private void OnValidate()
        {
            _defaultVerticalAngle = Mathf.Clamp(_defaultVerticalAngle, _minVerticalAngle, _maxVerticalAngle);
        }

        public void ConvertToEntity(EcsWorld ecsWorld, int entity)
        {
            ref var characterData = ref ecsWorld.GetPool<CharacterData>().Get(entity);
            characterData.TargetRotation = _followTransform.forward;
            characterData.TargetVerticalAngle = _defaultVerticalAngle;
        }
    }
}