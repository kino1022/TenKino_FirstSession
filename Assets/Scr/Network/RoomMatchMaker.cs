using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Fusion;
using Fusion.Sockets;
using Scr.AppManager;
using Scr.Utility;
using Sirenix.OdinInspector;
using VContainer;

namespace Scr.Network {
    public class RoomMatchMaker : SerializedMonoBehaviour, INetworkRunnerCallbacks, IConstructable {

        private NetworkRunner _runner;
        
        private IRunnerProvider _runnerProvider;
        
        private IObjectResolver _resolver;
        
        /// <summary>
        /// ロビーへの参加に成功した際に呼び出されるイベント
        /// </summary>
        public event Action OnJoinedLobby;
        
        /// <summary>
        /// ロビーへの参加に失敗した際に呼び出されるイベント
        /// </summary>
        public event Action<ShutdownReason> OnFailedToJoinLobby;
        
        /// <summary>
        /// ホストとしてルームの生成に成功した際に呼び出されるイベント
        /// </summary>
        public event Action OnCreatedRoom;
        
        /// <summary>
        /// ホストとしてルームの生成に失敗した際に呼び出されるイベント
        /// </summary>
        public event Action OnFailedToCreateRoom;
        
        /// <summary>
        /// クライアントとしてルームへの参加が成功した際に呼び出されるイベント
        /// </summary>
        public event Action OnJoinedRoom;
        
        /// <summary>
        /// クライアントとしてルームへの参加に失敗した際に呼び出されるイベント
        /// </summary>
        public event Action OnFailedToJoinRoom;

        [Inject]
        public void Construct(IObjectResolver resolver) {
            _resolver = resolver;
        }

        private void Start() {
            _runnerProvider = _resolver.Resolve<IRunnerProvider>();
            
            CleanupRunner();
        }


        /// <summary>
        /// セッションブラウザの開始とロビーへの接続を行う
        /// </summary>
        public async UniTask StartSessionBrowser() {
            
            CleanupRunner();

            //ロビーへの参加処理
            var result = await _runner.JoinSessionLobby(SessionLobby.ClientServer);

            if (result.Ok) {
                OnJoinedLobby?.Invoke();
            }
            else {
                OnFailedToJoinLobby?.Invoke(result.ShutdownReason);
                CleanupRunner();
            }
            
        }

        /// <summary>
        /// クライアントとしてルームに参加するための処理
        /// </summary>
        /// <param name="info"></param>
        public async UniTask JoinAnyRoom(SessionInfo info) {

            var appManager = _runner.gameObject;
            
            if (_runner is not null) {
                await _runner.Shutdown(true);
                _runner = null;
            }

            var newRunner = appManager.GetComponent<NetworkRunner>() ?? appManager.gameObject.AddComponent<NetworkRunner>();
            
            _runnerProvider.SetRunner(newRunner);
            _runner = _runnerProvider.Provide();
            _runner.AddCallbacks(this);

            var result = new StartGameArgs() {
                GameMode = GameMode.Client,
                SessionName = info.Name
            };
            
            var joinResult = await _runner.StartGame(result);

            if (joinResult.Ok) {
                OnJoinedRoom?.Invoke();
            }
            else {
                OnFailedToJoinRoom?.Invoke();
                CleanupRunner();
            }
            
        }

        public async UniTask CreateGameRoom(RoomProperty roomProp,Dictionary<string, SessionProperty> props) {
            
            CleanupRunner();

            var result = await _runner.StartGame(new StartGameArgs {
                GameMode = GameMode.Host,
                SessionName = roomProp.RoomName,
                PlayerCount = roomProp.PlayerCount,
                SessionProperties = props,
                Scene = SceneRef.FromIndex(roomProp.SceneIndex),
            });

            if (result.Ok) {
                OnCreatedRoom?.Invoke();
            }
            else {
                OnFailedToCreateRoom?.Invoke();
                CleanupRunner();
            }

        }

        /// <summary>
        /// Runnerの破棄と生成並びに再取得を行う
        /// </summary>
        private void CleanupRunner() {
            
            if (_runner is null || _runnerProvider.Provide() is null) return;
            
            _runner = _runnerProvider.Provide();
            
            _runner.RemoveCallbacks(this);
            
            var runnerHolder = _runner.gameObject;
            
            _runner = null;
            
            var newRunner = runnerHolder.AddComponent<NetworkRunner>();
            
            _runnerProvider.SetRunner(newRunner);

            _runner = _runnerProvider.Provide();

            _runner.AddCallbacks(this);
            
        }
        
        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) {
            
        }
        
        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) {
            
        }
        
        public void OnConnectedToServer(NetworkRunner runner) {
            throw new NotImplementedException();
        }
        
        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) {
            throw new NotImplementedException();
        }

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) {
            throw new NotImplementedException();
        }

        public void OnSceneLoadDone(NetworkRunner runner) {
            throw new NotImplementedException();
        }

        public void OnSceneLoadStart(NetworkRunner runner) {
            throw new NotImplementedException();
        }


        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) {
            throw new NotImplementedException();
        }

        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) {
            throw new NotImplementedException();
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) {
            throw new NotImplementedException();
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) {
            throw new NotImplementedException();
        }
        
        public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) {
            throw new NotImplementedException();
        }

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) {
            throw new NotImplementedException();
        }

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) {
            throw new NotImplementedException();
        }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) {
            throw new NotImplementedException();
        }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) {
            throw new NotImplementedException();
        }

        public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) {
            throw new NotImplementedException();
        }

        public void OnInput(NetworkRunner runner, NetworkInput input) {
            throw new NotImplementedException();
        }

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) {
            throw new NotImplementedException();
        }
    }
}