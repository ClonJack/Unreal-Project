using UnityEngine;
using UnrealTeam.SB.Common.Ecs.Binders;
using UnrealTeam.SB.Configs.Mining;
using UnrealTeam.SB.GamePlay.Mining.Components;

namespace UnrealTeam.SB.GamePlay.Mining.Binders
{
    public class MiningLaserRotationBinder : EcsComponentBinder<MiningLaserRotationData>
    {
        [SerializeField] private Transform _laserBase;
        [SerializeField] private MiningStationConfig _config;


        protected override void InitData(ref MiningLaserRotationData component)
        {
            component.LaserBase = _laserBase;
            component.RotationSpeed = _config.LaserRotationSpeed;
            component.RotationCurve = _config.LaserCurve;
            component.AccelerationDuration = _config.LaserAccelerationDuration;

            component.HasRestrictions = _config.HasLaserRestrictions;
            if (_config.HasLaserRestrictions)
            {
                component.LeftRestriction = _config.LaserLeftRestriction;
                component.RightRestriction = _config.LaserRightRestriction;
                component.UpRestriction = _config.LaserUpRestriction;
                component.DownRestriction = _config.LaserDownRestriction;
            }
        }
    }
}
