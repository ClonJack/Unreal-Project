using UnityEngine;

namespace UnrealTeam.SB.Configs.Interaction
{
    [CreateAssetMenu(menuName = "Configs/Interaction UI", fileName = "InteractionUiConfig")]
    public class InteractionUiConfig : ScriptableObject
    {
        [field: SerializeField] public string TooltipDescription { get; private set; }
    }
}