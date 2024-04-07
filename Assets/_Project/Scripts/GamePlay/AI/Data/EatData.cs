using CrashKonijn.Goap.Classes.References;
using UnrealTeam.SB.GamePlay.AI.Behaviours;

namespace UnrealTeam.SB.GamePlay.AI.Data
{
    public class EatData : CommonData
    {
        [GetComponent] public HungerBehaviour HungerBehaviour { get; set; }
    }
}