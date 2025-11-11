using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using RinaStatus.Calculator;

namespace Scr.Status.Core {
    
    public interface IStatusCore<T> where T : struct {
        
        T Value { get; }
        
        void Set(T next);
        
        void Increase(T amount);
        
        void Decrease(T amount);
        
        List<Func<T,T>> PreSetFunctions { get; }
        
        List<Action<T>> PostSetActions { get; }
    }
    
    public class StatusCore<T> : IStatusCore<T> where T : struct {

        protected T _rawValue;
        
        public T Value => GetValue();
         
        private ICalculator<T> _calculator;
        
        [CanBeNull] protected readonly Func<bool> _hasAuthority;
        
        private List<Func<T, T>> _preSetFunctions = new();
        
        private List<Action<T>> _postSetActions = new();
        
        private readonly object _locker = new();
        
        public List<Func<T, T>> PreSetFunctions => _preSetFunctions;
        
        public List<Action<T>> PostSetActions => _postSetActions;

        public StatusCore(ICalculator<T> calculator, [CanBeNull] Func<bool> hasAuthority = null) {
            _hasAuthority = hasAuthority;
            _calculator = calculator;
        }

        public void Set(T next) {
            
            if (_hasAuthority != null && !_hasAuthority()) {
                return;
            }
            
            var nextValue = PreValueSet(next);
            
            _rawValue = nextValue;
            
            PostValueSet();
            
        }

        public void Increase(T amount) {
            if (_hasAuthority != null && !_hasAuthority()) {
                return;
            }
            _rawValue = _calculator.Add(_rawValue, amount);
        }

        public void Decrease(T amount) {
            if (_hasAuthority != null && !_hasAuthority()) {
                return;
            }
            _rawValue = _calculator.Subtract(_rawValue, amount);
        }
        
        /// <summary>
        /// 外部から値が読み取られる際に参照されるメソッド(参照先を変える際はbaseを利用しないこと)
        /// </summary>
        /// <returns></returns>
        protected virtual T GetValue() => _rawValue;

        /// <summary>
        /// 値がセットされる直前に呼び出されるメソッド(base.PreValueSetはしないこと)
        /// </summary>
        /// <param name="next"></param>
        /// <returns></returns>
        protected virtual T PreValueSet(T next) {
            T result = next;

            if (_preSetFunctions.Count is 0) {
                return result;
            }

            List<Func<T, T>> copy;
            lock (_locker) {
                copy = new List<Func<T, T>>(_preSetFunctions);
            }

            foreach (var func in copy) {
                result = func(result);
            }
            
            return result;
        }

        /// <summary>
        /// 値がセットされた直後に呼び出されるメソッド
        /// </summary>
        protected virtual void PostValueSet() {
            
            T result = _rawValue;

            List<Action<T>> copy;

            lock (_locker) {
                copy = new List<Action<T>>(_postSetActions);
            }

            foreach (var action in copy) {
                action(result);
            }
            
        }
    }
}