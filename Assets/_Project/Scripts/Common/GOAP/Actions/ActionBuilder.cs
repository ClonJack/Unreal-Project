using UnrealTeam.SB.Common.GOAP.Beliefs;

namespace UnrealTeam.SB.Common.GOAP.Actions
{
    public class ActionBuilder
    {
        private readonly AgentAction _action;


        public ActionBuilder(string name)
        {
            _action = new AgentAction(name, 1);
        }

        public ActionBuilder WithCost(float cost)
        {
            _action.Cost = cost;
            return this;
        }
        
        public ActionBuilder WithStrategy(IActionStrategy strategy)
        {
            _action.Strategy = strategy;
            return this;
        }
        
        public ActionBuilder WithPrecondition(AgentBelief precondition)
        {
            _action.Preconditions.Add(precondition);
            return this;
        }
        
        public ActionBuilder WithEffect(AgentBelief effect)
        {
            _action.Effects.Add(effect);
            return this;
        }

        public AgentAction Build()
            => _action;
    }
}