using CrashKonijn.Goap.Behaviours;
using UnityEngine;
using UnrealTeam.SB.GamePlay.AI.Constants;

namespace UnrealTeam.SB.GamePlay.AI.Brains
{
    public class AnimalBrain : MonoBehaviour
    {
        [SerializeField] private AgentBehaviour _agentBehaviour;
        [SerializeField] private GoapRunnerBehaviour _runnerBehaviour;


        private void Start()
        {
            _agentBehaviour.GoapSet = _runnerBehaviour.GetGoapSet(GoapSets.Animal);
            _agentBehaviour.SetGoal<WanderGoal>(false);
        }
    }
}