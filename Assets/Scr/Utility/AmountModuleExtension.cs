using R3;

namespace Scr.Utility {
    
    public static class AmountModuleExtension {
        
        public static Observable<Unit> ObserveLowerAnyValue (this IAmountModule self, int threshold) {
            
            return self.Amount
                .Where(amount => amount < threshold)
                .AsUnitObservable();
            
        }

        public static Observable<Unit> ObserveHighAnyValue (this IAmountModule self, int threshold) {

            return self
                .Amount
                .Where(amount => amount > threshold)
                .AsUnitObservable();
            
        }
        
        public static Observable<Unit> ObserveZero (this IAmountModule self) {

            return self.ObserveLowerAnyValue(0);
        }
    }
}