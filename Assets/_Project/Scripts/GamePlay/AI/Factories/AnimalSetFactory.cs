using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes.Builders;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Resolver;
using UnrealTeam.SB.GamePlay.AI.Constants;
using UnrealTeam.SB.GamePlay.AI.Targets;
using UnrealTeam.SB.GamePlay.AI.WorldKeys;

namespace UnrealTeam.SB.GamePlay.AI.Factories
{
    public class AnimalSetFactory : GoapSetFactoryBase
    {
        public override IGoapSetConfig Create()
        {
            var setBuilder = new GoapSetBuilder(GoapSets.Animal);
            RegisterWander(setBuilder);
            return setBuilder.Build();
        }

        private static void RegisterWander(GoapSetBuilder setBuilder)
        {
            setBuilder
                .AddGoal<WanderGoal>()
                .AddCondition<IsWandering>(Comparison.GreaterThanOrEqual, 1);

            setBuilder
                .AddAction<WanderAction>()
                .SetTarget<WanderTarget>()
                .AddEffect<IsWandering>(EffectType.Increase)
                .SetBaseCost(5)
                .SetInRange(10);

            setBuilder
                .AddTargetSensor<WanderSensor>()
                .SetTarget<WanderTarget>();
        }
    }
}