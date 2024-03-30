using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnrealTeam.SB.Common.Ecs;
using UnrealTeam.SB.GamePlay.Actions;
using UnrealTeam.SB.GamePlay.Components;
using UnrealTeam.SB.GamePlay.Tags;
using UnrealTeam.SB.GamePlay.Views;

namespace UnrealTeam.SB.GamePlay.Systems
{
    public class CharacterRotateSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PlayerTag, CharacterRotateAction>> _filter;

        private readonly EcsPoolInject<CharacterData> _characterDataPool;
        
        private readonly EcsPoolInject<ComponentRef<CharacterView>> _characterRefPool;
        private readonly EcsPoolInject<ComponentRef<CameraView>> _cameraRefPool;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                UpdateRotate(entity);
            }
        }

        private void UpdateRotate(int entity)
        {
            ref var cameraView = ref _cameraRefPool.Value.Get(entity).Value;
            ref var characterView = ref _characterRefPool.Value.Get(entity).Value;

            var rotationSpeed = cameraView.RotationSpeed * Time.deltaTime;
            var rotationSharpness = cameraView.RotationSharpness * Time.deltaTime;
            var minVerticalAngle = cameraView.MinVerticalAngle;
            var maxVerticalAngle = cameraView.MaxVerticalAngle;
            var followTarget = cameraView.FollowTransform;
            var camera = cameraView.transform;

            ref var characterData = ref _characterDataPool.Value.Get(entity);
            var lookY = characterData.DirectionLook.y;
            var lookX = characterData.DirectionLook.x;

            var rotationFromInput = Quaternion.Euler(camera.transform.up * (lookX * rotationSpeed));

            cameraView.TargetRotation = Vector3.ProjectOnPlane(rotationFromInput * cameraView.TargetRotation,
                followTarget.up);

            cameraView.TargetVerticalAngle = Mathf.Clamp(cameraView.TargetVerticalAngle - (lookY * rotationSpeed),
                minVerticalAngle,
                maxVerticalAngle);

            var verticalRot = Quaternion.Euler(cameraView.TargetVerticalAngle, 0, 0);
            var planarRot =
                Quaternion.LookRotation(cameraView.TargetRotation, followTarget.up);
            var resultRotation = Quaternion.Slerp(camera.rotation, planarRot * verticalRot,
                1f - Mathf.Exp(-rotationSharpness));

            camera.rotation = resultRotation;

            camera.position = characterView.CameraTarget.position;
        }
    }
}