using System;
using System.Collections.Generic;
using UniRx;

namespace Utilities.ExtensionMethods.RX.Collections
{
    public static class RxCollectionsOneWayBindingExtensions
    {
        public static IDisposable BindAddAndRemoveToRxCollection<TSourceItem, TTargetValue>
        (
            this IList<TTargetValue> targetList,
            IReadOnlyReactiveCollection<TSourceItem> source,
            Func<TSourceItem, TTargetValue> targetFactory,
            Action<TTargetValue> targetDisposer
        )
        {
            void Reset(IList<TTargetValue> list, Action<TTargetValue> disposer)
            {
                foreach (var view in list)
                {
                    disposer(view);
                }

                list.Clear();
            }

            var addSubscription = source.ObserveAdd().Subscribe(x =>
            {
                var view = targetFactory(x.Value);
                targetList.Insert(x.Index, view);
            });

            var removeSubscription = source.ObserveRemove().Subscribe(x =>
            {
                var view = targetList[x.Index];
                targetList.RemoveAt(x.Index);
                targetDisposer(view);
            });

            var resetSubscription = source
                .ObserveReset()
                .SubscribeWithState2(targetList, targetDisposer, (u, list, disposer) => Reset(list, disposer));

            Reset(targetList, targetDisposer);

            foreach (var viewModel in source)
            {
                var view = targetFactory(viewModel);
                targetList.Add(view);
            }

            var listDisposer = Disposable.CreateWithState((targetList, targetDisposer),
                state => { Reset(state.targetList, state.targetDisposer); });

            return StableCompositeDisposable.Create(addSubscription, removeSubscription, resetSubscription, listDisposer);
        }

        /// <summary>
        /// Binds reactive collection content change to another collection using filtration.
        /// Order is not preserved.
        /// Size of target collection is always equal or less than source collection.
        /// </summary>
        /// <param name="target">Collection which contains source items except filtered ones</param>
        /// <param name="source">Source collection, before filtering</param>
        /// <param name="filterPredicate">Predicate used to decide which item should or should not be in target collection</param>
        /// <returns>Binding disposer</returns>
        public static IDisposable BindAddAndRemoveToRxCollectionWithFilter<T>
        (
            this IList<T> target,
            IReadOnlyReactiveCollection<T> source,
            Func<T, bool> filterPredicate
        )
        {
            var addSubscription = source.ObserveAdd().Subscribe(x =>
            {
                if (filterPredicate(x.Value))
                {
                    target.Add(x.Value);
                }
            });

            var removeSubscription = source.ObserveRemove().Subscribe(x =>
            {
                if (filterPredicate(x.Value))
                {
                    target.Remove(x.Value);
                }
            });

            var resetSubscription = source.ObserveReset().Subscribe(x => target.Clear());

            target.Clear();

            foreach (var item in source)
            {
                if (filterPredicate(item))
                {
                    target.Add(item);
                }
            }

            return StableCompositeDisposable.Create(addSubscription, removeSubscription, resetSubscription);
        }

        public static IDisposable BindAddAndRemoveToRxCollectionWithCast<TItemSource, TItemTarget>
        (
            this IList<TItemTarget> target,
            IReadOnlyReactiveCollection<TItemSource> source
        )
            where TItemSource : TItemTarget
            where TItemTarget : class
        {
            var addSubscription = source
                .ObserveAdd()
                .SubscribeWithState(target, (e, target) => target.Insert(e.Index, e.Value));

            var removeSubscription = source
                .ObserveRemove()
                .SubscribeWithState(target, (e, target) => target.RemoveAt(e.Index));

            var resetSubscription = source
                .ObserveReset()
                .SubscribeWithState(target, (e, target) => target.Clear());

            target.Clear();
            foreach (var item in source)
            {
                target.Add(item);
            }

            return StableCompositeDisposable.Create(addSubscription, removeSubscription, resetSubscription);
        }

        public static IDisposable BindAddAndRemoveToRxCollectionWithCast<TItemSource, TItemTarget>
        (
            this IList<TItemTarget> target,
            IReadOnlyReactiveCollection<TItemSource> source,
            Func<TItemSource, TItemTarget> castOperation
        )
        {
            void Added(CollectionAddEvent<TItemSource> e, IList<TItemTarget> list) =>
                list.Insert(e.Index, castOperation(e.Value));

            void Removed(CollectionRemoveEvent<TItemSource> e, IList<TItemTarget> list) =>
                list.RemoveAt(e.Index);

            void Reset(UniRx.Unit e, IList<TItemTarget> list) =>
                list.Clear();

            var addSubscription = source
                .ObserveAdd()
                .SubscribeWithState(target, Added);

            var removeSubscription = source
                .ObserveRemove()
                .SubscribeWithState(target, Removed);

            var resetSubscription = source
                .ObserveReset()
                .SubscribeWithState(target, Reset);

            target.Clear();
            foreach (var item in source)
            {
                target.Add(castOperation(item));
            }

            return StableCompositeDisposable.Create(addSubscription, removeSubscription, resetSubscription);
        }
    }
}