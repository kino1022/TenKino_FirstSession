using RinaCorrection;
using UnityEngine;
using VContainer;

namespace Scr.Player.Move.Fall {

    public interface IFallMovementManager : IMovementManager {
        
        ICorrectionManager Correction { get; }
        
    }
    
    public class FreeFallMovementManager : MovementBehaviour , IFallMovementManager {

        private IGroundedProvider _grounded;
        
        private ICorrectionManager _correction;
        
        public ICorrectionManager Correction => _correction;

        private void Start() {
            
            _correction = _resolver.Resolve<ICorrectionManager>();
            
            _grounded = _resolver.Resolve<IGroundedProvider>();
            
        }

        private void Update() {
            _movement = CalculateGravity();
        }

        private Vector3 CalculateGravity() {
            
            _grounded ??= _resolver.Resolve<IGroundedProvider>();
            
            var result = Vector3.zero;

            var isGrounded = _grounded.IsGrounded.CurrentValue;
            
            if (!isGrounded) {
                return result;
            }

            var movement = Physics.gravity.y;
            movement = _correction is not null ? _correction.Apply(movement) : movement;
            return new Vector3(0.0f, movement, 0.0f);
        }
    }
}