using Scr.Spell.Info;
using Scr.Utility;

namespace Scr.Spell.Instance {

    /// <summary>
    /// ゲーム中で扱われるスペルのインスタンスに対して約束するインターフェース
    /// </summary>
    public interface ISpellInstance {
        
        /// <summary>
        /// スペルの除方
        /// </summary>
        ISpellInfo Info { get; }
        
        /// <summary>
        /// スペルの残り使用回数
        /// </summary>
        ILimitableAmountModule AmountModule { get; }
        
    }
    
    public record ASpellInstance {
        
    }
}