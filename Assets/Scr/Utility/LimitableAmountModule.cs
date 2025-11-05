namespace Scr.Utility {
    
    /// <summary>
    /// 最大回数を定義可能な使用回数カウントモジュールのインターフェース
    /// </summary>
    public interface ILimitableAmountModule {
        
        /// <summary>
        /// 最大の使用回数
        /// </summary>
        int Max { get; }
        
        /// <summary>
        /// 最大回数を設定する
        /// </summary>
        /// <param name="max"></param>
        void SetMax(int max);
        
    }
    
    public class LimitableAmountModule {
        
    }
}