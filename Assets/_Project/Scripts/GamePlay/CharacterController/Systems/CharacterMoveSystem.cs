using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnrealTeam.SB.Common.Ecs;
using UnrealTeam.SB.GamePlay.CharacterController.Components;
using UnrealTeam.SB.GamePlay.CharacterController.Views;
using PlayerTag = UnrealTeam.SB.GamePlay.CharacterController.Components.PlayerTag;

namespace UnrealTeam.SB.GamePlay.CharacterController.Systems
{
    public class CharacterMoveSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PlayerTag>> _filter;

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

            var characterInputs = new PlayerCharacterInputs
            {
                MoveAxisForward = characterData.DirectionMove.y,
                MoveAxisRight = characterData.DirectionMove.x,
                JumpDown = characterData.IsJump,
                CameraRotation = cameraView.transform.rotation,
            };

            characterView.SetInputs(ref characterInputs);
        }
    }
}