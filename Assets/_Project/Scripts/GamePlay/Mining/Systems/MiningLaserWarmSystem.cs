using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnrealTeam.SB.Common.Ecs;
using UnrealTeam.SB.Common.Ecs.Binders;
using UnrealTeam.SB.GamePlay.Durability.Components;
using UnrealTeam.SB.GamePlay.Mining.Components;
using UnrealTeam.SB.GamePlay.Mining.Views;

namespace UnrealTeam.SB.GamePlay.Mining.Systems
{
    public class MiningLaserWarmSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<MiningLaserWarmAction, MiningLaserWarmData>> _includeActionFilter;
        private readonly EcsFilterInject<Inc<MiningLaserWarmData>, Exc<MiningLaserWarmAction>> _excludeActionFilter;
        private readonly EcsPoolInject<MiningLaserWarmData> _warmDataPool;
        private readonly EcsPoolInject<DurabilityChangeRequest> _durabilityChangeRequestPool;
        private readonly EcsPoolInject<ComponentRef<MiningStationSyncView>> _stationSyncViewPool;


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
            IncreaseWarmTime(ref warmData);
            SendLaserWarmedEvent(ref warmData, stationEntity);
            if (TryRaycastTarget(ref warmData, out var targetProvider))
                SendDurabilityReduceRequest(ref warmData, targetProvider);
        }

        private void UndoLaserAction(int stationEntity)
        {
            ref var warmData = ref _warmDataPool.Value.Get(stationEntity);
            var isLaserCold = !TryDecreaseWarmTime(ref warmData);
            if (isLaserCold)
            {
                SendLaserCooledEvent(ref warmData, stationEntity);
                return;
            }
            
            SendLaserWarmedEvent(ref warmData, stationEntity);
            if (TryRaycastTarget(ref warmData, out var targetProvider))
                SendDurabilityReduceRequest(ref warmData, targetProvider);
        }

        private static void IncreaseWarmTime(ref MiningLaserWarmData warmData)
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
        private static bool TryDecreaseWarmTime(ref MiningLaserWarmData warmData)
        {
            warmData.WarmTime -= Time.deltaTime;
            if (warmData.WarmTime <= 0)
            {
                warmData.WarmTime = 0;
                return false;
            }

            return true;
        }

        private bool TryRaycastTarget(ref MiningLaserWarmData warmData, out EcsEntityProvider targetProvider)
        {
            var laserPoint = warmData.LaserSpawnPoint;
            if (Physics.Raycast(laserPoint.position, laserPoint.forward, out var hit, warmData.WarmMaxDistance, warmData.WarmTarget))
            {
                targetProvider = hit.collider.GetComponent<EcsEntityProvider>();
                return true;
            }

            targetProvider = null;
            return false;
        }

        private void SendDurabilityReduceRequest(ref MiningLaserWarmData warmData, EcsEntityProvider targetProvider)
        {
            var durationCoefficient = warmData.WarmDurationCurve.Evaluate(warmData.WarmTime / warmData.WarmDuration);
            
            var distance = Vector3.Distance(warmData.LaserSpawnPoint.position, targetProvider.transform.position);
            var distanceCoefficient = warmData.WarmDistanceMultiplierCurve.Evaluate(distance / warmData.WarmMaxDistance);
            
            var reduceValue = durationCoefficient * distanceCoefficient * warmData.WarmPower * Time.deltaTime;
            _durabilityChangeRequestPool.Value.Add(targetProvider.Entity).DurabilityDiff = -reduceValue;
        }

        private void SendLaserWarmedEvent(ref MiningLaserWarmData warmData, int stationEntity)
        {
            var stationSyncView = _stationSyncViewPool.Value.Get(stationEntity).Component;
            var powerCoefficient = warmData.WarmDurationCurve.Evaluate(warmData.WarmTime / warmData.WarmDuration);
            stationSyncView.ChangeLaserPowerRpc(powerCoefficient);
            warmData.IsCooledEventSent = false;
        }
        
        private void SendLaserCooledEvent(ref MiningLaserWarmData warmData, int stationEntity)
        {
            if (warmData.IsCooledEventSent)
                return;
            
            var stationSyncView = _stationSyncViewPool.Value.Get(stationEntity).Component;
            stationSyncView.ChangeLaserPowerRpc(0);
            warmData.IsCooledEventSent = true;
        }
    }
}