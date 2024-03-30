using KinematicCharacterController;
using UnityEngine;

namespace UnrealTeam.SB.GamePlay.Views
{
    public class CameraView : MonoBehaviour
    {
        [field: SerializeField] public float InteractionDistance { get; private set; } = 5;
        [field: SerializeField] public LayerMask InteractionLayer { get; private set; }
        
        [field: SerializeField] public Transform FollowTransform { get; set; }
        [field: SerializeField, Range(-90f, 85)] public float MinVerticalAngle = -85;
        [field: SerializeField, Range(-90f, 85)] public float MaxVerticalAngle = 85;
        [field: SerializeField] public float RotationSpeed = 15f;
        [field: SerializeField] public float RotationSharpness = 1000;
        
        [ReadOnly] public Vector3 TargetRotation;
        [ReadOnly] public float TargetVerticalAngle;

        
        private void Awake()
        {
            TargetRotation = transform.forward;
        }
    }
}