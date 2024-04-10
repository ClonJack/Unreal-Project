using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.AI;
using UnrealTeam.SB.Common.GOAP.Actions;

namespace UnrealTeam.SB.GamePlay.AI.Strategies
{
    public class WanderStrategy : IActionStrategy
    {
        private readonly EcsWorld _ecsWorld;
        private readonly NavMeshAgent _navMeshAgent;
        private readonly float _minRadius;
        private readonly float _radiusDiff;
        private readonly int _pathfindTries;

        public bool CanPerform => !IsCompleted;
        public bool IsCompleted => _navMeshAgent.remainingDistance < 0.5f && !_navMeshAgent.pathPending;


        public WanderStrategy(EcsWorld ecsWorld, NavMeshAgent navMeshAgent, float minRadius,
            float maxRadius, int pathfindTries)
        {
            _ecsWorld = ecsWorld;
            _navMeshAgent = navMeshAgent;
            _minRadius = minRadius;
            _radiusDiff = maxRadius - minRadius;
            _pathfindTries = pathfindTries;
        }

        public void Start()
        {
            for (var i = 0; i < _pathfindTries; i++)
            {
                Vector3 randomPoint = FindRandomWanderPoint();
                if (!NavMesh.SamplePosition(randomPoint, out var navMeshHit, _radiusDiff, 1))
                    continue;

                _navMeshAgent.SetDestination(navMeshHit.position);
                break;
            }
        }

        public void Tick(float deltaTime)
        {
        }

        public void End()
        {
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