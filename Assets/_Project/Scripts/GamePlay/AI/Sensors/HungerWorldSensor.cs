using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using UnityEngine;
using UnrealTeam.SB.GamePlay.AI.Behaviours;

namespace UnrealTeam.SB.GamePlay.AI.Sensors
{
    public class HungerWorldSensor : LocalWorldSensorBase
    {
        public override SenseValue Sense(IMonoAgent agent, IComponentReference references)
        {
            float hunger = references.GetCachedComponent<HungerBehaviour>().Hunger;
            return new SenseValue(Mathf.CeilToInt(hunger));
        }

        public override void Created()
        {
        }

        public override void Update()
        {
        }
    }
}