using Sirenix.OdinInspector;
using UnityEngine;

namespace Scr.UI {
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class UIBehaviour : SerializedMonoBehaviour {

        protected CanvasGroup _canvas;

        protected bool _isOpen = false;
        
        public bool IsOpen => _isOpen;

        protected virtual void Awake() {
            //requireComponentで保証されているのでnullチェック不要
            _canvas = GetComponent<CanvasGroup>();        
            
            OnAwake();
            
            ChangeCanvasState(false, false);
        }
        
        protected void ChangeCanvasState (bool interactable, bool blocksRaycasts) {
            _canvas.interactable = interactable;
            _canvas.blocksRaycasts = blocksRaycasts;
        }
        
        protected virtual void OnAwake () { }
        
    }
}