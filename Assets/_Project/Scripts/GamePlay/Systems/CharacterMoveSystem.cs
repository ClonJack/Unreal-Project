using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnrealTeam.SB.Components;
using UnrealTeam.SB.Views;

namespace UnrealTeam.SB.Systems
{
    public class CharacterMoveSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PlayerTag, CharacterMoveAction>> _filter = default;

        private readonly EcsPoolInject<ComponentRef<CameraView>> _cameraRefPool = default;
        private readonly EcsPoolInject<ComponentRef<CharacterView>> _characterRefPool = default;
        private readonly EcsPoolInject<CharacterData> _characterDataPool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                UpdateMove(entity);
            //    UpdateRotate(entity);
            }
        }


        private void UpdateMove(int entity)
        {
            ref var characterView = ref _characterRefPool.Value.Get(entity).Value;
            ref var cameraView = ref _characterRefPool.Value.Get(entity).Value.CameraView;
            ref var characterData = ref _characterDataPool.Value.Get(entity);

            var directionMove = characterData.DirectionMove;

            var moveInputVector =
                Vector3.ClampMagnitude(new Vector3(directionMove.x, 0f, directionMove.y), 1f);

            var targetCamera =
                Vector3.ProjectOnPlane(cameraView.transform.rotation * Vector3.forward, characterView.Motor.CharacterUp)
                    .normalized;

            if (targetCamera.sqrMagnitude == 0f)
            {
                targetCamera = Vector3.ProjectOnPlane(cameraView.transform.rotation * Vector3.up,
                        characterView.Motor.CharacterUp)
                    .normalized;
            }

            var cameraPlanarRotation = Quaternion.LookRotation(targetCamera, characterView.Motor.CharacterUp);

            characterView.UpdateMove(targetCamera, cameraPlanarRotation * moveInputVector);
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