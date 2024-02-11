using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnrealTeam.SB.Components;
using UnrealTeam.SB.Views;

namespace UnrealTeam.SB.Systems
{
    public class CharacterSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PlayerTag, CharacterAction>> _filter = default;

        private readonly EcsPoolInject<ComponentRef<CameraView>> _cameraRefPool = default;
        private readonly EcsPoolInject<ComponentRef<CharacterView>> _characterRefPool = default;
        private readonly EcsPoolInject<CharacterData> _characterDataPool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                RotateCamera(entity);
            }
        }

        private void RotateCamera(int entity)
        {
            ref var cameraRef = ref _cameraRefPool.Value.Get(entity);
            ref var characterRef = ref _characterRefPool.Value.Get(entity);
            ref var characterData = ref _characterDataPool.Value.Get(entity);

            var rotationSpeed = 15f * Time.deltaTime;
            var rotationSharpness = 1000;

            var rotationFromInput =
                Quaternion.Euler(cameraRef.Value.transform.up *
                                 (characterData.DirectionLook.x * (rotationSpeed)));

            characterData.TargetRotation = Vector3.ProjectOnPlane(rotationFromInput * characterData.TargetRotation,
                characterRef.Value.transform.up);

            var planarRot =
                Quaternion.LookRotation(characterData.TargetRotation, characterRef.Value.transform.up);

            characterData.TargetVerticalAngle -= (characterData.DirectionLook.y * (rotationSpeed));
            characterData.TargetVerticalAngle = Mathf.Clamp(characterData.TargetVerticalAngle, -85f,
                85f);

            var verticalRot = Quaternion.Euler(characterData.TargetVerticalAngle, 0, 0);
            var targetRotation = Quaternion.Slerp(cameraRef.Value.transform.rotation, planarRot * verticalRot,
                1f - Mathf.Exp(-rotationSharpness * Time.deltaTime));

            cameraRef.Value.transform.rotation = targetRotation;
        }
    }
}