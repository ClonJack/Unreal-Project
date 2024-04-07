using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using UnityEngine;
using UnrealTeam.SB.Configs.AI;
using UnrealTeam.SB.GamePlay.AI.Common;

namespace UnrealTeam.SB.GamePlay.AI.Sensors
{
    public class FoodTargetSensor : LocalTargetSensorBase, IGoapInjectable
    {
        private GoapHungerConfig _hungerConfig;
        private Collider[] _foodColliders;


        public void Inject(IGoapConfigAccess configs)
        {
            _hungerConfig = configs.AnimalHungerConfig;
        }

        public override void Created()
        {
            _foodColliders = new Collider[_hungerConfig.MaxFoodsOnRadar];
        }

        public override ITarget Sense(IMonoAgent agent, IComponentReference references)
        {
            Vector3 agentPosition = agent.transform.position;
            int foundedFoodCount = Physics.OverlapSphereNonAlloc(
                agentPosition, 
                _hungerConfig.FoodSearchDistance,
                _foodColliders, 
                _hungerConfig.FoodLayer);

            if (foundedFoodCount == 0)
                return null;

            var closestFoodDistance = float.MaxValue;
            Vector3 closestFoodPosition = default;
            for (var i = 0; i < foundedFoodCount; i++)
            {
                Vector3 foodPosition = _foodColliders[i].transform.position;
                float foodDistance = (foodPosition - agentPosition).sqrMagnitude;
                if (foodDistance < closestFoodDistance)
                {
                    closestFoodDistance = foodDistance;
                    closestFoodPosition = foodPosition;
                }
            }

            return new PositionTarget(closestFoodPosition);
        }

        public override void Update()
        {
        }
    }
}