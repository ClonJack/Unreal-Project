using CrashKonijn.Goap.Interfaces;

namespace UnrealTeam.SB.GamePlay.AI.Data
{
    public class CommonData : IActionData
    {
        public ITarget Target { get; set; }
        public float Timer { get; set; }
    }
}