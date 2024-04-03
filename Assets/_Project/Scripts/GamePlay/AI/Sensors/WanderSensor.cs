       using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using UnityEngine;

namespace UnrealTeam.SB.GamePlay.AI
{
    public class WanderSensor : LocalTargetSensorBase
    {
        public override void Created()
        {
        }

        public override void Update()
        {
        }

        public override ITarget Sense(IMonoAgent agent, IComponentReference references)
        {
            Vector2 random =  Random.insideUnitCircle * 10f;
            Vector3 position = agent.transform.position + new Vector3(random.x, 0f, random.y);
            return new PositionTarget(position);
        }
    }
}