using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Fusion;
using ObservableCollections;
using R3;
using Scr.Network;
using Scr.Utility;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace Scr.UI.NetworkUI {
    public class MatchMakeWindowView : SerializedMonoBehaviour, IConstructable {
        
        private bool _isOpenedCreateDialog = false;

        private RoomMatchMaker _matchMaker;

        private IObjectResolver _resolver;

        [SerializeField]
        [LabelText("セッション情報スクロール")]
        private ScrollRect _sessionListScroll;
        
        [SerializeField]
        [LabelText("ルーム作成ボタン")]
        private Button _createRoomButton;

        [SerializeField]
        [LabelText("ルーム生成ダイアログ")]
        private RoomCreateDialogView _createDialog;
        
        [SerializeField]
        [LabelText("セッション情報")]
        private SessionInfoView _sessionInfo;
        
        
        [Inject]
        public void Construct(IObjectResolver resolver) {
            _resolver = resolver;
        }

        private void Start() {
            _matchMaker = _resolver.Resolve<RoomMatchMaker>() ?? gameObject.AddComponent<RoomMatchMaker>();

            _matchMaker.StartSessionBrowser().Forget();
            
            RegisterChangeSessions();

            //ルーム作成ボタン押下の購読
            _createRoomButton?
                .OnClickAsObservable()
                .Where(_ => !_createDialog.IsOpen.CurrentValue)
                .SubscribeAwait(
                    async (_, ct) => {
                        await _createDialog.Open_Async();
                    },
                    AwaitOperation.Sequential)
                .AddTo(this);

            //ルーム作成ダイアログの閉じる要求の購読処理
            _createDialog
                .OnCloseRequested
                .Where(_ => _createDialog.IsOpen.CurrentValue)
                .SubscribeAwait(
                    async (_, ct) => {
                        await _createDialog.Close_Async();
                    },
                    AwaitOperation.Sequential)
                .AddTo(this);

            //ルーム作成処理の購読処理
            _createDialog
                .EnterObservable
                .SubscribeAwait(
                    async (prop, ct) => {
                        await _matchMaker.CreateGameRoom(prop, new Dictionary<string, SessionProperty>());
                    },
                    AwaitOperation.Sequential
                )
                .AddTo(this);
            
        }

        private void RegisterChangeSessions() {

            _matchMaker
                .Sessions
                .ObserveAdd()
                .Subscribe(_ => OnChangeSessions())
                .AddTo(this);
            
            _matchMaker
                .Sessions
                .ObserveRemove()
                .Subscribe(_ => OnChangeSessions())
                .AddTo(this);
            
            _matchMaker
                .Sessions
                .ObserveClear()
                .Subscribe(_ => OnChangeSessions())
                .AddTo(this);
            
            _matchMaker
                .Sessions
                .ObserveChanged()
                .Subscribe(_ => OnChangeSessions())
                .AddTo(this);
            
        }

        private void OnChangeSessions() {
            
            //既存コンテナの破棄
            foreach (Transform child in _sessionListScroll.content) {
                Destroy(child.gameObject);
            }
            
            //新規コンテナの生成
            foreach (var session in _matchMaker.Sessions) {
                var sessionInfo = _resolver.Instantiate(_sessionInfo, _sessionListScroll.content);
                //初期化
                sessionInfo.Initialize(session);
            }
            
            
        }
        
    }
}