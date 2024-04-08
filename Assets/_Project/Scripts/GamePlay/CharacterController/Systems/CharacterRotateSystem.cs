using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnrealTeam.SB.Common.Ecs;
using UnrealTeam.SB.GamePlay.CharacterController.Components;
using UnrealTeam.SB.GamePlay.CharacterController.Views;
using UnrealTeam.SB.Services.InputControl.Interfaces;
using PlayerTag = UnrealTeam.SB.GamePlay.CharacterController.Components.PlayerTag;

namespace UnrealTeam.SB.GamePlay.CharacterController.Systems
{
    public class CharacterRotateSystem : IEcsRunSystem
    {
        private readonly IInputService _inputService;

        private readonly EcsFilterInject<Inc<PlayerTag>> _filter;

        private readonly EcsPoolInject<CharacterData> _characterDataPool;

        private readonly EcsPoolInject<ComponentRef<CharacterView>> _characterRefPool;
        private readonly EcsPoolInject<ComponentRef<CameraView>> _cameraRefPool;

        public CharacterRotateSystem(IInputService inputService)
        {
            _inputService = inputService;
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                UpdateRotate(entity);
            }
        }

        private void UpdateRotate(int entity)
        {
            ref var cameraView = ref _cameraRefPool.Value.Get(entity).Component;

            cameraView.UpdateRotateAndPosition(Time.deltaTime,
                _inputService.GameInput.Player.Look.ReadValue<Vector2>() * Time.deltaTime);
        }
    }
}