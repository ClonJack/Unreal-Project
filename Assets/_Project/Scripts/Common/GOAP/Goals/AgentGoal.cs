using System.Collections.Generic;
using UnrealTeam.SB.Common.GOAP.Beliefs;

namespace UnrealTeam.SB.Common.GOAP.Goals
{
    public class AgentGoal
    {
        public string Name { get; }
        public float Priority { get; set; }
        public HashSet<AgentBelief> DesiredEffects { get; } = new();


        public AgentGoal(string name)
        {
            Name = name;
        }
    }
}