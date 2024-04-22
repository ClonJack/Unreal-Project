using UnityEngine;
using UnityEngine.UI;

namespace UnrealTeam.SB.Additional.Game
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class RaycastTarget : Graphic
    {
        public override void SetMaterialDirty() { }
        public override void SetVerticesDirty() { }
    }
}