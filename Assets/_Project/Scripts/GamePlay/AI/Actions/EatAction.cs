using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;
using UnrealTeam.SB.Configs.AI;
using UnrealTeam.SB.GamePlay.AI.Common;
using UnrealTeam.SB.GamePlay.AI.Data;

namespace UnrealTeam.SB.GamePlay.AI.Actions
{
    public class EatAction : ActionBase<EatData>, IGoapInjectable
    {
        private GoapHungerConfig _hungerConfig;


        public void Inject(IGoapConfigAccess configs)
        {
            _hungerConfig = configs.AnimalHungerConfig;
        }

        public override void Start(IMonoAgent agent, EatData data)
        {
            data.Timer = Random.Range(1f, 3f);
            data.HungerBehaviour.IsDepleting = false;
        }

        public override ActionRunState Perform(IMonoAgent agent, EatData data, ActionContext context)
        {
            data.Timer -= context.DeltaTime;
            data.HungerBehaviour.Hunger -= _hungerConfig.FoodEatingRate * context.DeltaTime;
            if (data.Target == null || data.HungerBehaviour.Hunger <= 0)
                return ActionRunState.Stop;

            return ActionRunState.Continue;
        }

        public override void End(IMonoAgent agent, EatData data)
        {
            data.HungerBehaviour.IsDepleting = true;
        }

        public override void Created()
        {
        }
    }
}