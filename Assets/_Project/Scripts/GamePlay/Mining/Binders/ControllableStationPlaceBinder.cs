using UnityEngine;
using UnrealTeam.SB.Common.Ecs.Binders;
using UnrealTeam.SB.GamePlay.Mining.Components;

namespace UnrealTeam.SB.GamePlay.Mining.Binders
{
    public class ControllableStationPlaceBinder : EcsComponentBinder<ControllableStationPlace>
    {
        [SerializeField] private Transform _sitPoint;
        [SerializeField] private Collider _collider;
        
        protected override void InitData(ref ControllableStationPlace component)
        {
            component.SitPoint = _sitPoint;
            component.Collider = _collider;
        }
    }
}