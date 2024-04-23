using TMPro;
using TriInspector;
using UnityEngine;
using UnityEngine.UI;
using UnrealTeam.SB.Common.Ecs.Binders;
using UnrealTeam.SB.GamePlay.Durability.Additional;
using UnrealTeam.SB.GamePlay.Durability.Components;

namespace UnrealTeam.SB.GamePlay.Durability.Binders
{
    public class DurabilityDrawUiBinder : EcsComponentBinder<DurabilityDrawUiData>
    {
        [SerializeField] private GameObject _durabilityCanvas;
        [SerializeField] private Image _durabilityBar;
        [SerializeField] private TextMeshProUGUI _durabilityText;
        [SerializeField, HideIf(nameof(_durabilityText), null)] private DurabilityDrawType _drawType = DurabilityDrawType.Percents;


        protected override void InitData(ref DurabilityDrawUiData component)
        {
            component.DurabilityCanvas = _durabilityCanvas;
            component.DurabilityCanvasProvider = _durabilityCanvas.GetComponent<EcsEntityProvider>();
            component.DurabilityBar = _durabilityBar;
            component.DurabilityText = _durabilityText;
            component.DrawType = _drawType;
        }
    }
}