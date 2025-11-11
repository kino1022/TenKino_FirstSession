using System;
using System.Collections.Generic;
using Fusion;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Scr.Status {
    /// <summary>
    /// ステータスの初期化を行うクラス
    /// </summary>
    public class IntStatusInitializer : SerializedMonoBehaviour {

        [OdinSerialize]
        [LabelText("ステータスと初期値")] 
        [TableList]
        private List<InitializeData<int>> _datas = new();

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
    
    [Serializable]
    public class InitializeData<T> where T : struct {

        [OdinSerialize]
        [LabelText("ステータス")]
        private  IStatus<T> _status;

        [SerializeField]
        [LabelText("初期値")]
        private T _initValue;

       public InitializeData() { }

       public InitializeData(IStatus<T> status, T initValue) {
           _status = status;
              _initValue = initValue;
       }

        public void Initialize() {
            
            if (_status is null) {
                Debug.Log("ステータスが指定されていませんでした");
                return;
            }
            
            _status.Set(_initValue);
            
        }
    }
}