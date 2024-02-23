using GamePlay.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Services.Input;

namespace GamePlay.Systems
{
    public class PlayerInputSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PlayerTag, MovementData>> _filter;
        private readonly EcsPoolInject<MovementData> _movementDataPool;
        
        private readonly IInputService _inputService;

        
        public PlayerInputSystem(IInputService inputService)
        {
            _inputService = inputService;
        }

        public void Run(IEcsSystems systems)
        {
            var direction = _inputService.GetMoveDirection();
            foreach (int entity in _filter.Value)
            {
                _movementDataPool.Value.Get(entity).Direction = direction;
            }
        }
    }
}