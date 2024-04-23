using UnityEngine;

namespace UnrealTeam.SB.GamePlay.Mining.Components
{
    public struct MiningLaserDrawData
    {
        public LineRenderer LaserRenderer;
        public AnimationCurve WidthCurve;
        public AnimationCurve AlphaCurve;
        public AnimationCurve ColorCurve;
        public Color MinColor;
        public Color MaxColor;
    }
}