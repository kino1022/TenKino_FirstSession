using System.Collections.Generic;
using Fusion;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using VContainer;
using UnityEngine;

namespace Scr.Player.Move {
    public class CharacterMoveManager : MovementBehaviour {

        [OdinSerialize]
        [LabelText("運動量マネージャ")]
        private List<IMovementManager> _managers;
        
        private NetworkCharacterController _characterController;

        private void Start() {
            _characterController = _resolver.Resolve<NetworkCharacterController>();
        }

        private void Update() {
            
            _movement = CalculateMovement();
            
            _characterController.Move(_movement * Time.deltaTime);
            
        }


        private Vector3 CalculateMovement() {
            
            if (_managers.Count is 0 || _managers is null || _isEnable is false) return Vector3.zero;
            
            var result = Vector3.zero;
            
            _managers.ForEach(x => {

                if (x is null) {
                    Debug.LogWarning("Movement Manager is null");
                    return;
                }
                
                result += x.Movement;
            });
            
            return result;
        }
    }
}