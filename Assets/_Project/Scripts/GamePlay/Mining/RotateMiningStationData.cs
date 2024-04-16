using UnityEngine;

namespace UnrealTeam.SB.GamePlay.Mining
{
    public struct RotateMiningStationData
    {
        public Transform Platform;
        public Transform LaserBase;

        public float PlatformSpeed;
        public float PlatformLeftRestriction;
        public float PlatformRightRestriction;

        public float LaserSpeed;
        public float LaserLeftRestriction;
        public float LaserRightRestriction;
        public float LaserUpRestriction;
        public float LaserDownRestriction;
    }
}