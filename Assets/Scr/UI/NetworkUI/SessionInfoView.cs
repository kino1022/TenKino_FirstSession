using Cysharp.Threading.Tasks;
using Fusion;
using Scr.Network;
using Scr.Utility;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Scr.UI.NetworkUI {
    public class SessionInfoView : SerializedMonoBehaviour, IConstructable {

        private SessionInfo _info;
        
        private RoomMatchMaker _matchMaker;
        
        private IObjectResolver _resolver;
        
        [SerializeField]
        [LabelText("ルーム参加ボタン")]
        private Button _joinRoomButton;
        
        [SerializeField]
        [LabelText("ルーム名テキスト")]
        private TMP_Text _roomNameText;
        
        [SerializeField]
        [LabelText("メンバー数テキスト")]
        private TMP_Text _memberCountText;

        [SerializeField]
        [LabelText("エラーメッセージウィンドウ")]
        private UIDialogViewBehaviour _errorWindow;
        
        [Inject]
        public void Construct(IObjectResolver resolver) {
            _resolver = resolver;
        }

        private void Awake() {
            
            Debug.Assert(_joinRoomButton is null, "ルーム参加ボタンのアタッチがありませんでした");

            Debug.Assert(_memberCountText is null, "メンバ数表示テキストのアタッチがありませんでした");
            
            Debug.Assert(_roomNameText is null, "ルーム名表示テキストのアタッチがありませんでした");
            
        }

        private void Start() {
            _matchMaker = _resolver.Resolve<RoomMatchMaker>();
            
            _joinRoomButton.onClick.AddListener(() => {
                _matchMaker.JoinAnyRoom(_info).Forget();
                _matchMaker.OnFailedToJoinRoom += () => {
                    
                };
            });
        }
        

        public void Initialize(SessionInfo info) {
            
            _info = info;
            
            _roomNameText.text = _info.Name;

            _memberCountText.text = GetMemberCountText();
            
        }
        
        private string GetMemberCountText() {
            return $"{_info.PlayerCount} / {_info.MaxPlayers}";
        }
        
        
    }
}