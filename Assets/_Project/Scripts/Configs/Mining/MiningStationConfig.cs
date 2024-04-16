using UnityEngine;

namespace UnrealTeam.SB.Configs.Mining
{
    [CreateAssetMenu(menuName = "Configs/Mining Station", fileName = "MiningStationConfig")]
    public class MiningStationConfig : ScriptableObject
    {
        [field: Header("Platform Rotation"), Space(3)]
        [field: SerializeField] public float PlatformSpeed { get; private set; }
        [field: SerializeField] public float PlatformLeftRestriction { get; private set; }
        [field: SerializeField] public float PlatformRightRestriction { get; private set; }

        [field: Header("Laser Rotation"), Space(3)]
        [field: SerializeField] public float LaserSpeed { get; private set; }
        [field: SerializeField] public float LaserLeftRestriction { get; private set; }
        [field: SerializeField] public float LaserRightRestriction { get; private set; }
        [field: SerializeField] public float LaserUpRestriction { get; private set; }
        [field: SerializeField] public float LaserDownRestriction { get; private set; }
    }
}