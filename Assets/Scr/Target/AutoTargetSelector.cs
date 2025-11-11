using System;
using R3;
using RinaSymbol;
using RinaSymbol.Group;
using Scr.Utility;
using VContainer;
using VContainer.Unity;

namespace Scr.Target {
    [Serializable]
    public class AutoTargetSelector  : IStartable, IDisposable {

        private ITargetManager _target;

        private IGroupTag _selfGroup;

        private IGroupEntitiesProvider _entities;

        private ASymbol _self;
        
        private readonly IObjectResolver _resolver;
        
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        public AutoTargetSelector(IObjectResolver resolver) {
            _resolver = resolver;
        }

        public void Start() {
            
            _target = _resolver.Resolve<ITargetManager>();
            
            _self = _resolver.Resolve<ASymbol>();
            
            _selfGroup = _resolver.Resolve<IGroupTag>();
            
            RegisterEmptyTarget();
        }

        public void Dispose() {
            _disposables.Dispose();
            _disposables.Clear();
        }

        private void RegisterEmptyTarget() {
            
            _target ??= _resolver.Resolve<ITargetManager>();
            
            _selfGroup ??= _resolver.Resolve<IGroupTag>();

            _target
                .Provide
                .Where(x => x == null)
                .Subscribe(_ => {
                    _entities ??= _resolver.Resolve<IGroupEntitiesProvider>();
                    try {
                        var next = _entities.GetNearDifferentGroup(_self, _selfGroup);
                        if (next is null) {
                            return;
                        }
                        _target.ChangeTarget(next);
                    }
                    catch { }
                })
                .AddTo(_disposables);
            
        }
    }
}