using System;
using System.IO;

namespace Scr.Status.Health {
    
    [Serializable]
    public struct Damage {

        public int Value { get; private set; }
        
        public Damage(int value) {
            
            if (value < 0) { throw new ArgumentOutOfRangeException("value"); }
            
            Value = value;
            
        }
        
        public static byte[] Serialize(object obj) {
            var damage = (Damage)obj;
            
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write(damage.Value);
            }
            return ms.ToArray();
        }
        
        public static object Deserialize(byte[] data) {
            MemoryStream ms = new MemoryStream(data);
            using (BinaryReader reader = new BinaryReader(ms))
            {
                int value = reader.ReadInt32();
                return new Damage(value);
            }
        }
        
    }
    
}