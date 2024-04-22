using System.Collections.Generic;
using UnrealTeam.SB.Common.GOAP.Beliefs;

namespace UnrealTeam.SB.Common.GOAP.Actions
{
    public class AgentAction
    {
        public string Name { get; }
        public float Cost { get; set; }
        public IActionStrategy Strategy { get; set; }
        public HashSet<AgentBelief> Preconditions { get; } = new();
        public HashSet<AgentBelief> Effects { get; } = new();
        public bool IsCompleted => Strategy.IsCompleted;


        public AgentAction(string name, float cost)
        {
            Name = name;
            Cost = cost;
        }

        public void Start() 
            => Strategy.Start();

        public void Tick(float deltaTime)
        {
            if (Strategy.CanPerform)
                Strategy.Tick(deltaTime);
            
            if (!Strategy.IsCompleted)
                return;

            foreach (var effect in Effects) 
                effect.Evaluate();
        }

        public void End()
            => Strategy.End();
    }
}