using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnrealTeam.SB.Common.Ecs;
using UnrealTeam.SB.GamePlay.CharacterController.Components;
using UnrealTeam.SB.GamePlay.CharacterController.Views;

namespace UnrealTeam.SB.GamePlay.CharacterController.Systems
{
    public class CharacterMoveSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<CharacterMoveAction>> _filter;
        private readonly EcsPoolInject<CharacterControlData> _characterControlPool;
        private readonly EcsPoolInject<PlayerControlData> _playerControlPool;
        private readonly EcsPoolInject<ComponentRef<CharacterView>> _characterRefPool;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
                UpdateMove(entity);
        }

        private void UpdateMove(int entity)
        {
            ref var characterView = ref _characterRefPool.Value.Get(entity).Component;
            ref var characterData = ref _characterControlPool.Value.Get(entity);

            var characterInputs = new PlayerCharacterInputs
            {
                MoveAxisForward = characterData.DirectionMove.y,
                MoveAxisRight = characterData.DirectionMove.x,
                JumpDown = characterData.IsJump,
                CameraRotation = characterData.CameraRotation,
            };

            characterView.SetInputs(ref characterInputs);
        }
    }
}