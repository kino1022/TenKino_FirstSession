using Fusion;

namespace Scr.Status.Health {

    /// <summary>
    /// 回復量を表現するクラスに対して約束するインターフェース
    /// </summary>
    public interface IHeal : INetworkStruct {
        
        /// <summary>
        /// 回復量
        /// </summary>
        int Value { get; }
        
    }
    
    
    public struct Heal : IHeal{

        private readonly int _value;
        
        public int Value => _value;
        
        public Heal (int value) {

            if (value < 0) {
                throw new System.ArgumentNullException();
            }
            
            _value = value;
        }
    }
}