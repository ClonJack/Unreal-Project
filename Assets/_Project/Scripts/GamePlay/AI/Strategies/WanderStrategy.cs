using UnityEngine;
using UnrealTeam.SB.Common.GOAP.Actions;
using UnrealTeam.SB.Configs.AI;
using UnrealTeam.SB.GamePlay.AI.Behaviours;

namespace UnrealTeam.SB.GamePlay.AI.Strategies
{
    public class WanderStrategy : IActionStrategy
    {
        private readonly IMoveBehaviour _moveBehaviour;
        private readonly GoapWanderConfig _wanderConfig;
        private readonly Transform _transform;
        private readonly float _radiusDiff;

        public bool CanPerform => !_moveBehaviour.TargetReached;
        public bool IsCompleted => _moveBehaviour.TargetReached;

        private bool _hasValidPosition;


        public WanderStrategy(
            IMoveBehaviour moveBehaviour,
            Transform transform,
            GoapWanderConfig wanderConfig)
        {
            _transform = transform;
            _moveBehaviour = moveBehaviour;
            _wanderConfig = wanderConfig;
            _radiusDiff = wanderConfig.MaxRadius - wanderConfig.MinRadius;
        }

        public void Start()
        {
            _hasValidPosition = TryMoveToRandomPoint();
            if (_hasValidPosition)
                _moveBehaviour.SetParams(_wanderConfig);
        }

        public void Tick(float deltaTime)
        {
        }

        public void End()
        {
            if (!_hasValidPosition)
                return;
            
            _moveBehaviour.StopMoving();
            if (!_wanderConfig.OverrideMoveParams)
                return;
            
            _moveBehaviour.ResetParams();
        }

        private bool TryMoveToRandomPoint()
        {
            for (var i = 0; i < _wanderConfig.PathfindTries; i++)
            {
                Vector3 randomPosition = FindRandomWanderPoint();
                if (_moveBehaviour.IsValidPosition(randomPosition, _wanderConfig.MaxRadius, out var positionToMove))
                {
                    _moveBehaviour.MoveTo(positionToMove);
                    return true;
                }
            }

            return false;
        }

        private Vector3 FindRandomWanderPoint()
        {
            Vector2 randomUnitVector = Random.insideUnitCircle;
            float randomX = FindWanderDistance(randomUnitVector.x);
            float randomZ = FindWanderDistance(randomUnitVector.y);
            return _transform.position + new Vector3(randomX, 0f, randomZ);
        }

        private float FindWanderDistance(float multiplier)
            => multiplier >= 0
                ? multiplier * _radiusDiff + _wanderConfig.MinRadius
                : multiplier * _radiusDiff - _wanderConfig.MinRadius;
    }
}