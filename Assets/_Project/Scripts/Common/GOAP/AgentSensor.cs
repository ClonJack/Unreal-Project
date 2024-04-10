using System;
using UnityEngine;
using UnrealTeam.SB.Common.Timers;

namespace UnrealTeam.SB.Common.GOAP
{
    public class AgentSensor : MonoBehaviour
    {
        [SerializeField] private SphereCollider _detectionTrigger;

        private GameObject _target;
        private Vector3 _lastKnownPosition;
        private CountdownTimer _timer;
        private float _detectionRadius;
        private float _checkInterval;

        public event Action OnTargetChanged;


        public void Init(float detectionRadius, float checkInterval)
        {
            _checkInterval = checkInterval;
            _detectionRadius = detectionRadius;
        }

        private void Start()
        {
            _detectionTrigger.isTrigger = true;
            _detectionTrigger.radius = _detectionRadius;
            
            _timer = new CountdownTimer(_checkInterval)
            {
                OnStop = () => UpdateTargetPosition(_target),
                AutoRestart = true,
            };
            _timer.Start();
        }

        private void Update()
            => _timer.Tick(Time.deltaTime);

        private void OnTriggerEnter(Collider other)
            => UpdateTargetPosition(other.gameObject);

        private void OnTriggerExit(Collider other)
            => UpdateTargetPosition();

        private void OnDrawGizmos()
        {
            Gizmos.color = IsTargetInRange() ? Color.red : Color.green;
            Gizmos.DrawWireSphere(transform.position, _detectionRadius);
        }
        

        public Vector3 GetTargetPosition() => _target != null
            ? _target.transform.position
            : Vector3.zero;

        public bool IsTargetInRange() 
            => GetTargetPosition() != Vector3.zero;

        private void UpdateTargetPosition(GameObject target = null)
        {
            _target = target;
            if (IsTargetInRange() && (_lastKnownPosition != GetTargetPosition() || _lastKnownPosition != Vector3.zero))
            {
                _lastKnownPosition = GetTargetPosition();
                OnTargetChanged?.Invoke();
            }
        }
    }
}