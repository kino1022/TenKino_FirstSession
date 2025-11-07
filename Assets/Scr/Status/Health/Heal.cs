using System;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Scr.Status.Health {
    [Serializable]
    public class Heal {

        [SerializeField]
        [LabelText("回復量")]
        private int m_value;
        
        public int Value => m_value;

        public static byte[] Serialize(object obj) {
            var heal = (Heal)obj;
            
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms)) {
                writer.Write(heal.Value);
            }
            return ms.ToArray();
        }
        
        public static Heal Deserialize(byte[] data) {
            MemoryStream ms = new MemoryStream(data);
            using (BinaryReader reader = new BinaryReader(ms)) {
                int value = reader.ReadInt32();
                return new Heal {
                    m_value = value
                };
            }
        }
    }
}