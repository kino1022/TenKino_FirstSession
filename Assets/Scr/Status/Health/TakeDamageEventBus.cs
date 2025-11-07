using System;
using System.IO;

namespace Scr.Status.Health {
    
    [Serializable]
    public class TakeDamageEventBus {
        
        public int TargetId { get; private set; }
        
        public Damage Damage { get; private set; }
        
        public TakeDamageEventBus(int targetId, Damage damage) {

            TargetId = targetId;
            
            Damage = damage;
            
        }
        
        public static byte[] Serialize(object obj) {
            var eventBus = (TakeDamageEventBus)obj;
            
            byte[] damageData = Damage.Serialize(eventBus.Damage);
            
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write(eventBus.TargetId);
                
                writer.Write(damageData.Length);
                
                writer.Write(damageData);
            }
            return ms.ToArray();
        }
        
        public static TakeDamageEventBus Deserialize(byte[] data) {
            MemoryStream ms = new MemoryStream(data);
            using (BinaryReader reader = new BinaryReader(ms))
            {
                int targetId = reader.ReadInt32();
                
                int damageDataLength = reader.ReadInt32();
                
                byte[] damageData = reader.ReadBytes(damageDataLength);
                
                Damage damage = (Damage)Damage.Deserialize(damageData);
                
                return new TakeDamageEventBus(targetId, damage);
            }
        }
    }
    
}