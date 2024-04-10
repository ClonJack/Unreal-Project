namespace UnrealTeam.SB.Common.Timers
{
    public class CountdownTimer : Timer
    {
        public bool AutoRestart { get; set; }


        public CountdownTimer(float value) : base(value)
        {
        }

        public override void Tick(float deltaTime)
        {
            if (!IsRunning)
                return;
            
            if (Time > 0) 
                Time -= deltaTime;
            else
                Stop();
        }

        public override void Stop()
        {
            base.Stop();
            if (AutoRestart)
                Start();
        }

        public void Reset(float newTime)
        {
            InitialTime = newTime;
        }
    }
}