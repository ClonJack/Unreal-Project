using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

namespace UnrealTeam.SB.Services.Network
{
    [RequireComponent(typeof(NetworkRunner))]
    [RequireComponent(typeof(NetworkSceneManagerDefault))]
    public class NetworkStateMachine : SimulationBehaviour, INetworkRunnerCallbacks
    {
        [field: SerializeField, ReadOnly] public NetworkRunner NetworkRunner { get; private set; }
        [field: SerializeField, ReadOnly] public NetworkSceneManagerDefault NetworkScene { get; private set; }

        public event Action<NetworkRunner, PlayerRef> OnPlayerJoin;
        public event Action<NetworkRunner, PlayerRef> OnPlayerLeave;
        public event Action<NetworkRunner, NetworkInput> OnInputPlayer;

        public void Awake()
        {
            NetworkRunner = GetComponent<NetworkRunner>();
            NetworkScene = GetComponent<NetworkSceneManagerDefault>();
        }

        public void OnEnable()
        {
            if (Runner != null)
                Runner.AddCallbacks(this);
        }

        public void OnDisable()
        {
            if (Runner != null)
                Runner.RemoveCallbacks(this);

            OnInputPlayer = null;
            OnPlayerJoin = null;
        }

        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {
        }

        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) 
            => OnPlayerJoin?.Invoke(runner, player);

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) 
            => OnPlayerLeave?.Invoke(runner, player);

        public void OnInput(NetworkRunner runner, NetworkInput input) 
            => OnInputPlayer?.Invoke(runner, input);

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
        {
        }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
        }

        public void OnConnectedToServer(NetworkRunner runner)
        {
        }

        public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
        {
        }

        public void OnDisconnectedFromServer(NetworkRunner runner)
        {
        }

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request,
            byte[] token)
        {
        }

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
        {
        }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
        {
        }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
        }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
        {
        }

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
        }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key,
            ArraySegment<byte> data)
        {
        }

        public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
        {
        }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
        {
        }

        public void OnSceneLoadDone(NetworkRunner runner)
        {
        }

        public void OnSceneLoadStart(NetworkRunner runner)
        {
        }
    }
}