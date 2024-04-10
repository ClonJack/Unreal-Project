using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnrealTeam.SB.Common.GOAP.Beliefs
{
    public class BeliefsCollectionBuilder
    {
        private readonly Transform _agent;
        private readonly Dictionary<string, AgentBelief> _beliefs;


        public BeliefsCollectionBuilder(Transform agent)
        {
            _agent = agent;
            _beliefs = new Dictionary<string, AgentBelief>();
        }

        public BeliefsCollectionBuilder AddBelief(string key, Func<bool> condition)
        {
            _beliefs.Add(key, new BeliefBuilder(key)
                .WithCondition(condition)
                .Build());

            return this;
        }

        public BeliefsCollectionBuilder AddSensorBelief(string key, AgentSensor sensor)
        {
            _beliefs.Add(key, new BeliefBuilder(key)
                .WithCondition(() => sensor.IsTargetInRange())
                .WithLocation(() => sensor.GetTargetPosition())
                .Build());

            return this;
        }

        public BeliefsCollectionBuilder AddLocationBelief(string key, float distance, Vector3 locationCondition)
        {
            _beliefs.Add(key, new BeliefBuilder(key)
                .WithCondition(() => InRangeOf(locationCondition, distance))
                .WithLocation(() => locationCondition)
                .Build());

            return this;
        }

        public BeliefsCollectionBuilder AddLocationBelief(string key, float distance, Transform locationCondition)
            => AddLocationBelief(key, distance, locationCondition.position);

        public Dictionary<string, AgentBelief> Build()
            => _beliefs;

        private bool InRangeOf(Vector3 pos, float range) 
            => Vector3.Distance(_agent.position, pos) < range;
    }
}