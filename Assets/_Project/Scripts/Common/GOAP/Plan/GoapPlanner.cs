using System.Collections.Generic;
using System.Linq;
using UnrealTeam.SB.Common.GOAP.Actions;
using UnrealTeam.SB.Common.GOAP.Beliefs;
using UnrealTeam.SB.Common.GOAP.Goals;

namespace UnrealTeam.SB.Common.GOAP.Plan
{
    public class GoapPlanner
    {
        public GoapPlan ActionsPlan { get; private set; }
        
        
        public void Plan(HashSet<AgentAction> actions, HashSet<AgentGoal> goals, AgentGoal currentGoal, AgentGoal lastGoal)
        {
            if (currentGoal != null)
                goals = new HashSet<AgentGoal>(goals.Where(g => g.Priority > currentGoal.Priority));
            
            foreach (AgentGoal goal in OrderUnsolvedGoalsByPriority(goals, lastGoal))
            {
                var headNode = new ActionNode(parent: null, action: null, goal.DesiredEffects, 0);
                
                if (!TryFindPath(headNode, actions)) 
                    continue;
                
                if (headNode.IsLeafDead())
                    continue;

                var actionsStack = new Stack<AgentAction>();
                while (headNode.Leaves.Count > 0)
                {
                    ActionNode cheapestLeaf = headNode.Leaves.OrderBy(leaf => leaf.Cost).First();
                    headNode = cheapestLeaf;
                    actionsStack.Push(cheapestLeaf.Action);
                }

                ActionsPlan = new GoapPlan(actionsStack, goal, headNode.Cost);
                return;
            }
        }

        private bool TryFindPath(ActionNode parentNode, HashSet<AgentAction> actions)
        {
            foreach (AgentAction action in OrderActionsByCost(actions))
            {
                HashSet<AgentBelief> requiredEffects = parentNode.RequiredEffects;
                requiredEffects.RemoveWhere(effect => effect.Evaluate());
                
                if (requiredEffects.Count == 0)
                    return true;

                if (!action.Effects.Any(effect => requiredEffects.Contains(effect))) 
                    continue;
                
                var newRequiredEffects = new HashSet<AgentBelief>(requiredEffects);
                newRequiredEffects.ExceptWith(action.Effects);
                newRequiredEffects.UnionWith(action.Preconditions);

                var newAvailableActions = new HashSet<AgentAction>(actions);
                newAvailableActions.Remove(action);

                var newNode = new ActionNode(parentNode, action, newRequiredEffects, parentNode.Cost + action.Cost);

                if (TryFindPath(newNode, newAvailableActions))
                {
                    parentNode.Leaves.Add(newNode);
                    newRequiredEffects.ExceptWith(newNode.Action.Preconditions);
                }

                if (newRequiredEffects.Count == 0)
                    return true;
            }

            return false;
        }

        private static IEnumerable<AgentAction> OrderActionsByCost(HashSet<AgentAction> actions) 
            => actions.OrderBy(a => a.Cost);

        private static IEnumerable<AgentGoal> OrderUnsolvedGoalsByPriority(IEnumerable<AgentGoal> goals, AgentGoal lastGoal)
            => goals
                .Where(goal => goal.DesiredEffects.Any(belief => !belief.Evaluate()))
                .OrderByDescending(goal => goal == lastGoal ? goal.Priority - 0.01 : goal.Priority);
    }
}