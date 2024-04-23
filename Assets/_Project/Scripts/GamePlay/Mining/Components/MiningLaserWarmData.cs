using UnityEngine;

namespace UnrealTeam.SB.GamePlay.Mining.Components
{
    public struct MiningLaserWarmData
    {
        public float WarmTime;
        public bool IsCooledEventSent;

        public Transform LaserSpawnPoint;
        public AnimationCurve WarmDurationCurve;
        public float WarmDuration;
        public AnimationCurve WarmDistanceMultiplierCurve;
        public float WarmMaxDistance;
        public float WarmPower;
        public LayerMask WarmTarget;
    }
}