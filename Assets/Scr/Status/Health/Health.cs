using RinaStatus;

namespace Scr.Status.Health {

    public interface IHealth : IStatus<int> {
        
    }
    
    public class Health : AStatus<int>, IHealth {
        
    }
}