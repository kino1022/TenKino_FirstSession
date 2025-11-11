using System.Collections.Generic;
using Scr.UI.SessionList.Container;

namespace Scr.UI {

    public interface ISessionListView {
        
        List<ISessionContainer> Sessions { get; }
        
    }
    
    public class SessionListView {
        
    }
}