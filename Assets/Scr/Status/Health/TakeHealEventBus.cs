using System.IO;

namespace Scr.Status.Health {
    public readonly struct TakeHealEventBus {

        private readonly int m_targetId;

        private readonly Heal m_heal;

        public TakeHealEventBus(int id, Heal heal) {
            
            m_targetId = id;
            
            m_heal = heal;
            
        }
        
        public static byte[] Serialize(object obj) {
            var eventBus = (TakeHealEventBus)obj;
            
            byte[] healData = Heal.Serialize(eventBus.m_heal);
            
            using (var ms = new MemoryStream())
            using (var writer = new BinaryWriter(ms)) {
                writer.Write(eventBus.m_targetId);
                
                writer.Write(healData.Length);
                
                writer.Write(healData);
                
                return ms.ToArray();
            }
        }
        
        public static TakeHealEventBus Deserialize(byte[] data) {
            using (var ms = new MemoryStream(data))
            using (var reader = new BinaryReader(ms)) {
                int targetId = reader.ReadInt32();
                
                int healDataLength = reader.ReadInt32();
                
                byte[] healData = reader.ReadBytes(healDataLength);
                
                Heal heal = (Heal)Heal.Deserialize(healData);
                
                return new TakeHealEventBus(targetId, heal);
            }
        }
    }
}