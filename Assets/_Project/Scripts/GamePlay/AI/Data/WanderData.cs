using CrashKonijn.Goap.Classes.References;
using CrashKonijn.Goap.Interfaces;
using UnrealTeam.SB.GamePlay.AI.Behaviours;

namespace UnrealTeam.SB.GamePlay.AI.Data
{
    public class WanderData : IActionData
    {
        public ITarget Target { get; set; }
        public float Timer { get; set; }
        [GetComponent] 
        public AgentMoveBehaviour MoveBehaviour { get; set; }
    }
}