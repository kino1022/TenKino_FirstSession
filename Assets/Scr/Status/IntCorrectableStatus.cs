using System;
using System.Collections.Generic;
using Fusion;
using RinaCorrection;
using Scr.Status.Core;
using Scr.Utility;
using VContainer;

namespace Scr.Status { 
    public interface ICorrectableStatus<T> : IStatus<T>, ICorrectableStatusCore<T> where T : struct {
    
    }

    public class IntCorrectableStatus : NetworkBehaviour, ICorrectableStatus<int>, IConstructable {

        private ICorrectableStatusCore<int> _core;
        
        private IObjectResolver _resolver;

        [Networked]
        public int Value {
            get => _core.Value;
            private set { }
        }

        public List<Func<int, int>> PreSetFunctions => _core.PreSetFunctions;
        
        public List<Action<int>> PostSetActions => _core.PostSetActions;

        [Inject]
        public void Construct(IObjectResolver resolver) {
            _resolver = resolver;
        }

        private void Start() {
            _core = _resolver.Resolve<ICorrectableStatusCore<int>>();
        }
        
        public void Set(int next) => _core.Set(next);
        
        public void Increase(int amount) => _core.Increase(amount);
        
        public void Decrease(int amount) => _core.Decrease(amount);

        public void AddCorrection(ICorrectionValue correction) => _core.AddCorrection(correction);
        
        public void RemoveCorrection(ICorrectionValue correction) => _core.RemoveCorrection(correction);
        
        public void Clear() => _core.Clear();
        
    }
}