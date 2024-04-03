using CrashKonijn.Goap.Interfaces;

namespace UnrealTeam.SB.GamePlay.AI
{
    public class WanderData : IActionData
    {
        public ITarget Target { get; set; }
        public float Timer { get; set; }
    }
}