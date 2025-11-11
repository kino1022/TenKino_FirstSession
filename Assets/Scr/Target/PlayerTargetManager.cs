using R3;
using RinaSymbol;
using RinaSymbol.Group;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using VContainer;

namespace Scr.Target {

    /// <summary>
    /// ターゲットの管理を行うクラスに対して約束するインターフェース
    /// </summary>
    public interface ITargetManager : ITargetProvider {
        
        /// <summary>
        /// ターゲットの変更を行う
        /// </summary>
        /// <param name="target"></param>
        void ChangeTarget (ASymbol target);
        
    }

    /// <summary>
    /// ターゲットを保持するクラスに対して約束するインターフェース
    /// </summary>
    public interface ITargetProvider {
        
        /// <summary>
        /// ターゲット
        /// </summary>
        ReadOnlyReactiveProperty<ASymbol> Provide { get; }
        
    }
    
    public class PlayerTargetManager : SerializedMonoBehaviour , ITargetManager {
        
        private ReactiveProperty<ASymbol> m_target = new  ReactiveProperty<ASymbol>();
        
        public ReadOnlyReactiveProperty<ASymbol> Provide => m_target;

        [Title("参照")]
        
        [OdinSerialize]
        [ReadOnly]
        private IGroupTag m_selfTag;
        
        [OdinSerialize]
        [ReadOnly]
        private IGroupEntitiesProvider m_entitiesProvider;
        
        private IObjectResolver m_resolver;

        [Inject]
        public void Construct(IObjectResolver resolver) {
            m_resolver = resolver;
        }

        private void Start() {
            
            m_selfTag = m_resolver.Resolve<IGroupTag>();
            
            m_entitiesProvider = m_resolver.Resolve<IGroupEntitiesProvider>();
            
        }

        public void ChangeTarget(ASymbol target) {

            if (target is null) {
                Debug.Log("選択されたターゲットがnullでした");
                return;
            }
            
            Debug.Log($"{target.gameObject.name}にターゲットを変更します");
            
            m_target.Value = target;
            
        }
    }
    
}