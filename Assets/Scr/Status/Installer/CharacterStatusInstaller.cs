using Scr.Status.Health;
using Scr.Status.Stamina;
using Scr.Utility;
using Sirenix.OdinInspector;
using VContainer;
using VContainer.Unity;

namespace Scr.Status.Installer {
    public class CharacterStatusInstaller : SerializedMonoBehaviour, IInstaller {

        public void Install(IContainerBuilder builder) {
            
            //体力の登録処理
            var health = gameObject.GetComponentFromWhole<IHealth>();
            if (health is not null) {
                builder
                    .RegisterComponent(health)
                    .As<IHealth>();
            }
            
            //最大体力の登録処理
            var maxHealth = gameObject.GetComponentFromWhole<IMaxHealth>();
            if (maxHealth is not null) {
                builder
                    .RegisterComponent(maxHealth)
                    .As<IMaxHealth>();
            }
            
            //現在スタミナの登録処理
            var stamina = gameObject.GetComponentFromWhole<IStamina>();
            if (stamina is not null) {
                builder
                    .RegisterComponent(stamina)
                    .As<IStamina>();
            }
            
            //最大スタミナの登録処理
            var maxStamina = gameObject.GetComponentFromWhole<IMaxStamina>();
            if (maxStamina is not null) {
                builder
                    .RegisterComponent(stamina)
                    .As<IMaxStamina>();
            }
            
        }
    }
}