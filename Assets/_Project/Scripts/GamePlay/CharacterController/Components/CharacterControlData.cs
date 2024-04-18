using UnityEngine;

namespace UnrealTeam.SB.GamePlay.CharacterController.Components
{
    public struct CharacterControlData
    {
        public Vector2 DirectionMove;
        public Vector2 LookDirection;
        public bool IsJump;
        public Quaternion CameraRotation;
    }
}