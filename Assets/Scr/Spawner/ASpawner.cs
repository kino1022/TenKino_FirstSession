using Fusion;
using Scr.Spawner.Position;
using Scr.Spawner.Rotation;
using Scr.Utility;
using Sirenix.OdinInspector;
using VContainer;

namespace Scr.Spawner {

    /// <summary>
    /// スポナーが持つべき処理
    /// </summary>
    public interface ISpawner {
        
        /// <summary>
        /// スポナーに与えられた生成パターンに基づいてシンボルを生成する
        /// </summary>
        NetworkObject Spawn();

        /// <summary>
        /// 任意のシンボルを生成する
        /// </summary>
        /// <param name="prefab"></param>
        NetworkObject SpawnAnyPrefab(NetworkObject prefab);
        
    }
    
    public abstract class ASpawner : SerializedMonoBehaviour, ISpawner, IConstructable {

        protected IPositionProvider _posProvider;
        
        protected IRotationProvider _rotProvider;

        protected NetworkRunner _runner;

        protected IObjectResolver _resolver;
        
        [Inject]
        public void Construct(IObjectResolver resolver) {
            _resolver = resolver;
        }

        protected virtual void Start() {
            
            _runner = _resolver.Resolve<NetworkRunner>();
            
            _posProvider = _resolver.Resolve<IPositionProvider>();
            
            _rotProvider = _resolver.Resolve<IRotationProvider>();
            
        }
        
        public abstract NetworkObject Spawn();
        
        public abstract NetworkObject SpawnAnyPrefab(NetworkObject prefab);
    }
}