using Fusion;
using R3;

namespace Scr.Player {
    
    /// <summary>
    /// NetworkBehaviourからしかアクセスできない情報を外部クラスに対して提供するインターフェース
    /// </summary>
    public interface INetworkPropertyProvider {
        
        ReadOnlyReactiveProperty<bool> InputAuthority { get; }
        
        ReadOnlyReactiveProperty<bool> StateAuthority { get; }
        
    }
    
    public class NetworkPropertyProvider : NetworkBehaviour, INetworkPropertyProvider {
        
        private ReactiveProperty<bool> _inputAuthority = new ReactiveProperty<bool>(false);
        
        private ReactiveProperty<bool> _stateAuthority = new ReactiveProperty<bool>(false);
        
        public ReadOnlyReactiveProperty<bool> InputAuthority => _inputAuthority;
        
        public ReadOnlyReactiveProperty<bool> StateAuthority => _stateAuthority;
        
        private void Update() {
            
            _inputAuthority.Value = HasInputAuthority;
            
            _stateAuthority.Value = HasStateAuthority;
            
        }
    }
}