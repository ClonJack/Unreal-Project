using UnityEngine;
using UnrealTeam.SB.Common.Ecs.Binders;
using UnrealTeam.SB.Common.Enums;
using UnrealTeam.SB.GamePlay.CharacterController.Components;

namespace UnrealTeam.SB.GamePlay.CharacterController.Binders
{
    public class PlayerControlDataBinder : EcsComponentBinder<PlayerControlData>
    {
        [SerializeField] private PlayerControlState _startControlState = PlayerControlState.Character;
        
        
        protected override void InitData(ref PlayerControlData component)
        {
            component.CurrentState = _startControlState;
        }
    }
}