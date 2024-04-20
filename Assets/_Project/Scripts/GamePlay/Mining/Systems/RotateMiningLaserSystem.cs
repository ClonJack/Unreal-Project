using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnrealTeam.SB.GamePlay.Mining.Components;

namespace UnrealTeam.SB.GamePlay.Mining.Systems
{
    public class RotateMiningLaserSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<RotateMiningStationData, RotateMiningLaserAction>> _rotateActionFilter;
        private readonly EcsFilterInject<Inc<RotateMiningStationData>, Exc<RotateMiningLaserAction>> _withoutActionFilter;
        private readonly EcsPoolInject<RotateMiningLaserAction> _rotateActionPool;
        private readonly EcsPoolInject<RotateMiningStationData> _rotateDataPool;


        public void Run(IEcsSystems systems)
        {
            foreach (var stationEntity in _rotateActionFilter.Value)
            {
                ref var rotateData = ref AccelerateRotation(stationEntity);
                UpdateRotation(ref rotateData);
            }

            foreach (var stationEntity in _withoutActionFilter.Value)
            {
                ref var rotateData = ref DecelerateRotation(stationEntity);
                UpdateRotation(ref rotateData);
            }
        }

        private ref RotateMiningStationData AccelerateRotation(int stationEntity)
        {
            ref var rotateData = ref _rotateDataPool.Value.Get(stationEntity);
            ref var rotateAction = ref _rotateActionPool.Value.Get(stationEntity);

            if (rotateData.LaserAcceleration < 0 && rotateAction.ValueX > 0)
                rotateData.LaserAcceleration = 0;
            else if (rotateData.LaserAcceleration > 0 && rotateAction.ValueX < 0)
                rotateData.LaserAcceleration = 0;
                
            ChangeAcceleration(ref rotateData, rotateAction.ValueX, -1, 1);
            return ref rotateData;
        }

        private ref RotateMiningStationData DecelerateRotation(int stationEntity)
        {
            ref var rotateData = ref _rotateDataPool.Value.Get(stationEntity);
            if (rotateData.LaserAcceleration == 0)
                return ref rotateData;
            
            if (rotateData.LaserAcceleration < 0)
                ChangeAcceleration(ref rotateData, 1, -1, 0);
            else if (rotateData.LaserAcceleration > 0)
                ChangeAcceleration(ref rotateData, -1, 0, 1);
            
            return ref rotateData;
        }

        private void ChangeAcceleration(ref RotateMiningStationData rotateData, float direction, int min, int max)
        {
            float targetAcceleration = rotateData.LaserAcceleration + direction * Time.deltaTime / rotateData.LaserAccelerationDuration;
            rotateData.LaserAcceleration = Mathf.Clamp(targetAcceleration, min, max);
        }

        private static void UpdateRotation(ref RotateMiningStationData rotateData)
        {
            if (rotateData.LaserAcceleration == 0)
                return;

            var targetRotationX = rotateData.LaserCurve.Evaluate(Mathf.Abs(rotateData.LaserAcceleration)) * rotateData.LaserSpeed * Time.deltaTime;
            if (rotateData.LaserAcceleration < 0)
                targetRotationX *= -1;
            
            var rotation = Vector3.zero;
            rotation.y = targetRotationX;
            rotateData.LaserBase.Rotate(rotation);
        }
    }
}