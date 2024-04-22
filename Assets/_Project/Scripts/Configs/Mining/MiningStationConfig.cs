using TriInspector;
using UnityEngine;

namespace UnrealTeam.SB.Configs.Mining
{
    [CreateAssetMenu(menuName = "Configs/Mining Station", fileName = "MiningStationConfig")]
    public class MiningStationConfig : ScriptableObject
    {
        [field: Title("Platform Rotation")]
        [field: SerializeField] public AnimationCurve PlatformCurve { get; private set; }
        [field: SerializeField] public float PlatformSpeed { get; private set; }

        
        [field: Title("Laser Rotation")]
        [field: SerializeField] public AnimationCurve LaserAccelerationCurve { get; private set; }
        [field: SerializeField] public float LaserRotationSpeed { get; private set; } = 5;
        [field: SerializeField] public float LaserAccelerationDuration { get; private set; } = 1;
        [field: SerializeField] public bool LaserHasRestrictions { get; private set; }
        [field: SerializeField, ShowIf(nameof(LaserHasRestrictions))] public float LaserLeftRestriction { get; private set; }
        [field: SerializeField, ShowIf(nameof(LaserHasRestrictions))] public float LaserRightRestriction { get; private set; }
        [field: SerializeField, ShowIf(nameof(LaserHasRestrictions))] public float LaserUpRestriction { get; private set; }
        [field: SerializeField, ShowIf(nameof(LaserHasRestrictions))] public float LaserDownRestriction { get; private set; }
        
        
        [field: Title("Laser Mining")]
        
        [field: Header("Duration")]
        [field: SerializeField] public AnimationCurve WarmDurationCurve { get; private set; }
        [field: SerializeField] public float WarmDuration { get; private set; }
        
        [field: Header("Distance")]
        [field: SerializeField] public AnimationCurve WarmDistanceMultiplierCurve { get; private set; }
        [field: SerializeField] public float WarmMaxDistance { get; private set; }
        
        [field: Header("Power")]
        [field: SerializeField] public float WarmPower { get; private set; }
        [field: SerializeField] public LayerMask WarmTarget { get; private set; }
    }
}