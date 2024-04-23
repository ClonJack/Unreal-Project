using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnrealTeam.SB.Additional.Constants;
using UnrealTeam.SB.Common.Ecs.Extensions;
using UnrealTeam.SB.GamePlay.Common.Components;
using UnrealTeam.SB.GamePlay.Durability.Additional;
using UnrealTeam.SB.GamePlay.Durability.Components;

namespace UnrealTeam.SB.GamePlay.Durability.Systems
{
    public class DurabilityDrawUiSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<DurabilityChangedEvent, DurabilityDrawUiData>> _eventFilter;
        private readonly EcsPoolInject<DurabilityChangedEvent> _durabilityEventPool;
        private readonly EcsPoolInject<DurabilityDrawUiData> _durabilityUiPool;
        private readonly EcsPoolInject<LookAtCameraMarker> _lookCameraMarkerPool;
        
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _eventFilter.Value) 
                DrawDurabilityUi(entity);
        }

        private void DrawDurabilityUi(int entity)
        {
            ref var durabilityEvent = ref _durabilityEventPool.Value.Get(entity);
            ref var durabilityUiData = ref _durabilityUiPool.Value.Get(entity);

            var isDurabilityFull = IsDurabilityFull(durabilityEvent);
                
            if (isDurabilityFull)
            {
                if (durabilityUiData.IsActive) 
                    ToggleUi(durabilityUiData, false);
            }
            else
            {
                if (!durabilityUiData.IsActive) 
                    ToggleUi(durabilityUiData, true);
                    
                DrawBar(durabilityUiData, durabilityEvent);
                DrawText(durabilityUiData, durabilityEvent);
            }
        }

        private void ToggleUi(DurabilityDrawUiData durabilityUiData, bool isActive)
        {
            durabilityUiData.DurabilityCanvas.SetActive(isActive);
            durabilityUiData.IsActive = isActive;
            var canvasEntity = durabilityUiData.DurabilityCanvasProvider.Entity;
            if (isActive)
                _lookCameraMarkerPool.Value.GetOrAdd(canvasEntity);
            else
                _lookCameraMarkerPool.Value.Del(canvasEntity);
        }

        private static void DrawBar(DurabilityDrawUiData durabilityUi, DurabilityChangedEvent durabilityEvent)
        {
            if (durabilityUi.DurabilityBar == null) 
                return;
            
            durabilityUi.DurabilityBar.fillAmount = durabilityEvent.Durability / durabilityEvent.MaxDurability;
        }

        private static void DrawText(DurabilityDrawUiData durabilityUi, DurabilityChangedEvent durabilityEvent)
        {
            if (durabilityUi.DurabilityText == null) 
                return;

            if (durabilityUi.DrawType == DurabilityDrawType.Division)
            {
                var current = ((int)durabilityEvent.Durability).ToString();
                var max = ((int)durabilityEvent.MaxDurability).ToString();
                durabilityUi.DurabilityText.text = $"{current}/{max}";
            }
            else if (durabilityUi.DrawType == DurabilityDrawType.Percents)
            {
                var percents = Mathf.CeilToInt(100 * durabilityEvent.Durability / durabilityEvent.MaxDurability).ToString();
                durabilityUi.DurabilityText.text = $"{percents}%";
            }
        }

        private static bool IsDurabilityFull(DurabilityChangedEvent durabilityEvent) 
            => Math.Abs(durabilityEvent.MaxDurability - durabilityEvent.Durability) < GameConstants.Tolerance;
    }
}