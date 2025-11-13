using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Scr.UI {
    public abstract class UIDialogViewBehaviour : SerializedMonoBehaviour {

        private ReactiveProperty<bool> _isOpen = new();
        
        
        [Title("ダイアログ設定")]
        
        protected CanvasGroup _canvasGroup;
        
        [SerializeField]
        [LabelText("トランスジョン時間")]
        [ProgressBar(0.0f, 1.0f)]
        private float _transitionDuration = 0.25f;
        
        [SerializeField]
        [LabelText("閉じる")]
        private Button _closeButton;
        
        private readonly Subject<Unit> _closeSubject = new Subject<Unit>();
        
        public ReadOnlyReactiveProperty<bool> IsOpen => _isOpen.ToReadOnlyReactiveProperty();
        
        public Observable<Unit> OnCloseRequested => _closeSubject;

        protected virtual void Awake() {
            _canvasGroup = GetComponent<CanvasGroup>();
            
            _canvasGroup ??= gameObject.AddComponent<CanvasGroup>();
            
            _closeButton?.OnClickAsObservable()
                .Subscribe(_ => {
                    _closeSubject.OnNext(Unit.Default);
                })
                .AddTo(this);

            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            
        }

        public async UniTask Open_Async() {

            if (_isOpen.CurrentValue is true) {
                return;
            }

            _isOpen.Value = true;
            
            OnWillOpen();
            
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
            
            await FadeAsync(1f, _transitionDuration, this.GetCancellationTokenOnDestroy());

            OnDidOpen();
        }
        
        public async UniTask Close_Async() {

            if (_isOpen.CurrentValue is false) {
                return;
            }

            _isOpen.Value = false;
            
            OnWillClose();
            
            _canvasGroup.interactable = false;
            
            await FadeAsync(0f, _transitionDuration, this.GetCancellationTokenOnDestroy());

            _canvasGroup.blocksRaycasts = false;
            
            OnDidClose();
        }
        
        private async UniTask FadeAsync(float targetAlpha, float duration, CancellationToken ct)
        {
            float startAlpha = _canvasGroup.alpha;
            float timer = 0f;

            while (timer < duration)
            {
                // CancellationTokenがキャンセルされたら即座に終了
                if (ct.IsCancellationRequested)
                {
                    _canvasGroup.alpha = startAlpha; // 中途半端な状態を戻す（設計による）
                    return;
                }

                timer += Time.deltaTime;
                float t = Mathf.Clamp01(timer / duration);
                _canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            
                await UniTask.Yield(PlayerLoopTiming.Update, ct);
            }

            _canvasGroup.alpha = targetAlpha; // 確実にターゲット値にする
        }
        
        protected virtual void OnWillOpen () {}
        
        protected virtual void OnDidOpen () {}
        
        protected virtual void OnWillClose () {}
        
        protected virtual void OnDidClose () {}
        
        
    }
}