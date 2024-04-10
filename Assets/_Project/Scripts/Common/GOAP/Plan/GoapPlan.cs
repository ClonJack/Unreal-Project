using System.Collections.Generic;
using UnrealTeam.SB.Common.GOAP.Actions;
using UnrealTeam.SB.Common.GOAP.Goals;

namespace UnrealTeam.SB.Common.GOAP.Plan
{
    public class GoapPlan
    {
        public AgentGoal AgentGoal { get; }
        public Stack<AgentAction> Actions { get; }
        public float TotalCost { get; set; }


        public GoapPlan(Stack<AgentAction> actions, AgentGoal agentGoal, float totalCost)
        {
            Actions = actions;
            AgentGoal = agentGoal;
            TotalCost = totalCost;
        }
    }
}