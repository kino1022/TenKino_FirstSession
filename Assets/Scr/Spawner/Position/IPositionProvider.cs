using UnityEngine;
using VContainer;

namespace Scr.Spawner.Position {
    public interface IPositionProvider {
        
        Vector3 Provider (IObjectResolver resolver, GameObject spawner);
        
    }
}