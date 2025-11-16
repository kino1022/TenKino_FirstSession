using Scr.Utility;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;

namespace Scr.Player.Move {

    public interface IMovementManager {
        Vector3 Movement { get; }
    }
    
    public abstract class MovementBehaviour : SerializedMonoBehaviour, IMovementManager, IConstructable{
        
        protected Vector3 _movement = Vector3.zero;

        protected bool _isEnable = true;
        
        protected IObjectResolver _resolver;
        
        public Vector3 Movement {
            get {
                if (_movement.magnitude < _threshold || _isEnable is false) return Vector3.zero;
                
                return _movement;
            }
        }
        
        public bool IsEnable => _isEnable;
        
        
        [Title("設定")]
        
        [SerializeField]
        [LabelText("閾値")]
        protected float _threshold = 0.01f;

        [Inject]
        public void Construct(IObjectResolver resolver) {
            _resolver = resolver;
        }


        public virtual void ChangeEnable(bool enable) {
            OnPreChangeEnable(enable);
            _isEnable = enable;
            OnPostChangeEnable(enable);
        }

        public virtual void ChangeThreshold(float threshold) {
            if (threshold < 0.0f) return;
            _threshold = threshold;
        }
        
        protected virtual void OnPreChangeEnable(bool enable) { }
        
        protected virtual void OnPostChangeEnable(bool enable) { }
        
    }
}