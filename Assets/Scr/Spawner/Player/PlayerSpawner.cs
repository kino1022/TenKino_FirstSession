using Fusion;
using Scr.Utility;
using Sirenix.Serialization;
using UnityEngine;
using VContainer.Unity;

namespace Scr.Spawner {
    
    /// <summary>
    /// プレイヤーを生成するスポナーに対して約束するインターフェース
    /// </summary>
    public interface IPlayerSpawner : ISpawner {

        /// <summary>
        /// スポーンさせるプレイヤーの参照を受け取る
        /// </summary>
        /// <param name="playerRef"></param>
        void SetPlayer(PlayerRef playerRef);
        
        /// <summary>
        /// スポーンさせるオブジェクトを設定する
        /// </summary>
        /// <param name="prefab"></param>
        void SetPrefab(NetworkObject prefab);
        
    }
    
    public class PlayerSpawner : ASpawner {
        
        [OdinSerialize]
        private PlayerRef _playerRef;
        
        private NetworkObject _playerPrefab;

        public void SetPlayer(PlayerRef playerRef) {
            
            _playerRef = playerRef;
            
        }

        public void SetPrefab(NetworkObject prefab) {
            if (prefab == null) {
                Debug.Log("プレイヤープレハブが存在しませんでした");
                return;
            }
            
            _playerPrefab = prefab;
        }

        public override NetworkObject Spawn() {

            if (_playerPrefab is null) {
                Debug.Log("プレイヤープレハブが指定されていない状態でスポーンが呼ばれました");
                return null;
            } 
            
            return SpawnAnyPrefab(_playerPrefab);
        }

        public override NetworkObject SpawnAnyPrefab(NetworkObject prefab) {
            
            if (_playerRef == default(PlayerRef)) {
                Debug.Log("プレイヤ参照が指定されていない状態でスポーンが呼ばれました");
                return null;
            } 

            var pos = _posProvider is not null ? _posProvider.Provider(_resolver, gameObject) : gameObject.transform.position;
            
            var rot = _rotProvider is not null ? _rotProvider.Provider(_resolver, gameObject) : gameObject.transform.rotation;

            var instance = _runner.Spawn(
                prefab,
                pos,
                rot,
                _playerRef
            );
            
            _runner.SetPlayerObject(_playerRef, instance);
            
            var constructables = instance.gameObject.GetComponentsFromWhole<IConstructable>();

            if (constructables.Length is 0) {
                return instance;
            }

            var instanceScope = instance.gameObject.GetComponentFromWhole<LifetimeScope>().Container;
            
            instanceScope ??= _resolver;
            
            foreach (var constructable in constructables) {
                constructable.Construct(instanceScope);
            }
            
            _playerPrefab = default;
            
            _playerRef = default;

            return instance;
        }
    }
}