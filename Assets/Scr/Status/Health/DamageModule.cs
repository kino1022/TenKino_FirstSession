using System;
using MessagePipe;
using Photon.Pun;
using VContainer;
using VContainer.Unity;

namespace Scr.Status.Health {

    public interface IDamageable {
        
    }
    
    public class DamageModule : IDamageable, IStartable, IDisposable {

        private int m_selfid;
        
        private IHealth m_health;
        
        private ISubscriber<TakeDamageEventBus> m_subscriber;

        private IDisposable m_subscription;

        private readonly IObjectResolver m_resolver;

        public DamageModule(IObjectResolver resolver) {
            
            m_resolver = resolver;
            
        }

        public void Start() {
            
            m_health = m_resolver.Resolve<IHealth>() ?? throw new ArgumentNullException(nameof(IHealth));
            
            m_subscriber = m_resolver.Resolve<ISubscriber<TakeDamageEventBus>>();
            
            m_subscription = m_subscriber.Subscribe(OnTakeDamage);

            m_selfid = m_resolver.Resolve<PhotonView>().InstantiationId;

        }

        public void Dispose() {
            
            m_subscription?.Dispose();
            m_subscription = null;
            
        }
        
        private void OnTakeDamage(TakeDamageEventBus eventBus) {
            
            if (eventBus.TargetId != m_selfid) {
                return;
            }
            
            m_health.Decrease(eventBus.Damage.Value);
            
        }
    }
}