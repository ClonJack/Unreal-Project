using UnityEngine;
using UnrealTeam.SB.Common.Ecs.Binders;
using UnrealTeam.SB.Configs.Mining;
using UnrealTeam.SB.GamePlay.Mining.Components;

namespace UnrealTeam.SB.GamePlay.Mining.Binders
{
    public class RotateMiningStationBinder : EcsComponentBinder<RotateMiningStationData>
    {
        [SerializeField] private Transform _platform;
        [SerializeField] private Transform _laserBase;
        [SerializeField] private MiningStationConfig _config;


        protected override void InitData(ref RotateMiningStationData component)
        {
            component.Platform = _platform;
            component.PlatformSpeed = _config.PlatformSpeed;
            component.PlatformCurve = _config.PlatformCurve;
            
            component.LaserBase = _laserBase;
            component.LaserSpeed = _config.LaserRotationSpeed;
            component.LaserCurve = _config.LaserCurve;
            component.LaserAccelerationDuration = _config.LaserAccelerationDuration;

            component.HasPlatformRestrictions = _config.HasPlatformRestrictions;
            if (_config.HasPlatformRestrictions)
            {
                component.PlatformLeftRestriction = _config.PlatformLeftRestriction;
                component.PlatformRightRestriction = _config.PlatformRightRestriction;
            }

            component.HasLaserRestrictions = _config.HasLaserRestrictions;
            if (_config.HasLaserRestrictions)
            {
                component.LaserLeftRestriction = _config.LaserLeftRestriction;
                component.LaserRightRestriction = _config.LaserRightRestriction;
                component.LaserUpRestriction = _config.LaserUpRestriction;
                component.LaserDownRestriction = _config.LaserDownRestriction;
            }
        }
    }
}