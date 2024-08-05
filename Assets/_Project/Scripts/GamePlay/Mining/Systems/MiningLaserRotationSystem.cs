using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnrealTeam.SB.GamePlay.Mining.Components;

namespace UnrealTeam.SB.GamePlay.Mining.Systems
{
    public class MiningLaserRotationSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<MiningLaserRotationAction, MiningLaserRotationData>> _includeActionFilter;

        private readonly EcsFilterInject<Inc<MiningLaserRotationData>, Exc<MiningLaserRotationAction>>
            _excludeActionFilter;

        private readonly EcsPoolInject<MiningLaserRotationAction> _rotationActionPool;
        private readonly EcsPoolInject<MiningLaserRotationData> _rotationDataPool;


        public void Run(IEcsSystems systems)
        {
            foreach (var stationEntity in _includeActionFilter.Value)
            {
                ref var rotateData = ref AccelerateRotation(stationEntity);
                UpdateRotation(ref rotateData);
            }

            foreach (var stationEntity in _excludeActionFilter.Value)
            {
                ref var rotateData = ref DecelerateRotation(stationEntity);
                UpdateRotation(ref rotateData);
            }
        }

        private ref MiningLaserRotationData AccelerateRotation(int stationEntity)
        {
            ref var rotateData = ref _rotationDataPool.Value.Get(stationEntity);
            ref var rotateAction = ref _rotationActionPool.Value.Get(stationEntity);

            if (rotateData.AccelerationValue < 0 && rotateAction.ValueX > 0)
                rotateData.AccelerationValue = 0;
            else if (rotateData.AccelerationValue > 0 && rotateAction.ValueX < 0)
                rotateData.AccelerationValue = 0;

            ChangeAcceleration(ref rotateData, rotateAction.ValueX, -1, 1);
            return ref rotateData;
        }

        private ref MiningLaserRotationData DecelerateRotation(int stationEntity)
        {
            ref var rotateData = ref _rotationDataPool.Value.Get(stationEntity);
            if (rotateData.AccelerationValue == 0)
                return ref rotateData;

            if (rotateData.AccelerationValue < 0)
                ChangeAcceleration(ref rotateData, 1, -1, 0);
            else if (rotateData.AccelerationValue > 0)
                ChangeAcceleration(ref rotateData, -1, 0, 1);

            return ref rotateData;
        }

        private void ChangeAcceleration(ref MiningLaserRotationData rotationData, float direction, int min, int max)
        {
            var targetAcceleration = rotationData.AccelerationValue +
                                     direction * Time.deltaTime / rotationData.AccelerationDuration;
            rotationData.AccelerationValue = Mathf.Clamp(targetAcceleration, min, max);
        }

        private static void UpdateRotation(ref MiningLaserRotationData rotationData)
        {
            if (rotationData.AccelerationValue == 0)
                return;

            var rotationOffsetX = rotationData.RotationCurve.Evaluate(Mathf.Abs(rotationData.AccelerationValue)) *
                                  rotationData.RotationSpeed;
            if (rotationData.AccelerationValue < 0)
                rotationOffsetX *= -1;

            var rotationOffset = new Vector3(0, rotationOffsetX, 0);
            rotationData.LaserBase.RotateRpc(rotationOffset);
        }
    }
}