using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnrealTeam.SB.Common.Ecs;
using UnrealTeam.SB.GamePlay.CharacterController.Components;
using UnrealTeam.SB.GamePlay.CharacterController.Views;
using UnrealTeam.SB.Services.Other;

namespace UnrealTeam.SB.GamePlay.CharacterController.Systems
{
    public class CharacterBoundSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PlayerTag, ComponentRef<CharacterView>>> _filter;
        private readonly EcsPoolInject<ComponentRef<CharacterView>> _characterViewPool;
        private readonly Transform _spawnPoint;
        private readonly Transform _playerBound;

        public CharacterBoundSystem(ObjectsProvider objectsProvider)
        {
            _spawnPoint = objectsProvider.SpawnPoint;
            _playerBound = objectsProvider.PlayerBound;
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                var characterView = _characterViewPool.Value.Get(entity).Component;

                if (characterView.transform.position.y <= _playerBound.position.y)
                {
                    characterView.TeleportTo(_spawnPoint);
                }
            }
        }
    }
}