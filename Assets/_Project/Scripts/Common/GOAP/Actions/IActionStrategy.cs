namespace UnrealTeam.SB.Common.GOAP.Actions
{
    public interface IActionStrategy
    {
        public bool CanPerform { get; }
        public bool IsCompleted { get; }

        public void Start();
        public void Tick(float deltaTime);
        public void End();
    }
}