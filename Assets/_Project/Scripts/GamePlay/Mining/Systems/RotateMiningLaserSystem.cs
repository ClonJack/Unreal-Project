using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnrealTeam.SB.Common.Ecs;
using UnrealTeam.SB.GamePlay.Mining.Components;
using UnrealTeam.SB.GamePlay.Mining.Views;

namespace UnrealTeam.SB.GamePlay.Mining.Systems
{
    public class RotateMiningLaserSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<RotateMiningLaserAction>> _rotateActionFilter;
        private readonly EcsFilterInject<Inc<ComponentRef<MiningStationSyncView>>, Exc<RotateMiningLaserAction>> _withoutActionFilter;
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
            float targetAcceleration = rotateData.LaserAcceleration + direction * Time.deltaTime;
            rotateData.LaserAcceleration = Mathf.Clamp(targetAcceleration, min, max);
        }

        private static void UpdateRotation(ref RotateMiningStationData rotateData)
        {
            if (rotateData.LaserAcceleration == 0)
                return;
            
            var rotation = Vector3.zero;
            rotation.x = rotateData.LaserCurve.Evaluate(rotateData.LaserAcceleration) * rotateData.LaserSpeed * Time.deltaTime;
            rotateData.LaserBase.Rotate(rotation);
        }
    }
}