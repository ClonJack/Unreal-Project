using System;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;
using UnrealTeam.SB.Configs.App;
using UnrealTeam.SB.GamePlay.Network;
using UnrealTeam.SB.Services.Configs;
using UnrealTeam.SB.Services.Factories;
using VContainer.Unity;

namespace UnrealTeam.SB.GameFlow
{
    public class PrototypeEntryPoint : IDisposable, ITickable, IFixedTickable, ILateTickable
    {
        private readonly PlayerFactory _playerFactory;
        private readonly NetworkStateMachine _networkStateMachine;
        private readonly IConfigAccess _configAccess;
        private readonly EcsService _ecsService;

        public PrototypeEntryPoint(
            EcsService ecsService,
            PlayerFactory playerFactory, NetworkStateMachine networkStateMachine, IConfigAccess configAccess)
        {
            _playerFactory = playerFactory;
            _networkStateMachine = networkStateMachine;
            _configAccess = configAccess;
            _ecsService = ecsService;
        }

        public async UniTask Execute()
        {
            _ecsService.Init();

            var appConfig = _configAccess.GetSingle<AppConfig>();

            await _playerFactory.Create();

            await _networkStateMachine.NetworkRunner.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.Shared,
                SessionName = appConfig.SessionName,
                SceneManager = _networkStateMachine.NetworkScene,
                PlayerCount = appConfig.MaxPlayerCount
            });
        }

        public void Tick()
            => _ecsService.Tick();

        public void LateTick()
            => _ecsService.LateTick();

        public void FixedTick()
            => _ecsService.FixedTick();

        public void Dispose()
            => _ecsService.Dispose();
    }
}