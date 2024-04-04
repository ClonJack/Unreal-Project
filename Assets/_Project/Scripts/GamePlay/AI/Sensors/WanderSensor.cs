using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using UnityEngine;
using UnrealTeam.SB.Configs.AI;
using UnrealTeam.SB.GamePlay.AI.Common;

namespace UnrealTeam.SB.GamePlay.AI.Sensors
{
    public class WanderSensor : LocalTargetSensorBase, IGoapInjectable
    {
        private GoapWanderConfig _wanderConfig;

        
        public void Inject(IGoapConfigAccess configs)
        {
            _wanderConfig = configs.AnimalWanderConfig;
        }

        public override ITarget Sense(IMonoAgent agent, IComponentReference references)
        {
            Vector2 random =  Random.insideUnitCircle * _wanderConfig.Radius;
            Vector3 position = agent.transform.position + new Vector3(random.x, 0f, random.y);
            return new PositionTarget(position);
        }

        public override void Created()
        {
        }

        public override void Update()
        {
        }
    }
}