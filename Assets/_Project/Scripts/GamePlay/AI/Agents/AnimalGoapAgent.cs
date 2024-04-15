using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnrealTeam.SB.Common.GOAP;
using UnrealTeam.SB.Common.GOAP.Actions;
using UnrealTeam.SB.Common.GOAP.Beliefs;
using UnrealTeam.SB.Common.GOAP.Goals;
using UnrealTeam.SB.Configs.AI;
using UnrealTeam.SB.GamePlay.AI.Behaviours;
using UnrealTeam.SB.GamePlay.AI.Strategies;

namespace UnrealTeam.SB.GamePlay.AI.Agents
{
    public class AnimalGoapAgent : GoapAgentBase
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private NavMeshAgentConfig _navMeshConfig;
        [SerializeField] private GoapWanderConfig _wanderConfig;
        [SerializeField] private GoapRelaxConfig _relaxConfig;
        

        protected override void PreInit()
        {
            _navMeshAgent.speed = _navMeshConfig.MoveSpeed;
            _navMeshAgent.angularSpeed = _navMeshConfig.RotationSpeed;
            _navMeshAgent.stoppingDistance = _navMeshConfig.StoppingDistance;
        }

        protected override Dictionary<string, AgentBelief> CreateBeliefs()
            => new BeliefsCollectionBuilder(transform)
                .AddBelief("NothingBelief", () => false)
                .AddBelief("IdleBelief", () => !_navMeshAgent.hasPath)
                .AddBelief("MovingBelief", () => _navMeshAgent.hasPath)
                .Build();

        protected override HashSet<AgentAction> CreateActions()
        {
            IMoveBehaviour moveBehaviour = new AgentMoveBehaviour(_navMeshAgent, _navMeshConfig);
            
            return new HashSet<AgentAction>
            {
                new ActionBuilder("RelaxAction")
                    .WithStrategy(new IdleStrategy(_relaxConfig))
                    .WithEffect(GetBelief("NothingBelief"))
                    .WithCost(_relaxConfig.ActionCost)
                    .Build(),

                new ActionBuilder("WanderAction")
                    .WithStrategy(new WanderStrategy(moveBehaviour, transform, _wanderConfig))
                    .WithEffect(GetBelief("MovingBelief"))
                    .WithCost(_wanderConfig.ActionCost)
                    .Build(),
            };
        }

        protected override HashSet<AgentGoal> CreateGoals()
            => new()
            {
                new GoalBuilder("RelaxGoal")
                    .WithPriority(_relaxConfig.GoalPriority)
                    .WithDesiredEffect(GetBelief("NothingBelief"))
                    .Build(),

                new GoalBuilder("WanderGoal")
                    .WithPriority(_wanderConfig.GoalPriority)
                    .WithDesiredEffect(GetBelief("MovingBelief"))
                    .Build(),
            };
    }
}