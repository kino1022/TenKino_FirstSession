using Fusion;
using R3;
using Scr.Status.Extension;
using VContainer;

namespace Scr.Status.Health {

    public interface IHealth : IStatus<int> {
        
    }

    public class Health : IntStatus, IHealth {

        private NetworkRunner _runner;

        private NetworkObject _rootObject;

        private IMaxHealth _maxHealth;

        private CompositeDisposable _disposables = new();

        protected override void Start() {
            base.Start();

            _rootObject = _resolver.Resolve<NetworkObject>();

            _runner = _resolver.Resolve<NetworkRunner>();

            _maxHealth = _resolver.Resolve<IMaxHealth>();
        }

        public override void Spawned() {
            base.Spawned();

            //死亡の検知処理
            RegisterObserveDeath();

            //最大体力を超過しないかの検知処理
            RegisterObserveOverMaxHealth();

        }

        private void RegisterObserveDeath() {
            Observable
                .EveryValueChanged(this, x => x.Value)
                .Where(x => x <= 0)
                .Subscribe(_ => {
                    if (HasStateAuthority) {
                        OnHealthZero();
                    }
                })
                .AddTo(_disposables);
        }

        private void OnHealthZero() {

            if (!HasStateAuthority) {
                return;
            }

            _runner ??= _resolver.Resolve<NetworkRunner>();

            _rootObject ??= _resolver.Resolve<NetworkObject>();
            _runner.Despawn(_rootObject);

        }

        private void RegisterObserveOverMaxHealth() {
            
            _maxHealth ??= _resolver.Resolve<IMaxHealth>();
            
            this
                .ObserveGreaterAnyStatus(_maxHealth)
                .Subscribe(_ => {
                    if (HasStateAuthority) {
                        OnOverMaxHealth();
                    }
                })
                .AddTo(_disposables);
        }
        
        private void OnOverMaxHealth() {

            if (!HasStateAuthority) {
                return;
            }
            
            //本当に体力が最高体力を上回っているかどうかを確認
            if(_maxHealth.Value > Value) {
                return;
            }
            
            Set(_maxHealth.Value);
        }
    }
}