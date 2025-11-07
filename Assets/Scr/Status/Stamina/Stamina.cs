using RinaStatus;

namespace Scr.Status.Stamina {

    public interface IStamina : IStatus<float> {
        
    }
    
    public class Stamina : AStatus<float> , IStamina {
        
    }
}