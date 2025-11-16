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

            builder
                .Register<RunnerProvider>(Lifetime.Singleton)
                .AsImplementedInterfaces();

        }
        
    }
}