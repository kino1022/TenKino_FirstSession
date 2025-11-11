using System.Collections.Generic;
using Fusion;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Scr.Status {
    public class FloatInitializer : SerializedMonoBehaviour {
        
        [OdinSerialize]
        [LabelText("初期化データ")]
        [TableList]
        private List<InitializeData<float>> _datas = new();

        private void Start() {

            foreach (var data in _datas) {
                if (data is null) {
                    Debug.LogError("初期化用データが存在ませんでした");
                    return;
                } 
                data.Initialize();
            }
        }
    }
}