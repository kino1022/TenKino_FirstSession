using Fusion;

namespace Scr.Status.Health {
    
    /// <summary>
    /// 回復を発生させた際に発火されるイベントバス
    /// </summary>
    public interface ITakeHealEventBus : INetworkStruct {
        
        /// <summary>
        /// 回復の対象になるオブジェクトのID
        /// </summary>
        int TargetID { get; }
        
        /// <summary>
        /// 回復量
        /// </summary>
        IHeal Heal { get; }
        
    }
    
    public readonly struct TakeHealEventBus : ITakeHealEventBus {

        private readonly int _targetID;

        private readonly IHeal _heal;
        
        public int TargetID => _targetID;
        
        public IHeal Heal => _heal;
        
        public TakeHealEventBus (int targetID, IHeal heal) {
            _targetID = targetID;
            _heal = heal;
        }
        
    }
}