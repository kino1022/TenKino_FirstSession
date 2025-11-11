namespace Scr.Status.Health {
    
    public interface IMaxHealth : ICorrectableStatus<int> {
        
    }
    public class MaxHealth : IntCorrectableStatus, IMaxHealth {
        
        
    }
}