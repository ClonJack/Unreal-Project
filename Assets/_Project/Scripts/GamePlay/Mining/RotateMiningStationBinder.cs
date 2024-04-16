using UnityEngine;
using UnrealTeam.SB.Common.Ecs.Binders;
using UnrealTeam.SB.Configs.Mining;

namespace UnrealTeam.SB.GamePlay.Mining
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
            component.PlatformLeftRestriction = _config.PlatformLeftRestriction;
            component.PlatformRightRestriction = _config.PlatformRightRestriction;
            
            component.LaserBase = _laserBase;
            component.LaserSpeed = _config.LaserSpeed;
            component.LaserLeftRestriction = _config.LaserLeftRestriction;
            component.LaserRightRestriction = _config.LaserRightRestriction;
            component.LaserUpRestriction = _config.LaserUpRestriction;
            component.LaserDownRestriction = _config.LaserDownRestriction;
        }
    }
}