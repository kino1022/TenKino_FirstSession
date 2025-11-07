using RinaStatus;

namespace Scr.Status.Stamina {
    
    public interface IMaxStamina : ICorrectableStatus<float> {
        
    }
    
    public class MaxStamina : ACorrectableStatus<float>, IMaxStamina {
        
    }
    
}