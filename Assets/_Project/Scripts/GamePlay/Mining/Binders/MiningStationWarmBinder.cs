using UnityEngine;
using UnrealTeam.SB.Common.Ecs.Binders;
using UnrealTeam.SB.Configs.Mining;
using UnrealTeam.SB.GamePlay.Mining.Components;

namespace UnrealTeam.SB.GamePlay.Mining.Binders
{
    public class MiningStationWarmBinder : EcsComponentBinder<MiningStationWarmData>
    {
        [SerializeField] private Transform _laserSpawnPoint;
        [SerializeField] private MiningStationConfig _config;


        protected override void InitData(ref MiningStationWarmData component)
        {
            component.LaserSpawnPoint = _laserSpawnPoint;
            
            component.WarmDurationCurve = _config.WarmDurationCurve;
            component.WarmDuration = _config.WarmDuration;

            component.WarmDistanceMultiplierCurve = _config.WarmDistanceMultiplierCurve;
            component.WarmMaxDistance = _config.WarmMaxDistance;
            
            component.WarmPower = _config.WarmPower;
            component.WarmTarget = _config.WarmTarget;
        }
    }
}