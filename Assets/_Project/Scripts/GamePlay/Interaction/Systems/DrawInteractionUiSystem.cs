using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnrealTeam.SB.GamePlay.Interaction.Components;
using UnrealTeam.SB.Services.Other;

namespace UnrealTeam.SB.GamePlay.Interaction.Systems
{
    public class DrawInteractionUiSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<InteractAction, DrawInteractionUiData>> _interactedFilter;
        private readonly EcsFilterInject<Inc<EndInteractAction, DrawInteractionUiData>> _endInteractionFilter;
        private readonly EcsPoolInject<DrawInteractionUiData> _drawInteractionPool;
        private readonly ObjectsProvider _objectsProvider;


        public DrawInteractionUiSystem(ObjectsProvider objectsProvider)
        {
            _objectsProvider = objectsProvider;
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var interactedEntity in _endInteractionFilter.Value) 
                ClearInteractionUi(interactedEntity);
            
            foreach (var interactedEntity in _interactedFilter.Value) 
                DrawInteractionUi(interactedEntity);
        }

        private void DrawInteractionUi(int interactedEntity)
        {
            ref var drawInteractionData = ref _drawInteractionPool.Value.Get(interactedEntity);
            if (drawInteractionData.IsRendered)
                return;

            var hudRefs = _objectsProvider.HudRefs;
            hudRefs.InteractTooltipText.text = drawInteractionData.TooltipDescription;
            drawInteractionData.IsRendered = true;
        }

        private void ClearInteractionUi(int interactedEntity)
        {
            ref var drawInteractionData = ref _drawInteractionPool.Value.Get(interactedEntity);
            if (!drawInteractionData.IsRendered)
                return;
            
            var hudRefs = _objectsProvider.HudRefs;
            hudRefs.InteractTooltipText.text = string.Empty;
            drawInteractionData.IsRendered = false;
        }
    }
}