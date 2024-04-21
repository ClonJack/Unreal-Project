using UnityEngine;

namespace UnrealTeam.SB.GamePlay.Mining.Components
{
    public struct MiningLaserRotationData
    {
        public float AccelerationValue;
        public float AccelerationDuration;
        public Transform LaserBase;
        public AnimationCurve RotationCurve;
        public float RotationSpeed;
        public bool HasRestrictions;
        public float LeftRestriction;
        public float RightRestriction;
        public float UpRestriction;
        public float DownRestriction;
    }
}