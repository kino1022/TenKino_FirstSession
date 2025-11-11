using Fusion;
using RinaSymbol;
using Scr.Utility;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Scr.Player {
    public class PlayerLifetimeScope  : SymbolLifetimeScope {
        protected override void Configure(IContainerBuilder builder) {
            
            base.Configure(builder);

            var fusionObj = gameObject.transform.root.gameObject.GetComponent<NetworkObject>();

            if (fusionObj == null) {
                Debug.LogError($"{gameObject.name}のルートにNetworkObjectが見つかりません。");
                return;
            }
            
            builder
                    .RegisterComponent(fusionObj)
                    .As<NetworkObject>();
            
            var player = gameObject.GetComponentFromWhole<Player>();

            if (player is null) {
                Debug.LogError("Playerにシンボルコンポーネントがアタッチされていませんでした");
                return;
            }
            
            builder
                .RegisterComponent(player)
                .As<Player>();

        }
    
    }
}