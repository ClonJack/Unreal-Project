using UnityEngine;

namespace UnrealTeam.SB.GamePlay.Mining.Components
{
    public struct MiningStationWarmData
    {
        public Transform LaserSpawnPoint;
        public float WarmTime;
        
        public AnimationCurve WarmDurationCurve;
        public float WarmDuration;
        public AnimationCurve WarmDistanceMultiplierCurve;
        public float WarmMaxDistance;
        public float WarmPower;
        public LayerMask WarmTarget;
    }
}