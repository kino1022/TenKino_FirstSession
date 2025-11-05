using R3;

namespace Scr.Utility {

    /// <summary>
    /// 使用回数をカウントするクラスに対して約束するインターフェース
    /// </summary>
    public interface IAmountModule {
        
        /// <summary>
        /// 残りの回数
        /// </summary>
        ReadOnlyReactiveProperty<int> Amount { get; }

        /// <summary>
        /// 回数を設定する
        /// </summary>
        /// <param name="next"></param>
        void Set(int next);
        
        /// <summary>
        /// 回数を増やす
        /// </summary>
        /// <param name="value"></param>
        void Increase (int value);
        
        /// <summary>
        /// 回数を減らす
        /// </summary>
        /// <param name="value"></param>
        void Decrease (int value);
        
    }
    
    public class AmountModule {
        
    }
}