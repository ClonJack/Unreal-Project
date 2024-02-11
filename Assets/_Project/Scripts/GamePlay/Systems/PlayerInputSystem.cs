using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnrealTeam.SB.Components;
using UnrealTeam.SB.Extensions;
using UnrealTeam.SB.Input;

namespace UnrealTeam.SB.Systems
{
    public class PlayerInputSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PlayerTag>> _filter = default;
        private readonly EcsPoolInject<CharacterAction> _characterActionPool = default;
        private readonly EcsPoolInject<CharacterData> _characterDataPool = default;

        private readonly IInputService _inputService;

        public PlayerInputSystem(IInputService inputService)
        {
            _inputService = inputService;
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                if (_inputService.MoveAxisX.IsHold() || _inputService.MoveAxisY.IsHold() ||
                    _inputService.Look2DAxis.IsHold())
                {
                    ref var characterData = ref _characterDataPool.Value.Get(entity);

                    characterData.DirectionMove =
                        new Vector2(_inputService.MoveAxisX.Value(), _inputService.MoveAxisY.Value());
                    
                    characterData.DirectionLook = _inputService.Look2DAxis.Value2D();

                    _characterActionPool.Value.Add(entity);
                }
            }
        }
    }
}