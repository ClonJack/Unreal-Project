using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnrealTeam.SB.Common.Ecs.Binders;
using UnrealTeam.SB.GamePlay.Durability.Additional;

namespace UnrealTeam.SB.GamePlay.Durability.Components
{
    public struct DurabilityDrawUiData
    {
        public bool IsActive;
        public GameObject DurabilityCanvas;
        public EcsEntityProvider DurabilityCanvasProvider;
        public Image DurabilityBar;
        public TextMeshProUGUI DurabilityText;
        public DurabilityDrawType DrawType;
    }
}