using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnrealTeam.SB.GamePlay.Mining.Components;

namespace UnrealTeam.SB.GamePlay.Mining.Systems
{
    public class MiningLaserDrawSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<MiningLaserWarmedEvent, MiningLaserDrawData>> _warmedEventFilter;
        private readonly EcsFilterInject<Inc<MiningLaserCooledEvent, MiningLaserDrawData>> _cooledEventFilter;
        private readonly EcsPoolInject<MiningLaserWarmedEvent> _warmedEventPool;
        private readonly EcsPoolInject<MiningLaserDrawData> _laserDrawPool;
        private readonly EcsPoolInject<MiningLaserWarmData> _warmDataPool;
        
        
        public void Run(IEcsSystems systems)
        {
            foreach (var stationEntity in _warmedEventFilter.Value)
                DrawLaser(stationEntity);

            foreach (var stationEntity in _cooledEventFilter.Value) 
                UnDrawLaser(stationEntity);
        }

        private void DrawLaser(int stationEntity)
        {
            ref var drawData = ref _laserDrawPool.Value.Get(stationEntity);
            ref var warmData = ref _warmDataPool.Value.Get(stationEntity);

            var powerCoefficient = _warmedEventPool.Value.Get(stationEntity).PowerCoefficient;
            var laserSpawn = warmData.LaserSpawnPoint;
            var laserRenderer = drawData.LaserRenderer;
            laserRenderer.enabled = true;

            var laserStartPosition = laserSpawn.position;
            var laserEndPosition = laserStartPosition + laserSpawn.forward * warmData.WarmMaxDistance;
            laserRenderer.SetPosition(0, laserStartPosition);
            laserRenderer.SetPosition(1, laserEndPosition);

            var laserWidth = drawData.WidthCurve.Evaluate(powerCoefficient);
            laserRenderer.startWidth = laserWidth;
            laserRenderer.endWidth = laserWidth;

            var colorCoefficient = drawData.ColorCurve.Evaluate(powerCoefficient);
            var alphaCoefficient = drawData.AlphaCurve.Evaluate(powerCoefficient);
            var laserColor = Color.Lerp(drawData.MinColor, drawData.MaxColor, colorCoefficient);
            laserColor.a = alphaCoefficient;
            laserRenderer.startColor = laserColor;
            laserRenderer.endColor = laserColor;
        }

        private void UnDrawLaser(int stationEntity)
        {
            ref var laserDrawData = ref _laserDrawPool.Value.Get(stationEntity);
            laserDrawData.LaserRenderer.enabled = false;
        }
    }
}