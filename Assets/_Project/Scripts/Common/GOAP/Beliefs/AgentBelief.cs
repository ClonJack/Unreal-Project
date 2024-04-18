using System;
using UnityEngine;

namespace UnrealTeam.SB.Common.GOAP.Beliefs
{
    public class AgentBelief
    {
        public string Name { get; }
        public Func<bool> Condition { get; set; } = () => false;
        public Func<Vector3> ObservedLocation { get; set; } = () => Vector3.zero;


        public AgentBelief(string name)
        {
            Name = name;
        }

        public bool Evaluate()
            => Condition.Invoke();

        public Vector3 GetLocation() 
            => ObservedLocation.Invoke();
    }
}