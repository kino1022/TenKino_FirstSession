using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Scr.Player.Move.Inertial {

    public interface IInertial : IDisposable {
        
        /// <summary>
        /// 慣性の移動量
        /// </summary>
        Vector3 Velocity { get; }

        void Start();
        
    }
    
    public class Inertial : IInertial {

        private float _force = 1.0f;
        
        private Vector3 _direction = Vector3.forward;

        private float _damping = 0.98f;
        
        private float _threshold = 0.01f;

        private CancellationTokenSource _cts = new();
        
        public Vector3 Velocity => _direction.normalized * _force;

        public Inertial(float force, Vector3 direction, float damping) {

            if (force < 0.0f) {
                force = Mathf.Abs(force);
                direction *= -1.0f;
            }

            if (damping > 1.0f) {
                throw new ArgumentOutOfRangeException();
            }
            
            CancellationToken token = _cts.Token;
            
            _force = force;
            _direction = direction;
            _damping = damping;
        }

        public void Start() {
            StartDamping().Forget();
        }

        public void Dispose() {
            _cts.Cancel();
            _cts.Dispose();
        }


        private async UniTask StartDamping() {
            while (_force > _threshold || !_cts.IsCancellationRequested) {
                await UniTask.Yield(cancellationToken: _cts.Token);
                _force *= _damping * Time.deltaTime;
            }
        }
    }
}