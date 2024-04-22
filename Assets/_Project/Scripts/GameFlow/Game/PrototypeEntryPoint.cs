using System;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine.SceneManagement;
using UnrealTeam.SB.Configs.App;
using UnrealTeam.SB.Services.Configs;
using UnrealTeam.SB.Services.Factories;
using UnrealTeam.SB.Services.Network;
using VContainer.Unity;

namespace UnrealTeam.SB.GameFlow.Game
{
    public class PrototypeEntryPoint : IDisposable, ITickable, IFixedTickable, ILateTickable
    {
        private readonly PlayerFactory _playerFactory;
        private readonly NetworkStateMachine _networkStateMachine;
        private readonly IConfigAccess _configAccess;
        private readonly EcsLoop _ecsLoop;

        public PrototypeEntryPoint(
            EcsLoop ecsLoop,
            PlayerFactory playerFactory, 
            NetworkStateMachine networkStateMachine, 
            IConfigAccess configAccess)
        {
            _playerFactory = playerFactory;
            _networkStateMachine = networkStateMachine;
            _configAccess = configAccess;
            _ecsLoop = ecsLoop;
        }

        public async UniTask Execute()
        {
            _ecsLoop.Init();

            var appConfig = _configAccess.GetSingle<AppConfig>();

            await _playerFactory.Create();

            var sceneInfo = new NetworkSceneInfo();
            sceneInfo.AddSceneRef(SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex));
            
            await _networkStateMachine.NetworkRunner.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.Shared,
                SessionName = appConfig.SessionName,
                SceneManager = _networkStateMachine.NetworkScene,
                PlayerCount = appConfig.MaxPlayerCount,
                Scene = sceneInfo,
            });
        }

        public void Tick()
            => _ecsLoop.Tick();

        public void LateTick()
            => _ecsLoop.LateTick();

        public void FixedTick()
            => _ecsLoop.FixedTick();

        public void Dispose()
            => _ecsLoop.Dispose();
    }
}