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
    public class WanderAction : ActionBase<WanderData>, IGoapInjectable
    {
        private GoapWanderConfig _wanderConfig;


        public void Inject(IGoapConfigAccess configs)
        {
            _wanderConfig = configs.AnimalWanderConfig;
        }

        public override void Start(IMonoAgent agent, WanderData data)
        { 
            data.Timer = Random.Range(_wanderConfig.MinTime, _wanderConfig.MaxTime);
            if (_wanderConfig.OverrideMoveParams)
            {
                data.MoveBehaviour.MoveSpeed = _wanderConfig.MoveSpeed;
                data.MoveBehaviour.RotationSpeed = _wanderConfig.RotationSpeed;
            }
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
            if (_wanderConfig.OverrideMoveParams) 
                data.MoveBehaviour.ResetParams();
        }

        public override void Created()
        {
        }
    }
}