using Photon.Pun;
using UnityEngine;
using VContainer;

namespace Scr.Utility {
    public static class ConstructableExtension {
        
        /// <summary>
        /// PhotonNetworkを使用してオブジェクトを生成し、IConstructableを実装しているコンポーネントに対して依存性注入を行う拡張メソッド
        /// </summary>
        /// <param name="resolver"></param>
        /// <param name="prefabName"></param>
        /// <param name="vec"></param>
        /// <param name="rot"></param>
        /// <returns></returns>
        public static GameObject Construct(this IObjectResolver resolver,string prefabName, Vector3 vec , Quaternion rot ) {
;           var instance = PhotonNetwork.Instantiate(prefabName, vec, rot);

            var constructable = instance.GetComponentsFromWhole<IConstructable>();

            if (constructable.Length !> 0) {
                return instance;
            }

            foreach (var construct in constructable) {
                construct.Construct(resolver);
            }
            
            return instance;    
        }
    }
}