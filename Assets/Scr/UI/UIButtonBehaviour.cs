using R3;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Scr.UI {
    [RequireComponent(typeof(Button))]
    public abstract class UIButtonBehaviour<T> : UIBehaviour {
        
        protected Button _button;
        
        protected T _callBackParam;
        
        protected Subject<T> _onClickSubject = new Subject<T>();
        
        public Observable<T> OnClick => _onClickSubject;
        
        [Title("ButtonUIConfig")]
        
        [SerializeField]
        [LabelText("自動可視化")]
        private bool _autoVisualize = true;

        protected override void OnAwake() {
            base.OnAwake();
            _button = GetComponent<Button>();
        }

        private void Start() {
            //ボタンの購読はStartで行う
            _button
                .onClick
                .AsObservable()
                .Subscribe(_ => {
                    _onClickSubject.OnNext(_callBackParam);
                    _onClickSubject.OnCompleted();
                })
                .AddTo(this);

            if (_autoVisualize) {
                Activate();
            }
        }

        public void Activate() {
            _isOpen = true;
            ChangeCanvasState(true, true);
        }

        public void Deactivate() {
            _isOpen = false;
            ChangeCanvasState(false, false);
        }
        
    }
}