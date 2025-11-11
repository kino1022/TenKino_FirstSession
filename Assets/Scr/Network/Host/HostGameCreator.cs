using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Fusion;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;

namespace Scr.Network.Host {
    
    
    public class HostGameCreator : SerializedMonoBehaviour {
        
        private string _roomName = "UnknownRoom";

        private int _mapName = 1;
        
        private int _memberLimit = 4;

        private NetworkRunner _runner;

        private IObjectResolver _resolver;
        
        public async UniTask CreateRoomAsync() {
            
            _runner ??= _resolver.Resolve<NetworkRunner>();

            var args = new StartGameArgs() {
                GameMode = GameMode.Host,
                SessionName = _roomName,
                Scene = SceneRef.FromIndex(_mapName),
                PlayerCount = _memberLimit
            };
            
            StartGameResult result = await _runner.StartGame(args);

            if (result.Ok) {
                Debug.Log("ホストとして部屋の作成に成功しました");
            }
            else {
                Debug.LogError("部屋の作成に失敗しました");
            }
        }
    }
}