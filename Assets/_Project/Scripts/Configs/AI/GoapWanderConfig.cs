using TriInspector;
using UnityEngine;

namespace UnrealTeam.SB.Configs.AI
{
    [CreateAssetMenu(menuName = "Configs/AI/Wander")]
    public class GoapWanderConfig : GoapConfigBase
    {
        [field: SerializeField, Min(0)] 
        public float MinRadius { get; private set; } = 3.0f;
        
        [field: SerializeField, Min(0)] 
        public float MaxRadius { get; private set; } = 8.0f;        
        
        [field: SerializeField, Min(0)] 
        public int PathfindTries { get; private set; } = 5;
        
        
        [field: SerializeField] 
        public bool OverrideMoveParams { get; private set; }

        [field: SerializeField, ShowIf(nameof(OverrideMoveParams), true)]
        public float MoveSpeed { get; private set; } = 3.5f;

        [field: SerializeField, ShowIf(nameof(OverrideMoveParams), true)]
        public float RotationSpeed { get; private set; } = 120;
    }
}