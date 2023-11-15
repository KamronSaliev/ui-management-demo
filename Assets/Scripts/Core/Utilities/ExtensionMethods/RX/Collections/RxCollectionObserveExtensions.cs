using System;
using UniRx;

namespace Utilities.ExtensionMethods.RX.Collections
{
    public static class RxCollectionObserveExtensions
    {
        public static IObservable<T> ObserveIndex<T>(this IReadOnlyReactiveCollection<T> source, int index) where T : class
        {
            return Observable.Return((source, index)).Merge
                (
                    source.ObserveAdd().AsObservable((source, index)),
                    source.ObserveRemove().AsObservable((source, index)),
                    source.ObserveReplace().AsObservable((source, index)),
                    source.ObserveReset().AsObservable((source, index))
                )
                .Select(state => state.source.Count > state.index ? state.source[state.index] : null)
                .StartWith(default(T))
                .Pairwise()
                .Where(pair => pair.Previous != pair.Current)
                .Select(pair => pair.Current);
        }
    }
}