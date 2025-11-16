using R3;

namespace Scr.Player.Move.Fall {

    public interface IGroundedProvider {
        ReadOnlyReactiveProperty<bool> IsGrounded { get; }
    }
    
    public class GroundedProvider {
        
    }
}