using RinaStatus;

namespace Scr.Status.Health {
    
    public interface IMaxHealth : ICorrectableStatus<int> {
        
    }
    
    public class MaxHealth : ACorrectableStatus<int> , IMaxHealth {
        
    }
}