using TriInspector;
using UnityEngine;

namespace UnrealTeam.SB.Configs.Mining
{
    [CreateAssetMenu(menuName = "Configs/Mining Station", fileName = "MiningStationConfig")]
    public class MiningStationConfig : ScriptableObject
    {
        [field: Header("Platform Rotation"), Space(3)]
        [field: SerializeField] public AnimationCurve PlatformCurve { get; private set; }
        [field: SerializeField] public float PlatformSpeed { get; private set; }

        
        [field: Header("Laser Rotation"), Space(3)]
        [field: SerializeField] public AnimationCurve LaserCurve { get; private set; }
        [field: SerializeField] public float LaserRotationSpeed { get; private set; } = 5;
        [field: SerializeField] public float LaserAccelerationDuration { get; private set; } = 1;
        [field: SerializeField] public bool HasLaserRestrictions { get; private set; }
        [field: SerializeField, ShowIf(nameof(HasLaserRestrictions))] public float LaserLeftRestriction { get; private set; }
        [field: SerializeField, ShowIf(nameof(HasLaserRestrictions))] public float LaserRightRestriction { get; private set; }
        [field: SerializeField, ShowIf(nameof(HasLaserRestrictions))] public float LaserUpRestriction { get; private set; }
        [field: SerializeField, ShowIf(nameof(HasLaserRestrictions))] public float LaserDownRestriction { get; private set; }
    }
}