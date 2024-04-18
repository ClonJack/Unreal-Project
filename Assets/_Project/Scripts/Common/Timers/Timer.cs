using System;

namespace UnrealTeam.SB.Common.Timers
{
    public abstract class Timer
    {
        protected float InitialTime { get; set; }
        protected float Time { get; set; }
        protected bool IsRunning { get; private set; }
        
        public float Progress => Time / InitialTime;
        public Action OnStart { get; set; }
        public Action OnStop { get; set; }


        protected Timer(float value)
        {
            InitialTime = value;
        }

        public virtual void Start()
        {
            Time = InitialTime;
            if (IsRunning) 
                return;
            
            IsRunning = true;
            OnStart?.Invoke();
        }

        public virtual void Stop()
        {
            if (!IsRunning) 
                return;
            
            IsRunning = false;
            OnStop?.Invoke();
        }

        public abstract void Tick(float deltaTime);
    }
}