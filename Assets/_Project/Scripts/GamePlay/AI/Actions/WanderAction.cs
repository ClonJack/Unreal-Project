
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;

namespace UnrealTeam.SB.GamePlay.AI
{
    public class WanderAction : ActionBase<WanderData>
    {
        public override void Created()
        {
        }

        public override void Start(IMonoAgent agent, WanderData data)
        {
            data.Timer = Random.Range(1.0f, 3.0f);
        }

        public override ActionRunState Perform(IMonoAgent agent, WanderData data, ActionContext context)
        {
            data.Timer -= context.DeltaTime;
            if (data.Timer > 0)
                return ActionRunState.Continue;
            
            return ActionRunState.Stop;
        }

        public override void End(IMonoAgent agent, WanderData data)
        {
        }
    }
}