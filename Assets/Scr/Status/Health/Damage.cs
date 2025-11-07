using System;
using Fusion;

namespace Scr.Status.Health {

    public interface IDamage : INetworkStruct {
        
        int Value { get; }
        
    }
    
    [Serializable]
    public struct Damage : IDamage {

        private readonly int _value;
        
        public int Value => _value;
        
        public Damage (int value) {

            if (value < 0) {
                throw new ArgumentNullException();
            }
            
            _value = value;
        }
    }
}