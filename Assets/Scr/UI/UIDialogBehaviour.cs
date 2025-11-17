using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

namespace Scr.UI {
    public class UIDialogBehaviour : UIBehaviour {
        
        protected UIButtonBehaviour<Unit> _closeButton;
        
        protected Subject<Unit> _closeSubject;
        
        public Observable<Unit> OnCloseRequested => _closeSubject;
        
        protected float _transitionDuration;

        protected override void OnAwake() {
            base.OnAwake();

            _closeButton
                .OnClick
                .Subscribe(_ => {
                    _closeSubject.OnNext(Unit.Default);
                    _closeSubject.OnCompleted();
                    Close_Async().Forget();
                })
                .AddTo(this);
        }

        protected virtual void Start() {
            if (_closeButton.IsOpen is false) {
                _closeButton.Activate();
            }
        }
        
        /// <summary>
        /// ダイアログを開く際に呼び出す非同期処理
        /// </summary>
        public async UniTask Open_Async() {

            if (_isOpen is true) {
                return;
            }

            _isOpen = true;
            
            OnWillOpen();
            
            _canvas.interactable = true;
            _canvas.blocksRaycasts = true;
            
            await FadeAsync(1f, _transitionDuration, this.GetCancellationTokenOnDestroy());

            OnDidOpen();
        }
        
        /// <summary>
        /// ダイアログを閉じる際に呼び出す非同期処理
        /// </summary>
        public async UniTask Close_Async() {

            if (_isOpen is false) {
                return;
            }

            _isOpen = false;
            
            OnWillClose();
            
            _canvas.interactable = false;
            
            await FadeAsync(0f, _transitionDuration, this.GetCancellationTokenOnDestroy());

            _canvas.blocksRaycasts = false;
            
            OnDidClose();
        }
        
        private async UniTask FadeAsync(float targetAlpha, float duration, CancellationToken ct)
        {
            float startAlpha = _canvas.alpha;
            float timer = 0f;

            while (timer < duration)
            {
                // CancellationTokenがキャンセルされたら即座に終了
                if (ct.IsCancellationRequested)
                {
                    _canvas.alpha = startAlpha; // 中途半端な状態を戻す（設計による）
                    return;
                }

                timer += Time.deltaTime;
                float t = Mathf.Clamp01(timer / duration);
                _canvas.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            
                await UniTask.Yield(PlayerLoopTiming.Update, ct);
            }

            _canvas.alpha = targetAlpha; // 確実にターゲット値にする
        }
        
        protected virtual void OnWillOpen() {}
        
        protected virtual void OnDidOpen() {}
        
        protected virtual void OnWillClose () {}
        protected virtual void OnDidClose() {}
    }
}