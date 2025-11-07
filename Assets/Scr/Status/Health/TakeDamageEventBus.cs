using Fusion;

namespace Scr.Status.Health {

    /// <summary>
    /// ダメージが与えられた際に発信されるイベント通知に約束するインターフェース
    /// </summary>
    public interface ITakeDamageEventBus : INetworkStruct {
        
        int TargetID { get; }
        
        IDamage Damage { get; }
        
    }
    
    public readonly struct TakeDamageEventBus : ITakeDamageEventBus {

        private readonly int _targetID;

        private readonly IDamage _damage;
        
        public int TargetID => _targetID;
        
        public IDamage Damage => _damage;
        
        public TakeDamageEventBus (int targetID, IDamage damage) {
            _targetID = targetID;
            _damage = damage;
        }
        
    }
}