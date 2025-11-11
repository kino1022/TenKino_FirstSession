using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using Scr.UI.SessionList.Container;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Scr.UI.SessionList {
    public class SessionListPresenter : INetworkRunnerCallbacks {
        
        private readonly ISessionListView _view;

        private SessionContainer _prefab;

        private readonly NetworkRunner _runner;

        private readonly IObjectResolver _resolver;

        public void Start() {
            _runner.AddCallbacks(this);
        }
        
        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) {
            //セッションリスト表示の破壊処理
            foreach (var container in _view.Sessions) {
                if (container is MonoBehaviour mono) {
                    GameObject.Destroy(mono);
                }
            }
            
            //セッションリストのクリア処理
            _view.Sessions.Clear();

            for (int i = 0; i < sessionList.Count; i++) {
                
                var info = sessionList[i] ?? throw new ArgumentNullException(nameof(sessionList));

                if (info.IsValid) {
                    continue;
                }

                var instance = _resolver.Instantiate(
                    _prefab
                );
                
                instance.Initialize(info);
                
                _view.Sessions.Add(instance);
                
            }
            
        }

        #region RunnerCallBacks
        
        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) {
        }

        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) {
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) {
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) {
        }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) {
        }

        public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) {
        }

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) {
        }

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) {
        }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) {
        }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) {
        }

        public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) {
        }

        public void OnInput(NetworkRunner runner, NetworkInput input) {
        }

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) {
        }

        public void OnConnectedToServer(NetworkRunner runner) {
        }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) {
        }

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) {
        }

        public void OnSceneLoadDone(NetworkRunner runner) {
        }

        public void OnSceneLoadStart(NetworkRunner runner) {
        }
        
        #endregion
    }
}