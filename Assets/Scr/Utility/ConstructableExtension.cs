using Fusion;
using VContainer;

namespace Scr.Utility {
    
    public interface IConstructable {
        
        void Construct(IObjectResolver resolver);
    }
    
    public static class ConstructableExtension {
        
        /// <summary>
        /// Runner.Spawnを行ったゲームオブジェクト相手にDIの注入を行うメソッド
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="resolver"></param>
        public static void ExecuteConstruct(this NetworkObject obj, IObjectResolver resolver) {

            if (resolver == null) {
                return;
            }
            
            var constructables = obj.GetComponentsInChildren<IConstructable>();

            if (constructables.Length is 0) {
                return;
            }
            
            foreach (var constructable in constructables) {
                constructable.Construct(resolver);
            }
        }
    }
}