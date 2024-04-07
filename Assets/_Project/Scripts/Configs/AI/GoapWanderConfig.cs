using TriInspector;
using UnityEngine;

namespace UnrealTeam.SB.Configs.AI
{
    [CreateAssetMenu(menuName = "Configs/AI/Wander")]
    public class GoapWanderConfig : ScriptableObject
    {
        [field: SerializeField, Min(0), Tooltip("In Seconds")]
        public float MinTime { get; private set; } = 1.5f;

        [field: SerializeField, Min(0), Tooltip("In Seconds")]
        public float MaxTime { get; private set; } = 4.5f;

        [field: SerializeField, Min(0)] 
        public float Radius { get; private set; } = 7.0f;

        [field: SerializeField, Min(0)] 
        public int BaseCost { get; private set; } = 10;

        
        
        [field: SerializeField] 
        public bool OverrideMoveParams { get; private set; }

        [field: SerializeField, ShowIf(nameof(OverrideMoveParams), true)]
        public float MoveSpeed { get; private set; }

        [field: SerializeField, ShowIf(nameof(OverrideMoveParams), true)]
        public float RotationSpeed { get; private set; }
    }
}