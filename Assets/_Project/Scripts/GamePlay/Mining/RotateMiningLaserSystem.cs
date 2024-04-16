using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace UnrealTeam.SB.GamePlay.Mining
{
    public class RotateMiningLaserSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<RotateMiningLaserAction>> _filter;
        private readonly EcsPoolInject<RotateMiningLaserAction> _rotateActionPool;
        private readonly EcsPoolInject<RotateMiningStationData> _rotateDataPool;


        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                var rotateData = _rotateDataPool.Value.Get(entity);
                var rotateAction = _rotateActionPool.Value.Get(entity);
                
                
                _rotateActionPool.Value.Del(entity);
            }
        }
    }    
    
    public class RotateMiningPlatformSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<RotateMiningPlatformAction>> _filter;
        private readonly EcsPoolInject<RotateMiningPlatformAction> _rotateActionPool;
        private readonly EcsPoolInject<RotateMiningStationData> _rotateDataPool;


        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                var rotateData = _rotateDataPool.Value.Get(entity);
                var rotateAction = _rotateActionPool.Value.Get(entity);
                
                
                _rotateActionPool.Value.Del(entity);
            }
        }
    }
}