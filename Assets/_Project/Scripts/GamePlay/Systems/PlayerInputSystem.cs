using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnrealTeam.SB.Components;
using UnrealTeam.SB.Input;

namespace UnrealTeam.SB.Systems
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
                ref var characterMove = ref _characterDataPool.Value.Get(entity);
                
                if (_inputService.Mouse.IsPressed())
                {
                    Cursor.lockState = CursorLockMode.Locked;
                }

                if (_inputService.Look2DAxis.IsHold())
                {
                    characterMove.DirectionLook = _inputService.Look2DAxis.Value2D();
                    
                    _characterRotateAcitonPool.Value.Add(entity);

                }
                
                if (_inputService.MoveAxisX.IsHold() || _inputService.MoveAxisY.IsHold())
                {
                    characterMove.DirectionMove =
                        new Vector2(_inputService.MoveAxisX.Value(), _inputService.MoveAxisY.Value());

                    _characterMoveActionPool.Value.Add(entity);
                }
            }
        }
    }
}