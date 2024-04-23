using UnityEngine;

namespace UnrealTeam.SB.Configs.Mining
{
    [CreateAssetMenu(menuName = "Configs/Durability", fileName = "DurabilityConfig")]
    public class DurabilityConfig : ScriptableObject
    {
        [field: SerializeField] public float MaxDurability { get; private set; }
    }
}