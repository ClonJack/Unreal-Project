using UnityEngine;

namespace UnrealTeam.SB.GamePlay.Mining.Components
{
    public struct MiningPlatformRotationData
    {
        public float AccelerationValue;
        public float AccelerationDuration;
        public Transform Platform;
        public AnimationCurve RotationCurve;
        public float RotationSpeed;
    }
}