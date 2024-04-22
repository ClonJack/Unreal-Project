using UnityEngine;
using UnrealTeam.SB.Common.GOAP.Actions;
using UnrealTeam.SB.Common.Timers;
using UnrealTeam.SB.Configs.AI;

namespace UnrealTeam.SB.GamePlay.AI.Strategies
{
    public class IdleStrategy : IActionStrategy
    {
        private readonly CountdownTimer _timer;
        private readonly float _minDuration;
        private readonly float _maxDuration;

        public bool CanPerform => true;
        public bool IsCompleted { get; private set; }


        public IdleStrategy(GoapRelaxConfig relaxConfig)
        {
            _minDuration = relaxConfig.MinDuration;
            _maxDuration = relaxConfig.MaxDuration;

            _timer = new CountdownTimer(0f);
            _timer.OnStart += () => IsCompleted = false;
            _timer.OnStop += () => IsCompleted = true;
        }

        public void Start()
        {
            var duration = Random.Range(_minDuration, _maxDuration);
            _timer.Reset(duration);
            _timer.Start();
        }

        public void Tick(float deltaTime)
        {
            _timer.Tick(deltaTime);
        }

        public void End()
        {
        }
    }
}