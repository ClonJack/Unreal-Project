using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnrealTeam.SB.Common.Ecs;
using UnrealTeam.SB.GamePlay.Common.Components;
using UnrealTeam.SB.Services.Other;

namespace UnrealTeam.SB.GamePlay.Common.Systems
{
    public class LookAtCameraSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<LookAtCameraMarker>> _markerFilter;
        private readonly EcsPoolInject<ComponentRef<Transform>> _transformPool;
        private readonly ObjectsProvider _objectsProvider;


        public LookAtCameraSystem(ObjectsProvider objectsProvider)
        {
            _objectsProvider = objectsProvider;
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _markerFilter.Value)
            {
                var transform = _transformPool.Value.Get(entity).Component;
                transform.LookAt(_objectsProvider.GameCameraTransform, Vector3.up);
            }
        }
    }
}