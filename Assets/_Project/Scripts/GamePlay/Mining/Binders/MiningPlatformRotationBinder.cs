using UnityEngine;
using UnrealTeam.SB.Common.Ecs.Binders;
using UnrealTeam.SB.Configs.Mining;
using UnrealTeam.SB.GamePlay.Mining.Components;

namespace UnrealTeam.SB.GamePlay.Mining.Binders
{
    public class MiningPlatformRotationBinder : EcsComponentBinder<MiningPlatformRotationData>
    {
        [SerializeField] private Transform _platform;
        [SerializeField] private MiningStationConfig _config;


        protected override void InitData(ref MiningPlatformRotationData component)
        {
            component.Platform = _platform;
            component.RotationSpeed = _config.PlatformSpeed;
            component.RotationCurve = _config.PlatformCurve;
        }
    }
}