using UnityEngine;

namespace UnrealTeam.SB.Configs.AI
{
    [CreateAssetMenu(menuName = "Configs/AI/Relax")]
    public class GoapRelaxConfig : GoapConfigBase
    {
        [field: SerializeField, Min(0), Tooltip("In Seconds")]
        public float MinDuration { get; private set; } = 1f;

        [field: SerializeField, Min(0), Tooltip("In Seconds")]
        public float MaxDuration { get; private set; } = 3f;
    }
}