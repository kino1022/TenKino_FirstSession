using Fusion;
using RinaSymbol;
using Scr.Utility;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Scr.AppManager {
    public class ApplicationManagerLifetimeScope : SymbolLifetimeScope {
        
        protected override void Configure(IContainerBuilder builder) {
            base.Configure(builder);

            var runner = gameObject.GetComponentFromWhole<NetworkRunner>();

            if (runner is null) {
                Debug.Log("アプリケーション管理マネージャーにNetworkRunnerが存在しません。");
                return;
            }

            builder
                .RegisterComponent(runner)
                .As<NetworkRunner>();
            
        }
        
    }
}