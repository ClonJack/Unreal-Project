using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;
using UnrealTeam.SB.Configs.AI;

namespace UnrealTeam.SB.GamePlay.AI.Common
{
    public class GoapConfigInjector : GoapConfigInitializerBase, IGoapInjector, IGoapConfigAccess
    {
        [field: SerializeField] public GoapWanderConfig AnimalWanderConfig { get; private set; }


        public override void InitConfig(GoapConfig config)
            => config.GoapInjector = this;

        public void Inject(IActionBase action)
        {
            if (action is IGoapInjectable injectable)
                injectable.Inject(this);
        }

        public void Inject(IGoalBase goal)
        {
            if (goal is IGoapInjectable injectable)
                injectable.Inject(this);
        }

        public void Inject(IWorldSensor worldSensor)
        {
            if (worldSensor is IGoapInjectable injectable)
                injectable.Inject(this);
        }

        public void Inject(ITargetSensor targetSensor)
        {
            if (targetSensor is IGoapInjectable injectable)
                injectable.Inject(this);
        }
    }
}