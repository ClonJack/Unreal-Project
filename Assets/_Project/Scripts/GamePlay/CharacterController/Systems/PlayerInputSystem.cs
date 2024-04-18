using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnrealTeam.SB.Common.Ecs;
using UnrealTeam.SB.Common.Enums;
using UnrealTeam.SB.GamePlay.CharacterController.Components;
using UnrealTeam.SB.GamePlay.CharacterController.Views;
using UnrealTeam.SB.Services.InputControl.Interfaces;

namespace UnrealTeam.SB.GamePlay.CharacterController.Systems
{
    public class PlayerInputSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PlayerControlData>> _filter;
        private readonly EcsPoolInject<PlayerControlData> _playerControlPool;
        private readonly EcsPoolInject<CharacterControlData> _characterControlPool;
        private readonly EcsPoolInject<ComponentRef<CameraView>> _cameraViewPool;
        private readonly EcsPoolInject<CharacterMoveAction> _moveActionPool;
        private readonly EcsPoolInject<CharacterRotateAction> _rotateActionPool;
        
        private readonly IInputService _inputService;
        
        
        public PlayerInputSystem(IInputService inputService)
        {
            _inputService = inputService;
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                if (_inputService.Mouse.IsPressed()) 
                    Cursor.lockState = CursorLockMode.Locked;

                ref var characterData = ref _characterControlPool.Value.Get(entity);
                ref var cameraView = ref _cameraViewPool.Value.Get(entity).Component;
                var playerControlState = _playerControlPool.Value.Get(entity).CurrentState;

                characterData.LookDirection = _inputService.Look2DAxis.Value2D();
                _rotateActionPool.Value.Add(entity);

                if (playerControlState == PlayerControlState.Character)
                {
                    characterData.DirectionMove = new Vector2(_inputService.MoveAxisX.Value(), _inputService.MoveAxisY.Value());
                    characterData.IsJump = _inputService.Jump.IsPressed();
                    characterData.CameraRotation = cameraView.transform.rotation;
                    _moveActionPool.Value.Add(entity);
                }
            }
        }
    }
}