using System.Collections.Generic;
using System.Linq;
using TriInspector;
using UnityEngine;
using UnrealTeam.SB.Common.GOAP.Actions;
using UnrealTeam.SB.Common.GOAP.Beliefs;
using UnrealTeam.SB.Common.GOAP.Goals;
using UnrealTeam.SB.Common.GOAP.Plan;

namespace UnrealTeam.SB.Common.GOAP
{
    public abstract class GoapAgentBase : MonoBehaviour
    {
#if UNITY_EDITOR
        [ShowInInspector, ReadOnly] private string _currentActionName;
        [ShowInInspector, ReadOnly] private string _currentGoalName;
#endif

        private Dictionary<string, AgentBelief> _beliefs;
        private HashSet<AgentAction> _actions;
        private HashSet<AgentGoal> _goals;

        private GoapPlanner _planner;
        private AgentGoal _lastGoal;
        private AgentAction _currentAction;
        private AgentGoal _currentGoal;
        

        private void Start()
        {
            PreInit();
            _planner = new GoapPlanner();
            _beliefs = CreateBeliefs();
            _actions = CreateActions();
            _goals = CreateGoals();
        }

        private void Update()
        {
            if (!HasAction())
                PlanAndStartAction();

            if (HasPlanAndAction())
                PerformCurrentAction();

#if UNITY_EDITOR
            UpdateEditorFields();
#endif
        }


        protected AgentBelief GetBelief(string beliefName)
            => _beliefs[beliefName];

        protected abstract Dictionary<string, AgentBelief> CreateBeliefs();
        protected abstract HashSet<AgentAction> CreateActions();
        protected abstract HashSet<AgentGoal> CreateGoals();
        protected virtual void PreInit() { }


        private void PlanAndStartAction()
        {
            _planner.Plan(_actions, _goals, _currentGoal, _lastGoal);

            if (!HasActionsInPlan()) 
                return;
            
            _currentGoal = _planner.ActionsPlan.AgentGoal;
            _currentAction = _planner.ActionsPlan.Actions.Pop();
            
            if (_currentAction.Preconditions.All(a => a.Evaluate()))
            {
                _currentAction.Start();
                return;
            }

            _currentAction = null;
            _currentGoal = null;
        }

        private void PerformCurrentAction()
        {
            _currentAction!.Tick(Time.deltaTime);

            if (!_currentAction.IsCompleted)
                return;

            _currentAction.End();
            _currentAction = null;

            if (_planner.ActionsPlan.Actions.Count == 0)
            {
                _lastGoal = _currentGoal;
                _currentGoal = null;
            }
        }

        private bool HasActionsInPlan()
            => _planner.ActionsPlan != null && _planner.ActionsPlan.Actions.Count > 0;

        private bool HasPlanAndAction()
            => _planner.ActionsPlan != null && HasAction();

        private bool HasAction()
            => _currentAction != null;

        private void UpdateEditorFields()
        {
            _currentActionName = _currentAction?.Name ?? "None";
            _currentGoalName = _currentGoal?.Name ?? "None";
        }
    }
}