using System;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;

namespace UnrealTeam.SB.GamePlay.AI
{
    public class AgentMoveBehaviour : MonoBehaviour
    {
        [SerializeField] private AgentBehaviour _agentBehaviour;
        [SerializeField] private float _moveSpeed = 5;
        [SerializeField] private float _rotateSpeed = 5;
        [SerializeField] private float _rotationSqrStopDistance = 0.1f;
        
        private ITarget _currentTarget;
        private bool _shouldMove;


        private void OnEnable()
        {
            _agentBehaviour.Events.OnTargetInRange += OnTargetInRange;
            _agentBehaviour.Events.OnTargetChanged += OnTargetChanged;
            _agentBehaviour.Events.OnTargetOutOfRange += OnTargetOutOfRange;
        }

        private void OnDisable()
        {
            _agentBehaviour.Events.OnTargetInRange -= OnTargetInRange;
            _agentBehaviour.Events.OnTargetChanged -= OnTargetChanged;
            _agentBehaviour.Events.OnTargetOutOfRange -= OnTargetOutOfRange;
        }

        private void Update()
        {
            if (!HasTarget())
                return;
            
            Vector3 targetPosition = GetTargetPosition();
            MoveToTarget(targetPosition);
            RotateToTarget(targetPosition);
        }

        private void OnTargetInRange(ITarget target) 
            => _shouldMove = true;

        private void OnTargetOutOfRange(ITarget target) 
            => _shouldMove = false;

        private void OnTargetChanged(ITarget target, bool inRange)
        {
            _currentTarget = target;
            _shouldMove = inRange;
        }

        private bool HasTarget()
        {
            if (!_shouldMove)
                return false;
        
            if (_currentTarget == null)
                return false;

            return true;
        }

        private void MoveToTarget(Vector3 targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, _moveSpeed * Time.deltaTime);
        }

        private void RotateToTarget(Vector3 targetPosition)
        {
            Vector3 direction = targetPosition - transform.position;
            if (direction.sqrMagnitude < _rotationSqrStopDistance)
                return;
            
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotateSpeed * Time.deltaTime); 
        }

        private Vector3 GetTargetPosition() 
            => new(_currentTarget.Position.x, transform.position.y, _currentTarget.Position.z);
    }
}