using CrashKonijn.Goap.Classes.References;
using UnrealTeam.SB.GamePlay.AI.Behaviours;

namespace UnrealTeam.SB.GamePlay.AI.Data
{
    public class WanderData : CommonData
    {
        [GetComponent] public AgentMoveBehaviour MoveBehaviour { get; set; }
    }
}