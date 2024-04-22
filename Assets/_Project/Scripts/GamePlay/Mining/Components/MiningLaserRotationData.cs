using UnityEngine;
using UnrealTeam.SB.GamePlay.Common.Views;

namespace UnrealTeam.SB.GamePlay.Mining.Components
{
    public struct MiningLaserRotationData
    {
        public float AccelerationValue;
        public float AccelerationDuration;
        public SyncRotationView LaserBase;
        public AnimationCurve RotationCurve;
        public float RotationSpeed;
        public bool HasRestrictions;
        public float LeftRestriction;
        public float RightRestriction;
        public float UpRestriction;
        public float DownRestriction;
    }
}