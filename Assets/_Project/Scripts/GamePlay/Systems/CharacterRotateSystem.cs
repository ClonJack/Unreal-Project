﻿using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnrealTeam.SB.Components;
using UnrealTeam.SB.Views;

namespace UnrealTeam.SB.Systems
{
    public class CharacterRotateSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PlayerTag, CharacterRotateAction>> _filter = default;
        
        private readonly EcsPoolInject<ComponentRef<CharacterView>> _characterRefPool = default;
        private readonly EcsPoolInject<CharacterData> _characterDataPool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                UpdateRotate(entity);
            }
        }
        
        private void UpdateRotate(int entity)
        {
            ref var cameraView = ref _characterRefPool.Value.Get(entity).Value.CameraView;

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
        }
    }
}