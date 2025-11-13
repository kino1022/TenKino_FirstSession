using Fusion;

namespace Scr.AppManager {

    public interface IRunnerProvider {

        NetworkRunner Provide();
        
        void SetRunner(NetworkRunner runner);
        
    }
    
    public class RunnerProvider {
        
    }
}