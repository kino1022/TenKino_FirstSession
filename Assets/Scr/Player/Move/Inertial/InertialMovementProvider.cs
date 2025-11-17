using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scr.Player.Move.Inertial {

    public interface IInertialMovementProvider : IMovementProvider {
        
        void AddInertial(IInertial inertial);
        
        void RemoveInertial(IInertial inertial);
        
        void ClearInertial();
        
    }
    
    public class InertialMovementProvider : MovementBehaviour {

        private List<IInertial> _inertials = new();
        
        private float _threshold = 0.01f;

        protected void FixedUpdate() {
            
            RemoveInertial(_threshold);
            
            _movement = CalculateVelocity();
            
        }


        public void AddInertial(IInertial inertial) {
            
            if (inertial is null) throw new ArgumentNullException();

            if (inertial.Velocity.magnitude > _threshold) {
                return;
            }
            
            _inertials.Add(inertial);
            
            inertial.Start();
        }

        public void RemoveInertial(IInertial inertial) {
            
            if (inertial is null) throw new ArgumentNullException();
            
            if (_inertials.Contains(inertial) is false) {
                return;
            }
            
            inertial.Dispose();
            _inertials.Remove(inertial);
        }

        public void ClearInertial() {

            if (_inertials.Count is 0) return;
            
            foreach (var inertial in _inertials) {
                RemoveInertial(inertial);
            }
        }

        private Vector3 CalculateVelocity() {
            if (_inertials.Count == 0) {
                return Vector3.zero;
            }

            var result = Vector3.zero;
            
            _inertials.ForEach(x => {
                result += x.Velocity;
            });
            
            return result;
        }
        
        private void RemoveInertial (float threshold) {
            var target = _inertials.FindAll(x => x.Velocity.magnitude < threshold);

            foreach (var inertial in target) {
                RemoveInertial(inertial);
            }
        }
    }
}