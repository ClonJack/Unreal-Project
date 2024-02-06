using Configs;
using GamePlay.Components;
using Leopotam.EcsLite;
using Services.Configs;
using UnityEngine;

namespace Services.Factories
{
    public class PlayerFactory
    {
        private readonly IConfigAccess _configAccess;
        private readonly EcsWorld _ecsWorld;


        public PlayerFactory(EcsWorld ecsWorld, IConfigAccess configAccess)
        {
            _ecsWorld = ecsWorld;
            _configAccess = configAccess;
        }
        
        public void Create()
        {
            var playerConfig = _configAccess.GetSingle<PlayerConfig>();
            CreatePlayer(playerConfig.Prefab, out GameObject playerObject, out int playerEntity);
            InitRigidbody(playerEntity, playerObject);
            InitTransform(playerEntity, playerObject);
            InitMovement(playerEntity, playerConfig);
            InitTag(playerEntity);
        }

        private void CreatePlayer(GameObject prefab, out GameObject playerObject, out int playerEntity)
        {
            playerObject = Object.Instantiate(prefab);
            playerEntity = _ecsWorld.NewEntity();
        }

        private void InitTransform(int playerEntity, GameObject playerObject) 
            => _ecsWorld.GetPool<ComponentRef<Transform>>().Add(playerEntity).Value = playerObject.transform;

        private void InitRigidbody(int playerEntity, GameObject playerObject) 
            => _ecsWorld.GetPool<ComponentRef<Rigidbody>>().Add(playerEntity).Value = playerObject.GetComponent<Rigidbody>();

        private void InitTag(int playerEntity)
            => _ecsWorld.GetPool<PlayerTag>().Add(playerEntity);
        
        private void InitMovement(int playerEntity, PlayerConfig playerConfig)
        {
            ref var movementData = ref _ecsWorld.GetPool<MovementData>().Add(playerEntity);
            movementData.MoveSpeed = playerConfig.MoveSpeed;
            movementData.RotationSpeed = playerConfig.RotationSpeed;
        }
    }
}