using UnityEngine;

namespace UnrealTeam.SB.GamePlay.Mining.Components
{
    public struct RotateMiningStationData
    {
        public float PlatformAcceleration;
        public Transform Platform;
        public AnimationCurve PlatformCurve;
        public float PlatformSpeed;
        public bool HasPlatformRestrictions;
        public float PlatformLeftRestriction;
        public float PlatformRightRestriction;

        public float LaserAcceleration;
        public Transform LaserBase;
        public AnimationCurve LaserCurve;
        public float LaserSpeed;
        public bool HasLaserRestrictions;
        public float LaserLeftRestriction;
        public float LaserRightRestriction;
        public float LaserUpRestriction;
        public float LaserDownRestriction;
    }
}