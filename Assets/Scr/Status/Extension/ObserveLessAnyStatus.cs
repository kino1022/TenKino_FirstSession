using System;
using R3;

namespace Scr.Status.Extension {
    public static partial class StatusExtension {
        
        public static Observable<Unit> ObserveLessAnyStatus<T>(this IStatus<T> status, IStatus<T> raw)
            where T : struct {

            return Observable
                .CombineLatest(
                    Observable
                        .EveryValueChanged(status, x => x.Value),
                    Observable
                        .EveryValueChanged(raw, x => x.Value),
                    (value, rawValue) => ((IComparable)value, (IComparable)rawValue)
                )
                .Where(x => x.Item1.CompareTo(x.Item2) < 0)
                .Select(_ => Unit.Default);
        }
    }
}