using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnrealTeam.SB.Common.Extensions;
using UnrealTeam.SB.GamePlay.Actions;
using UnrealTeam.SB.GamePlay.Components;
using UnrealTeam.SB.GamePlay.Tags;
using UnrealTeam.SB.Services.InputControl.Interfaces;

namespace UnrealTeam.SB.GamePlay.Systems
{
    public class PlayerInputSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PlayerTag>> _filter;

        private readonly EcsPoolInject<CharacterMoveAction> _characterMoveActionPool;
        private readonly EcsPoolInject<CharacterRotateAction> _characterRotateAcitonPool;
        private readonly EcsPoolInject<CharacterData> _characterDataPool;

        private readonly IInputService _inputService;

        public PlayerInputSystem(IInputService inputService)
        {
            _inputService = inputService;
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var characterData = ref _characterDataPool.Value.Get(entity);
                
                characterData.DirectionMove =
                    new Vector2(_inputService.MoveAxisX.Value(), _inputService.MoveAxisY.Value());
                
                characterData.DirectionLook = _inputService.Look2DAxis.Value2D();
                characterData.IsJump = _inputService.Jump.IsPressed();
                
                if (_inputService.Mouse.IsPressed())
                {
                    Cursor.lockState = CursorLockMode.Locked;
                }
                
                _characterRotateAcitonPool.Value.Replace(entity);
                _characterMoveActionPool.Value.Replace(entity);
            }
        }
    }
}