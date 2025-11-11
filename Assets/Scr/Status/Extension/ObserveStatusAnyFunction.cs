using R3;

namespace Scr.Status.Extension {
    public static partial class StatusExtension {
        
        public static Observable<Unit> ObserveStatusAnyFunction<T>(this IStatus<T> status, System.Func<T, bool> predicate)
            where T : struct 
        {
            return Observable
                .EveryValueChanged(status, x => x.Value)
                .Where(predicate)
                .Select(_ => Unit.Default);
        }
        
    }
}