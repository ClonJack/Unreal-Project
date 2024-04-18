using System.Collections.Generic;
using UnrealTeam.SB.Common.GOAP.Actions;
using UnrealTeam.SB.Common.GOAP.Beliefs;

namespace UnrealTeam.SB.Common.GOAP.Plan
{
    public class ActionNode
    {
        public ActionNode Parent { get; }
        public List<ActionNode> Leaves { get; }
        public AgentAction Action { get; }
        public HashSet<AgentBelief> RequiredEffects { get; }
        public float Cost { get; }

        
        public ActionNode(
            ActionNode parent, 
            AgentAction action,
            IEnumerable<AgentBelief> effects, 
            float cost)
        {
            Parent = parent;
            Leaves = new List<ActionNode>();
            Action = action;
            Cost = cost;
            RequiredEffects = new HashSet<AgentBelief>(effects);
        }

        public bool IsLeafDead() 
            => Leaves.Count == 0 && Action == null;
    }
}