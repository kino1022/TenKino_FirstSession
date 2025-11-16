using System.Collections.Generic;
using Fusion;
using R3;
using RinaInput.Controller.Command;
using RinaInput.Controller.Module;
using RinaInput.Provider;
using Scr.AppManager;
using Scr.Utility;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using VContainer;

namespace Scr.Player.Input {
    public class PlayerInput : SerializedMonoBehaviour, IConstructable {

        private INetworkPropertyProvider _networkProperty;

        [OdinSerialize]
        [LabelText("ボタンの入力モジュール")]
        private List<IInputModule<float>> _buttonModules = new();
        
        [OdinSerialize]
        [LabelText("レバーの入力モジュール")]
        private List<IInputModule<Vector2>> _leverModules = new();
        
        [OdinSerialize]
        [LabelText("入力コマンド")]
        private List<IInputCommand> _commands = new();
        
        private IInputStreamProvider _streamProvider;

        private IObjectResolver _resolver;

        [Inject]
        public void Construct(IObjectResolver resolver) {
            _resolver = resolver;
        }

        private void Start() {

            _streamProvider = _resolver.Resolve<IInputStreamProvider>();

            _networkProperty = _resolver.Resolve<INetworkPropertyProvider>();

            _buttonModules?.ForEach(x =>
            {
                x?.Start();
                x?.GenerateStream(_streamProvider);
                x?.ChangeEnable(true);
            });

            _leverModules?.ForEach(x =>
            {
                x?.Start();
                x?.GenerateStream(_streamProvider);
                x?.ChangeEnable(true);
            });

            _commands?.ForEach(x =>
            {
                x?.GenerateStream();
                x?.ChangeEnable(true);
            });
            
            RegisterNetworkProperty();

        }
        
        private void AllInputsChangeEnable(bool isEnable) {
            _buttonModules?.ForEach(x => x?.ChangeEnable(isEnable));
            _leverModules?.ForEach(x => x?.ChangeEnable(isEnable));
            _commands?.ForEach(x => x?.ChangeEnable(isEnable));
        }
        
        private void RegisterNetworkProperty() {
            _networkProperty
                .InputAuthority
                .Subscribe(AllInputsChangeEnable)
                .AddTo(this);
        }
        
    }
}