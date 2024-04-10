using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.AI;
using UnrealTeam.SB.Common.GOAP;
using UnrealTeam.SB.Common.GOAP.Actions;
using UnrealTeam.SB.Common.GOAP.Beliefs;
using UnrealTeam.SB.Common.GOAP.Goals;
using UnrealTeam.SB.GamePlay.AI.Strategies;
using VContainer;

namespace UnrealTeam.SB.GamePlay.AI.Agents
{
    public class AnimalGoapAgent : GoapAgentBase
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        
        private EcsWorld _ecsWorld;

        
        [Inject]
        public void Construct(EcsWorld ecsWorld)
        {
            _ecsWorld = ecsWorld;
        }
        
        protected override Dictionary<string, AgentBelief> CreateBeliefs()
            => new BeliefsCollectionBuilder(transform)
                .AddBelief("NothingBelief", () => false)
                .AddBelief("IdleBelief", () => !_navMeshAgent.hasPath)
                .AddBelief("MovingBelief", () => _navMeshAgent.hasPath)
                .Build();

        protected override HashSet<AgentAction> CreateActions()
            => new()
            {
                new ActionBuilder("RelaxAction")
                    .WithStrategy(new IdleStrategy(1f, 3f))
                    .WithEffect(GetBelief("NothingBelief"))
                    .WithCost(3)
                    .Build(),

                new ActionBuilder("WanderAction")
                    .WithStrategy(new WanderStrategy(_ecsWorld, _navMeshAgent, 3f, 8f, 5))
                    .WithEffect(GetBelief("MovingBelief"))
                    .WithCost(3)
                    .Build(),
            };

        protected override HashSet<AgentGoal> CreateGoals()
            => new()
            {
                new GoalBuilder("RelaxGoal")
                    .WithPriority(1)
                    .WithDesiredEffect(GetBelief("NothingBelief"))
                    .Build(),

                new GoalBuilder("WanderGoal")
                    .WithPriority(1)
                    .WithDesiredEffect(GetBelief("MovingBelief"))
                    .Build(),
            };
    }
}