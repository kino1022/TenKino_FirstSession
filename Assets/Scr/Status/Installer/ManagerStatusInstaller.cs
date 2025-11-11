using RinaStatus.Installer.Manager;
using Scr.Status.Core;
using Sirenix.OdinInspector;
using VContainer;
using VContainer.Unity;

namespace Scr.Status.Installer {
    
    [InfoBox("ゲームマネージャが持つべきステータス管理システム用のインストーラー")]
    public class ManagerStatusInstaller : IInstaller {

        public void Install(IContainerBuilder builder) {
            
            StatusManagerInstaller installer = new StatusManagerInstaller();
            
            installer.Install(builder);
            
            builder
                .Register<StatusCore<int>>(Lifetime.Transient)
                .As<IStatusCore<int>>();

            builder
                .Register<StatusCore<float>>(Lifetime.Transient)
                .As<IStatusCore<float>>();

            builder
                .Register<CorrectableStatusCore<int>>(Lifetime.Transient)
                .As<ICorrectableStatusCore<int>>();

            builder
                .Register<CorrectableStatusCore<float>>(Lifetime.Transient)
                .As<ICorrectableStatusCore<float>>();
            
        }
    }
}