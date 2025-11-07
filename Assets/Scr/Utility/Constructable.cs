using VContainer;

namespace Scr.Utility {
    /// <summary>
    /// lifetimeScopeの外で生成されたオブジェクトに対して、依存性注入を行うためのインターフェース
    /// </summary>
    public interface IConstructable {
        
        void Construct(IObjectResolver resolver);
        
    }
}