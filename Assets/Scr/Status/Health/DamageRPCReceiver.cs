using System;
using MessagePipe;
using Photon.Pun;
using Scr.Status.Health;
using Sirenix.OdinInspector;
using VContainer;

namespace Scr.Status.Health {
    /// <summary>
    /// 
    /// </summary>
    public class HealthRPCReceiver : MonoBehaviourPunCallbacks {

        private IPublisher<TakeDamageEventBus> m_takeDamagePublisher;
        
        private IPublisher<TakeHealEventBus> m_takeHealPublisher;
        
        private IObjectResolver m_resolver;

        [Inject]
        public void Construct(IObjectResolver resolver) {
            m_resolver = resolver;
        }
        private void Start() {
            
            m_takeDamagePublisher = m_resolver.Resolve<IPublisher<TakeDamageEventBus>>();

            m_takeHealPublisher = m_resolver.Resolve<IPublisher<TakeHealEventBus>>();
            
        }

        [PunRPC]
        private void OnTakeDamage_RPC(byte[] message) {
            
            var eventBus = TakeDamageEventBus.Deserialize(message);

            if (m_takeDamagePublisher is null) {
                m_takeDamagePublisher = m_resolver.Resolve<IPublisher<TakeDamageEventBus>>();
            }
            
            m_takeDamagePublisher?.Publish(eventBus);
            
        }

        [PunRPC]
        private void OnTakeHeal_RPC(byte[] bytes) {
            
            var eventBus = TakeHealEventBus.Deserialize(bytes);

            m_takeHealPublisher ??= m_resolver.Resolve<IPublisher<TakeHealEventBus>>();
            
            m_takeHealPublisher.Publish(eventBus);
            
        }
    }
}