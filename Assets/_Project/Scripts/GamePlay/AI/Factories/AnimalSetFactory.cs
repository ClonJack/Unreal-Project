using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes.Builders;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Resolver;
using UnrealTeam.SB.GamePlay.AI.Actions;
using UnrealTeam.SB.GamePlay.AI.Common;
using UnrealTeam.SB.GamePlay.AI.Goals;
using UnrealTeam.SB.GamePlay.AI.Sensors;
using UnrealTeam.SB.GamePlay.AI.Targets;
using UnrealTeam.SB.GamePlay.AI.WorldKeys;
using VContainer;

namespace UnrealTeam.SB.GamePlay.AI.Factories
{
    public class AnimalSetFactory : GoapSetFactoryBase
    {
        private IGoapConfigAccess _goapConfigs;

        
        [Inject]
        public void Inject(IGoapConfigAccess goapConfigs)
        {
            _goapConfigs = goapConfigs;
        }
        
        public override IGoapSetConfig Create()
        {
            var setBuilder = new GoapSetBuilder(GoapSetsNames.Animal);
            RegisterWander(setBuilder);
            RegisterEat(setBuilder);
            return setBuilder.Build();
        }

        private void RegisterWander(GoapSetBuilder builder)
        {
            builder
                .AddGoal<WanderGoal>()
                .AddCondition<IsWanderingKey>(Comparison.GreaterThanOrEqual, 1);

            builder
                .AddAction<WanderAction>()
                .SetTarget<WanderTarget>()
                .AddEffect<IsWanderingKey>(EffectType.Increase)
                .SetBaseCost(_goapConfigs.AnimalWanderConfig.BaseCost)
                .SetInRange(_goapConfigs.AnimalWanderConfig.Radius);

            builder
                .AddTargetSensor<WanderTargetSensor>()
                .SetTarget<WanderTarget>();
        }

        private void RegisterEat(GoapSetBuilder builder)
        {
            builder
                .AddGoal<EatGoal>()
                .AddCondition<HungerKey>(Comparison.SmallerThanOrEqual, 0);

            builder
                .AddAction<EatAction>()
                .SetTarget<FoodTarget>()
                .AddEffect<HungerKey>(EffectType.Decrease)
                .SetBaseCost(_goapConfigs.AnimalHungerConfig.BaseCost)
                .SetInRange(_goapConfigs.AnimalHungerConfig.FoodEatingDistance);

            builder
                .AddTargetSensor<FoodTargetSensor>()
                .SetTarget<FoodTarget>();

            builder
                .AddWorldSensor<HungerWorldSensor>()
                .SetKey<HungerKey>();
        }
    }
}