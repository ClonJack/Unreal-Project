using UnityEngine;
using UnrealTeam.SB.Common.Ecs.Binders;
using UnrealTeam.SB.Configs.Interaction;
using UnrealTeam.SB.GamePlay.Interaction.Components;

namespace UnrealTeam.SB.GamePlay.Interaction.Binders
{
    public class DrawInteractionUiBinder : EcsComponentBinder<DrawInteractionUiData>
    {
        [SerializeField] private InteractionUiConfig _config;


        protected override void InitData(ref DrawInteractionUiData component)
        {
            component.TooltipDescription = _config.TooltipDescription;
        }
    }
}