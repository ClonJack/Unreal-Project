using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;
using UnrealTeam.SB.Configs.AI;
using UnrealTeam.SB.GamePlay.AI.Behaviours;
using UnrealTeam.SB.GamePlay.AI.Common;
using UnrealTeam.SB.GamePlay.AI.Goals;
using VContainer;
using Random = UnityEngine.Random;

namespace UnrealTeam.SB.GamePlay.AI.Brains
{
    public class AnimalBrain : MonoBehaviour
    {
        [SerializeField] private AgentBehaviour _agentBehaviour;
        [SerializeField] private HungerBehaviour _hungerBehaviour;
        [SerializeField] private GoapRunnerBehaviour _runnerBehaviour;
        
        private IGoapConfigAccess _goapConfigs;
        private GoalPredicate[] _goalsPredicates;
        private IGoapSet _goapSet;


        [Inject]
        public void Inject(IGoapConfigAccess goapConfigs)
        {
            _goapConfigs = goapConfigs;
        }

        private void Start()
        {
            InitHungerBehaviour();
            SetStartGoal();
        }

        private void Update()
        {
            foreach (GoalPredicate goalPredicate in _goalsPredicates)
            {
                
            }
        }

        private void InitHungerBehaviour()
        {
            GoapHungerConfig hungerConfig = _goapConfigs.AnimalHungerConfig;
            _hungerBehaviour.DepletionRate = hungerConfig.HungerDepletionRate;
            _hungerBehaviour.Hunger = Random.Range(0f, hungerConfig.MaxHunger / 2);
            _hungerBehaviour.MaxHunger = hungerConfig.MaxHunger;
        }

        private void SetStartGoal()
        {
            _goapSet = _runnerBehaviour.GetGoapSet(GoapSetsNames.Animal);
            _agentBehaviour.GoapSet = _goapSet;

            _goalsPredicates = CreateGoalsPredicates();
            _agentBehaviour.SetGoal<WanderGoal>(false);
        }

        private GoalPredicate[] CreateGoalsPredicates()
        {
            return new GoalPredicate[]
            {
                new()
                {
                    Goal = _goapSet.ResolveGoal<EatGoal>(),
                    Predicate = () => _hungerBehaviour.Hunger > _goapConfigs.AnimalHungerConfig.AcceptableHunger,
                },
                new()
                {
                    Goal = _goapSet.ResolveGoal<WanderGoal>(),
                    Predicate = () => _hungerBehaviour.Hunger <= 0,
                },
            };
        }
    }
}