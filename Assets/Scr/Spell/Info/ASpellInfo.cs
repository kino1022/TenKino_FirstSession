using UnityEngine;

namespace Scr.Spell.Info {

    /// <summary>
    /// スペルの情報を管理するクラスに対して約束するインターフェース
    /// </summary>
    public interface ISpellInfo {
        
        /// <summary>
        /// スペルの名前
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// スペルのアイコン
        /// </summary>
        Sprite Icon { get; }
        
    }
    
    public class ASpellInfo {
        
    }
}