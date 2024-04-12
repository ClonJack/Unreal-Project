using TriInspector;
using UnityEngine;

namespace UnrealTeam.SB.Configs.AI
{
    public class GoapConfigBase : ScriptableObject
    {
        [field: SerializeField, Min(0)] 
        public float ActionCost { get; private set; } = 10;

        [field: SerializeField, Min(0)]
        public float GoalPriority { get; private set; } = 1;
    }
}