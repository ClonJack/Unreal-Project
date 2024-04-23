using UnityEngine;
using UnrealTeam.SB.Common.Ecs.Binders;
using UnrealTeam.SB.Configs.Mining;
using UnrealTeam.SB.GamePlay.Mining.Components;

namespace UnrealTeam.SB.GamePlay.Mining.Binders
{
    public class MiningLaserDrawBinder : EcsComponentBinder<MiningLaserDrawData>
    {
        [SerializeField] private LineRenderer _laserRenderer;
        [SerializeField] private MiningStationConfig _config;
        
        
        protected override void InitData(ref MiningLaserDrawData component)
        {
            component.LaserRenderer = _laserRenderer;
            component.WidthCurve = _config.RenderWidthCurve;
            component.AlphaCurve = _config.RenderAlphaCurve;
            component.ColorCurve = _config.RenderColorCurve;
            component.MinColor = _config.RenderMinColor;
            component.MaxColor = _config.RenderMaxColor;
        }
    }
}