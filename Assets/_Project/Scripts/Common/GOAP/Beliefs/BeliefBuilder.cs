using System;
using UnityEngine;

namespace UnrealTeam.SB.Common.GOAP.Beliefs
{
    public class BeliefBuilder
    {
        private readonly AgentBelief _agentBelief;


        public BeliefBuilder(string name)
        {
            _agentBelief = new AgentBelief(name);
        }

        public BeliefBuilder WithCondition(Func<bool> condition)
        {
            _agentBelief.Condition = condition;
            return this;
        }

        public BeliefBuilder WithLocation(Func<Vector3> observedLocation)
        {
            _agentBelief.ObservedLocation = observedLocation;
            return this;
        }

        public AgentBelief Build() 
            => _agentBelief;
    }
}