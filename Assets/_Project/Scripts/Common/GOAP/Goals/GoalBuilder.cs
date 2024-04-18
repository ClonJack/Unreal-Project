using UnrealTeam.SB.Common.GOAP.Beliefs;

namespace UnrealTeam.SB.Common.GOAP.Goals
{
    public class GoalBuilder
    {
        private readonly AgentGoal _goal;

        
        public GoalBuilder(string name)
        {
            _goal = new AgentGoal(name);
        }

        public GoalBuilder WithPriority(float priority)
        {
            _goal.Priority = priority;
            return this;
        }

        public GoalBuilder WithDesiredEffect(AgentBelief effect)
        {
            _goal.DesiredEffects.Add(effect);
            return this;
        }

        public AgentGoal Build() 
            => _goal;
    }
}