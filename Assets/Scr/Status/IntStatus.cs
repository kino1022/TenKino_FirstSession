using System;
using System.Collections.Generic;
using Fusion;
using Scr.Status.Core;
using VContainer;

namespace Scr.Status {
    
    public interface IStatus<T> : IStatusCore<T> where T : struct {
    
    }
    
    public class IntStatus : NetworkBehaviour , IStatus<int> {
        
        protected IStatusCore<int> _core;

        protected IObjectResolver _resolver;
        
        public List<Func<int, int>> PreSetFunctions => _core.PreSetFunctions;
        
        public List<Action<int>> PostSetActions => _core.PostSetActions;

        [Networked]
        public int Value {
            get => _core.Value;
            private set {}
        }
        
        [Inject]
        public void Construct(IObjectResolver resolver) {
            _resolver = resolver;
        }

        protected virtual void Start() {
            _core = _resolver.Resolve<IStatusCore<int>>();
        }

        public void Set(int next) => _core.Set(next);
        
        public void Increase(int amount) => _core.Increase(amount);
        
        public void Decrease(int amount) => _core.Decrease(amount);
        
    }
}