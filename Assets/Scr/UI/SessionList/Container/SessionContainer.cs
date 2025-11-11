using Fusion;
using Sirenix.OdinInspector;

namespace Scr.UI.SessionList.Container {

    public interface ISessionContainer {
        void Initialize(SessionInfo info);
    }
    
    public class SessionContainer : SerializedMonoBehaviour, ISessionContainer {
        
        public void Initialize(SessionInfo info) {
            
        }
    }
}