using Scr.Status.Health;
using Scr.Utility;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Scr.Status.Installer {
    public class CharacterStatusInstaller : SerializedMonoBehaviour, IInstaller {
        public void Install(IContainerBuilder builder) {
            
            var health = gameObject.GetComponentFromWhole<IHealth>();

            if (health is null) {
                Debug.LogError("Healthコンポーネントがアタッチされていませんでした");
                return;
            }

            builder
                .RegisterComponent(health)
                .As<IHealth>();
            
            var maxHealth = gameObject.GetComponentFromWhole<IMaxHealth>();

            if (maxHealth is null) {
                Debug.LogError("MaxHealthコンポーネントがアタッチされていませんでした");
                return;
            }

            builder
                .RegisterComponent(maxHealth)
                .As<IMaxHealth>();
        }
    }
}