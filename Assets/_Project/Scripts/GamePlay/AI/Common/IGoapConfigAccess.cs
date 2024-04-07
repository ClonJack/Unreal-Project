using UnrealTeam.SB.Configs.AI;

namespace UnrealTeam.SB.GamePlay.AI.Common
{
    public interface IGoapConfigAccess
    {
        public GoapWanderConfig AnimalWanderConfig { get; }
        public GoapHungerConfig AnimalHungerConfig { get; }
    }
}