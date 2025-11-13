using Cysharp.Threading.Tasks;
using Scr.Network;
using Scr.Utility;
using Sirenix.OdinInspector;
using VContainer;

namespace Scr.UI.NetworkUI {
    public class MatchMakeWindowView : SerializedMonoBehaviour, IConstructable {

        private RoomMatchMaker _matchMaker;

        private IObjectResolver _resolver;

        [Inject]
        public void Construct(IObjectResolver resolver) {
            _resolver = resolver;
        }

        private void Start() {
            _matchMaker = _resolver.Resolve<RoomMatchMaker>() ?? gameObject.AddComponent<RoomMatchMaker>();

            _matchMaker.StartSessionBrowser().Forget();
        }
        
        
        
    }
}