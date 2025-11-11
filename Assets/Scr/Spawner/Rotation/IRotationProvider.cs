using UnityEngine;
using VContainer;

namespace Scr.Spawner.Rotation {
    public interface IRotationProvider {
        
        Quaternion Provider (IObjectResolver resolver, GameObject spawner);
    }
}