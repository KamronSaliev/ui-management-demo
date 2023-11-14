using System;
using System.Collections.Generic;
using UniRx;

namespace Utilities.ExtensionMethods.RX
{
    public static class RxObservableExtensions
    {
        public static IObservable<T> WhereNotNull<T>(this IObservable<T> observable) where T : class
        {
            return observable.Where(v => !v.IsUnityNull());
        }

        public static IObservable<T> WhereNull<T>(this IObservable<T> observable) where T : class
        {
            return observable.Where(v => v.IsUnityNull());
        }

        public static IObservable<T> SelectValue<T>(this IObservable<CollectionAddEvent<T>> eventObservable)
        {
            return eventObservable.Select(e => e.Value);
        }

        public static IObservable<T> SelectValue<T>(this IObservable<CollectionRemoveEvent<T>> eventObservable)
        {
            return eventObservable.Select(e => e.Value);
        }

        public static IObservable<bool> WhereTrue(this IObservable<bool> observable)
        {
            return observable.Where(v => v);
        }

        public static IObservable<bool> WhereFalse(this IObservable<bool> observable)
        {
            return observable.Where(v => !v);
        }

        public static IObservable<TR> AsObservable<T, TR>(this IObservable<T> observable, TR value)
        {
            return observable.Select(_ => value);
        }

        public static IObservable<Unit> ToAnyCompletedObservable<T>(this IEnumerable<IObservable<T>> observables)
        {
            var subject = new Subject<Unit>();
            var disposables = new CompositeDisposable();

            foreach (var observable in observables)
            {
                observable.Subscribe
                    (
                        u => { },
                        subject.OnError,
                        subject.OnCompleted
                    )
                    .AddTo(disposables);
            }

            return subject.Finally(disposables.Dispose);
        }

        public static IDisposable SubscribeWithFactory<TSource, TResult>
        (
            this IObservable<TSource> observable,
            Func<TSource, TResult> produce,
            Action<TResult> dispose
        )
        {
            var valueProperty = new ReactiveProperty<TResult>();

            var subscriptionDisposable = observable.SubscribeWithState((valueProperty, produce, dispose),
                (source, state) =>
                {
                    if (state.valueProperty.Value != null)
                    {
                        state.dispose(state.valueProperty.Value);
                    }

                    if (source != null)
                    {
                        state.valueProperty.Value = state.produce(source);
                    }
                });

            return StableCompositeDisposable.Create
            (
                subscriptionDisposable,
                Disposable.CreateWithState((valueProperty, dispose), state =>
                {
                    if (state.valueProperty.Value != null)
                    {
                        state.dispose(state.valueProperty.Value);
                    }
                }),
                valueProperty
            );
        }

        public static IDisposable ChainedSubscribe<T>(this IObservable<T> observable,
            Action<T, CompositeDisposable> innerSubscribe)
        {
            var disposable = new CompositeDisposable();

            var outerDisposable = observable.SubscribeWithState(disposable, innerSubscribe);

            return StableCompositeDisposable.Create(outerDisposable, disposable);
        }

        public static IDisposable ChainedSubscribe<T>(this IObservable<T> observable,
            Func<T, IDisposable> innerSubscribe)
        {
            var disposable = new MultipleAssignmentDisposable();

            var outerDisposable = observable
                .SubscribeWithState((disposable, innerSubscribe),
                    (arg, state) => state.disposable.Disposable = state.innerSubscribe(arg));

            return StableCompositeDisposable.Create(outerDisposable, disposable);
        }
    }
}