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
    public class CharacterMoveSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PlayerTag, CharacterMoveAction>> _filter;
        
        private readonly EcsPoolInject<CharacterData> _characterDataPool;
        
        private readonly EcsPoolInject<ComponentRef<CharacterView>> _characterRefPool;
        private readonly EcsPoolInject<ComponentRef<CameraView>> _cameraRefPool;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                UpdateMove(entity);
            }
        }
        private void UpdateMove(int entity)
        {
            ref var characterView = ref _characterRefPool.Value.Get(entity).Component;
            ref var cameraView = ref _cameraRefPool.Value.Get(entity).Component;
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
            characterView.UpdateJump(characterData.IsJump);
        }
    }
}