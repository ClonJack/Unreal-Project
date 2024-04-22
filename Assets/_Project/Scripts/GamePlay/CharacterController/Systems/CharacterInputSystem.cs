using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnrealTeam.SB.Additional.Enums;
using UnrealTeam.SB.Common.Ecs;
using UnrealTeam.SB.Common.Ecs.Extensions;
using UnrealTeam.SB.Common.Extensions;
using UnrealTeam.SB.GamePlay.CharacterController.Components;
using UnrealTeam.SB.GamePlay.CharacterController.Views;
using UnrealTeam.SB.Services.InputControl.Interfaces;

namespace UnrealTeam.SB.GamePlay.CharacterController.Systems
{
    public class CharacterInputSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PlayerControlData>> _filter;
        private readonly EcsPoolInject<PlayerControlData> _playerControlPool;
        private readonly EcsPoolInject<CharacterControlData> _characterControlPool;
        private readonly EcsPoolInject<ComponentRef<CameraView>> _cameraViewPool;
        private readonly EcsPoolInject<CharacterMoveAction> _moveActionPool;
        private readonly EcsPoolInject<CharacterRotateAction> _rotateActionPool;
        private readonly EcsPoolInject<CharacterUseAction> _useActionPool;
        
        private readonly IInputService _inputService;
        
        
        public CharacterInputSystem(IInputService inputService)
        {
            _inputService = inputService;
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var playerEntity in _filter.Value) 
                HandleInputs(playerEntity);
        }

        private void HandleInputs(int playerEntity)
        {
            if (_inputService.Mouse.IsPressed()) 
                Cursor.lockState = CursorLockMode.Locked;

            ref var characterData = ref _characterControlPool.Value.Get(playerEntity);
            ref var cameraView = ref _cameraViewPool.Value.Get(playerEntity).Component;
            var playerControlState = _playerControlPool.Value.Get(playerEntity).CurrentState;

            characterData.LookDirection = _inputService.Look2DAxis.Value2D();
            _rotateActionPool.Value.Add(playerEntity);

            if (playerControlState != PlayerControlState.Character) 
                return;
                
            HandleCharacterInputs(playerEntity, cameraView, ref characterData);
        }

        private void HandleCharacterInputs(int playerEntity, CameraView cameraView, ref CharacterControlData characterData)
        {
            characterData.DirectionMove = new Vector2(_inputService.MoveAxisX.Value(), _inputService.MoveAxisY.Value());
            characterData.IsJump = _inputService.Jump.IsPressed();
            characterData.CameraRotation = cameraView.transform.rotation;
            _moveActionPool.Value.Add(playerEntity);

            if (_inputService.Use.IsPressed()) 
                _useActionPool.Value.GetOrAdd(playerEntity);
        }
    }
}