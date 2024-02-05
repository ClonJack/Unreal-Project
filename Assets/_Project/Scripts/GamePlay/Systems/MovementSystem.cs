using GamePlay.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace GamePlay.Systems
{
    public class MovementSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<MovementData>> _filter;
        private readonly EcsPoolInject<MovementData> _movementDataPool;
        private readonly EcsPoolInject<ComponentRef<Rigidbody>> _rigidbodyRefPool;
        private readonly EcsPoolInject<ComponentRef<Transform>> _transformRefPool;
        
        
        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter.Value)
            {
                ref var movementData = ref _movementDataPool.Value.Get(entity);
                MoveEntity(entity, movementData);
            }
        }

        private void MoveEntity(int entity, MovementData movementData)
        {
        }
    }
}