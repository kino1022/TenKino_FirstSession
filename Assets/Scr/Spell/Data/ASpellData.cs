using Scr.Spell.Info;
using UnityEngine;

namespace Scr.Spell.Data {

    public interface ISpellData {
        
        /// <summary>
        /// スペルの情報
        /// </summary>
        ISpellInfo Info { get; }
        
        /// <summary>
        /// スペルのコスト
        /// </summary>
        int Cost { get; }
        
        /// <summary>
        /// 最大の使用回数
        /// </summary>
        int MaxAmount { get; }
        
        /// <summary>
        /// 初期の仕様回数
        /// </summary>
        int InitialAmount { get; }
        
    }
    
    public class ASpellData {
        
    }
}