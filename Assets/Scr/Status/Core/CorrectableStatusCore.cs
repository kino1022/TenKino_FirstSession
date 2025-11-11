using System;
using Fusion;
using JetBrains.Annotations;
using R3;
using RinaCorrection;
using RinaStatus.Calculator;

namespace Scr.Status.Core {
    
    public interface ICorrectableStatusCore<T> : IStatusCore<T> where T : struct {
        
        void AddCorrection (ICorrectionValue correction);
        
        void RemoveCorrection (ICorrectionValue correction);

        void Clear();
    }
    
    public class CorrectableStatusCore<T> : StatusCore<T> where T : struct {

        private T _corrected;
        
        private readonly ICorrectionManager _corrector;
        
        private readonly CompositeDisposable _disposables = new();

        public CorrectableStatusCore(ICalculator<T> culculator, ICorrectionManager corrector, [CanBeNull] Func<bool> hasAuthority = null) : 
            base(culculator, hasAuthority) {

            _corrector = corrector;

            PostSetActions.Add(_ => _corrected = Apply());
        }
        
        public void AddCorrection(ICorrectionValue correction) => _corrector.Add(correction);
        
        public void RemoveCorrection(ICorrectionValue correction) => _corrector.Remove(correction);
        
        public void Clear() => _corrector.Clear();
        
        private void RegisterCorrectionValueChanged() {
            
            _corrector
                .OnChanged
                .Subscribe(_ => OnCorrectionValueChanged())
                .AddTo(_disposables);
            
        }

        protected override T GetValue() => _corrected;
        
        
        private void OnCorrectionValueChanged() {
            var next = Apply();
            _corrected = next;
        }

        private T Apply() {

            if (_hasAuthority != null && !_hasAuthority()) {
                return _corrected;
            }
            
            dynamic value = _rawValue;

            dynamic correctedValue = _corrector.Apply(value);
            
            return (T)correctedValue;
            
        }
    }
}