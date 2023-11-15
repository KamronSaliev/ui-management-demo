using System;
using System.Collections.Generic;
using UniRx;

namespace Utilities.ExtensionMethods.RX.Collections
{
    public static class RxCollectionItemEffectsExtensions
    {
        private static void ExecuteEffectOnItem<T>
        (
            int index,
            T item,
            Func<int, T, IDisposable> effect,
            Dictionary<T, IDisposable> subscriptions,
            CompositeDisposable disposables
        )
        {
            var disposable = effect(index, item);
            disposable.AddTo(disposables);
            subscriptions.Add(item, disposable);
        }

        /// <summary>
        /// Effect is going to be executed on every item and on every new item added to the collection. 
        /// </summary>
        /// <param name="collection">Collection to observe</param>
        /// <param name="effect">Effect to execute on every item, returns disposable to run when item is removed from collection</param>
        public static IDisposable SubscribeToItems<T>
        (
            this IReadOnlyReactiveCollection<T> collection,
            Func<int, T, IDisposable> effect
        )
        {
            void ItemAdded
            (
                CollectionAddEvent<T> eventData,
                Func<int, T, IDisposable> effect,
                Dictionary<T, IDisposable> subscriptions,
                CompositeDisposable disposables
            )
            {
                ExecuteEffectOnItem(eventData.Index, eventData.Value, effect, subscriptions, disposables);
            }

            void ItemRemoved(CollectionRemoveEvent<T> eventData, Dictionary<T, IDisposable> subscriptions)
            {
                subscriptions[eventData.Value].Dispose();
                subscriptions.Remove(eventData.Value);
            }

            void Reset(UniRx.Unit unit, Dictionary<T, IDisposable> subscriptions)
            {
                foreach (var subscription in subscriptions.Values)
                {
                    subscription.Dispose();
                }

                subscriptions.Clear();
            }

            var disposables = new CompositeDisposable();

            var subscriptions = new Dictionary<T, IDisposable>();
            for (var index = 0; index < collection.Count; index++)
            {
                ExecuteEffectOnItem(index, collection[index], effect, subscriptions, disposables);
            }

            collection.ObserveAdd().SubscribeWithState3(effect, subscriptions, disposables, ItemAdded).AddTo(disposables);
            collection.ObserveRemove().SubscribeWithState(subscriptions, ItemRemoved).AddTo(disposables);
            collection.ObserveReset().SubscribeWithState(subscriptions, Reset).AddTo(disposables);

            return disposables;
        }

        /// <summary>
        /// Effect is going to be executed on every item and on every new item added to the collection.
        /// </summary>
        /// <param name="effect">Effect to execute on every item, returns disposable to run when item is removed from collection</param>
        public static IDisposable SubscribeToItems<T>(this IReadOnlyReactiveCollection<T> collection, Action<int, T> effect)
        {
            for (var index = 0; index < collection.Count; index++)
            {
                var item = collection[index];
                effect(index, item);
            }

            return collection.ObserveAdd().SubscribeWithState(effect, (x, effect) => effect(x.Index, x.Value));
        }
    }
}