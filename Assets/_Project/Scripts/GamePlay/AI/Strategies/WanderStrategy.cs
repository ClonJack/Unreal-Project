using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.AI;
using UnrealTeam.SB.Common.GOAP.Actions;
using UnrealTeam.SB.Configs.AI;

namespace UnrealTeam.SB.GamePlay.AI.Strategies
{
    public class WanderStrategy : IActionStrategy
    {
        private readonly EcsWorld _ecsWorld;
        private readonly NavMeshAgent _navMeshAgent;
        private readonly float _minRadius;
        private readonly float _radiusDiff;
        private readonly int _pathfindTries;
        private readonly GoapWanderConfig _wanderConfig;
        private readonly NavMeshAgentConfig _navMeshConfig;

        public bool CanPerform => !IsCompleted;
        public bool IsCompleted => _navMeshAgent.remainingDistance < 0.5f && !_navMeshAgent.pathPending;


        public WanderStrategy(
            EcsWorld ecsWorld, 
            NavMeshAgent navMeshAgent,
            GoapWanderConfig wanderConfig,
            NavMeshAgentConfig navMeshConfig)
        {
            _navMeshConfig = navMeshConfig;
            _ecsWorld = ecsWorld;
            _navMeshAgent = navMeshAgent;
            _wanderConfig = wanderConfig;
            _minRadius = wanderConfig.MinRadius;
            _radiusDiff = wanderConfig.MaxRadius - wanderConfig.MinRadius;
            _pathfindTries = wanderConfig.PathfindTries;
        }

        public void Start()
        {
            bool isPointFounded = TryWalkInRandomPoint();
            TryOverrideMoveParams(isPointFounded);
        }

        public void Tick(float deltaTime)
        {
        }

        public void End()
        {
            TryResetMoveParams();
        }

        private bool TryWalkInRandomPoint()
        {
            for (var i = 0; i < _pathfindTries; i++)
            {
                Vector3 randomPoint = FindRandomWanderPoint();
                if (!NavMesh.SamplePosition(randomPoint, out var navMeshHit, _radiusDiff, 1))
                    continue;

                _navMeshAgent.SetDestination(navMeshHit.position);
                return true;
            }

            return false;
        }

        private void TryOverrideMoveParams(bool isPointFounded)
        {
            if (isPointFounded && _wanderConfig.OverrideMoveParams)
            {
                _navMeshAgent.speed = _wanderConfig.MoveSpeed;
                _navMeshAgent.angularSpeed = _wanderConfig.RotationSpeed;
            }
        }
        
        private void TryResetMoveParams()
        {
            if (_wanderConfig.OverrideMoveParams)
            {
                _navMeshAgent.speed = _navMeshConfig.MoveSpeed;
                _navMeshAgent.angularSpeed = _navMeshConfig.RotationSpeed;
            }
        }

        private Vector3 FindRandomWanderPoint()
        {
            Vector2 randomUnitVector = Random.insideUnitCircle;
            float randomX = FindWanderDistance(randomUnitVector.x);
            float randomZ = FindWanderDistance(randomUnitVector.y);
            return _navMeshAgent.transform.position + new Vector3(randomX, 0f, randomZ);
        }

        private float FindWanderDistance(float multiplier)
            => multiplier >= 0
                ? multiplier * _radiusDiff + _minRadius
                : multiplier * _radiusDiff - _minRadius;
    }
}