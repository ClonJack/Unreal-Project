using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnrealTeam.SB.GamePlay.Mining.Components;
using UnrealTeam.SB.GamePlay.Mining.Views;

namespace UnrealTeam.SB.GamePlay.Mining.Systems
{
    public class MiningStationWarmSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<MiningStationWarmAction, MiningStationWarmData>> _includeActionFilter;
        private readonly EcsFilterInject<Inc<MiningStationWarmData>, Exc<MiningStationWarmAction>> _excludeActionFilter;
        private readonly EcsPoolInject<MiningStationWarmData> _warmDataPool;


        public void Run(IEcsSystems systems)
        {
            foreach (var stationEntity in _includeActionFilter.Value) 
                DoLaserAction(stationEntity);
            
            foreach (var stationEntity in _excludeActionFilter.Value) 
                UndoLaserAction(stationEntity);
        }

        private void DoLaserAction(int stationEntity)
        {
            ref var warmData = ref _warmDataPool.Value.Get(stationEntity);
            IncreaseWarmTime(warmData);
            
            if (!TryRaycastTarget(warmData, out var targetView))
                return;

            ReduceTargetDurability(warmData, targetView);
        }

        private void UndoLaserAction(int stationEntity)
        {
            ref var warmData = ref _warmDataPool.Value.Get(stationEntity);
            var isLaserCold = !TryDecreaseWarmTime(warmData);
            
            if (isLaserCold)
                return;
            
            if (!TryRaycastTarget(warmData, out var targetView))
                return;
            
            ReduceTargetDurability(warmData, targetView);
        }

        private static void IncreaseWarmTime(MiningStationWarmData warmData)
        {
            if (warmData.WarmTime == warmData.WarmDuration)
                return;
            
            warmData.WarmTime += Time.deltaTime;
            if (warmData.WarmTime > warmData.WarmDuration)
                warmData.WarmTime = warmData.WarmDuration;
        }

        /// <summary>
        /// Returns false when "Cold"
        /// </summary>
        /// <param name="warmData"></param>
        /// <returns></returns>
        private static bool TryDecreaseWarmTime(MiningStationWarmData warmData)
        {
            if (warmData.WarmTime == 0)
                return false;
            
            warmData.WarmTime -= Time.deltaTime;
            if (warmData.WarmTime <= 0)
            {
                warmData.WarmTime = 0;
                return false;
            }

            return true;
        }

        private bool TryRaycastTarget(MiningStationWarmData warmData, out ObjectDurabilitySyncView targetView)
        {
            var laserPoint = warmData.LaserSpawnPoint;
            if (Physics.Raycast(laserPoint.position, laserPoint.forward, out var hit, warmData.WarmMaxDistance, warmData.WarmTarget))
            {
                targetView = hit.collider.GetComponent<ObjectDurabilitySyncView>();
                return true;
            }

            targetView = null;
            return false;
        }

        private void ReduceTargetDurability(MiningStationWarmData warmData, ObjectDurabilitySyncView targetView)
        {
            var durationCoefficient = warmData.WarmDurationCurve.Evaluate(warmData.WarmTime / warmData.WarmDuration);
            
            var distance = Vector3.Distance(warmData.LaserSpawnPoint.position, targetView.transform.position);
            var distanceCoefficient = warmData.WarmDistanceMultiplierCurve.Evaluate(distance / warmData.WarmMaxDistance);
            
            var reduceValue = durationCoefficient * distanceCoefficient * warmData.WarmPower * Time.deltaTime;
            targetView.ReduceDurabilityRpc(reduceValue);
        }
    }
}