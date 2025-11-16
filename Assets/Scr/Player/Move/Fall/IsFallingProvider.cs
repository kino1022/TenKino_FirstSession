using R3;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Scr.Player.Move.Fall {

    public interface IFallingProvider {
        
        ReadOnlyReactiveProperty<bool> IsFalling { get; }
        
        Observable<float> OnGrounded { get; }
        
    }
    
    public class IsFallingProvider : SerializedMonoBehaviour, IFallingProvider {
        
        private Subject<float> _onGrounded = new Subject<float>();
        
        private ReactiveProperty<bool> _isFalling = new ReactiveProperty<bool>(false);
        
        private float _previousHeight = 0.0f;

        private float _fallingHeight = 0.0f;
        
        public ReadOnlyReactiveProperty<bool> IsFalling => _isFalling;
        
        public Observable<float> OnGrounded => _onGrounded;

        private void Update() {
            var currentHeight = transform.position.y;

            //上昇時の処理
            if (currentHeight > _previousHeight) {
                OnUpperHeight(currentHeight);
            }
            //落下時の処理
            else if (currentHeight < _previousHeight) {
                OnFalling(currentHeight);
            }
            //高度停滞時の処理
            else {
                OnGround(currentHeight);
            }
        }
        
        protected virtual void OnUpperHeight(float currentHeight) {
            
            if (_isFalling.CurrentValue is true) {
                _isFalling.Value = false;
            }
            
            _previousHeight = currentHeight;

            _fallingHeight = 0.0f;
            
        }

        protected virtual void OnFalling(float currentHeight) {
            
            if (_isFalling.CurrentValue is false) {
                _isFalling.Value = true;
            }
            
            var delta = _previousHeight - currentHeight;
            
            _fallingHeight += delta;
            
        }

        protected virtual void OnGround(float currentHeight) {
            var delta = _previousHeight - currentHeight;
            _fallingHeight += delta;

            _isFalling.Value = false;

            _onGrounded.OnNext(_fallingHeight);
            _onGrounded.OnCompleted();
            
            _fallingHeight = 0.0f;
        }
        
    }
}