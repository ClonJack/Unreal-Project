using UnityEngine;
using UnityEngine.AI;
using UnrealTeam.SB.Configs.AI;

namespace UnrealTeam.SB.GamePlay.AI.Behaviours
{
    public class AgentMoveBehaviour : IMoveBehaviour
    {
        private readonly NavMeshAgent _navMeshAgent;
        private readonly IMoveConfig _defaultMoveConfig;

        public bool TargetReached => _navMeshAgent.remainingDistance < 0.5f && !_navMeshAgent.pathPending;
        
        
        public AgentMoveBehaviour(NavMeshAgent navMeshAgent, IMoveConfig defaultMoveConfig)
        {
            _defaultMoveConfig = defaultMoveConfig;
            _navMeshAgent = navMeshAgent;
        }
        
        public bool IsValidPosition(Vector3 targetPosition, float maxDistance, out Vector3 positionToMove)
        {
            if (NavMesh.SamplePosition(targetPosition, out var navMeshHit, maxDistance, 1))
            {
                positionToMove = navMeshHit.position;
                return true;
            }

            positionToMove = default;
            return false;
        }

        public void MoveTo(Vector3 positionToMove) 
            => _navMeshAgent.SetDestination(positionToMove);

        public void StopMoving() 
            => _navMeshAgent.ResetPath();

        public void SetParams(IMoveConfig moveConfig)
        {
            _navMeshAgent.speed = moveConfig.MoveSpeed;
            _navMeshAgent.angularSpeed = moveConfig.RotationSpeed;
        }

        public void ResetParams()
        {
            SetParams(_defaultMoveConfig);
        }
    }
}