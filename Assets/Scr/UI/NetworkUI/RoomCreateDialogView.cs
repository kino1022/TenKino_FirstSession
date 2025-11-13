using R3;
using Scr.Network;
using Scr.Utility;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;
using Button = UnityEngine.UI.Button;

namespace Scr.UI.NetworkUI {
    public class RoomCreateDialogView : UIDialogViewBehaviour, IConstructable {

        private ReactiveProperty<int> _currentMember = new(1);
        
        private RoomMatchMaker _matchMaker;
        
        private Subject<RoomProperty> _enterSubject = new();

        private IObjectResolver _resolver;
        
        public Observable<RoomProperty> EnterObservable => _enterSubject;
        
        [SerializeField]
        [LabelText("ルーム名入力フィールド")]
        private TextField _roomNameInputField;
        
        [SerializeField]
        [LabelText("メンバー数表示テキスト")]
        private TMP_Text _currentMemberCountText;
        
        [SerializeField]
        [LabelText("メンバー数増加")]
        private Button _increaseMemberButton;
        
        [SerializeField]
        [LabelText("メンバー数減少")]
        private Button _decreaseMemberButton;
        
        [SerializeField]
        [LabelText("確定ボタン")]
        private Button _enterButton;
        

        [Inject]
        public void Construct(IObjectResolver resolver) {
            _resolver = resolver;
        }

        protected override void Awake() {
            base.Awake();

            _enterButton?
                .OnClickAsObservable()
                .Subscribe(_ => {

                })
                .AddTo(this);

            _increaseMemberButton
                ?.OnClickAsObservable()
                .Subscribe(_ => {
                    _currentMember.Value++;
                })
                .AddTo(this);

            _decreaseMemberButton
                ?.OnClickAsObservable()
                .Subscribe(_ => {
                    if (_currentMember.CurrentValue < 1) {
                        return;
                    }

                    _currentMember.Value++;
                })
                .AddTo(this);
        }

        private void Start() {
            _matchMaker = _resolver.Resolve<RoomMatchMaker>() ?? gameObject.AddComponent<RoomMatchMaker>();
            
        }

        private (bool success, RoomProperty prop) CreateRoomProperty() {
            //あとでルーム名の最大長を宣言してもいいかもしれない
            if (_roomNameInputField.value.Length <= 0) {
                return (false, default);
            }

            var result = new RoomProperty() {
                RoomName = _roomNameInputField.text,
                PlayerCount = _currentMember.CurrentValue
            };
            
            return (true, result);
        } 
        
    }
}