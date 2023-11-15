using System;
using System.Collections.Generic;

namespace Utilities.ExtensionMethods.Collections
{
    public static class CollectionsExtensions_Remove
    {
        /// <summary>
        ///     Remove all items by key and predicate
        /// </summary>
        /// <param name="param">Key </param>
        /// <returns>Removed items count</returns>
        public static int RemoveAllBy<TItem, TParam>
        (
            this IList<TItem> collection,
            TParam param,
            Func<TParam, TItem, bool> predicate
        )
        {
            var removedCount = 0;
            for (var i = 0; i < collection.Count; i++)
            {
                if (predicate(param, collection[i]))
                {
                    collection.RemoveAt(i);
                    i--; // because we removed an item, we need to decrement the index
                    removedCount++;
                }
            }

            return removedCount;
        }

        public static void ClearIterative<TItem>(this IList<TItem> collection)
        {
            for (var i = 0; i < collection.Count; i++)
            {
                collection.RemoveAt(0);
            }
        }

        /// <summary>
        ///     Removed all items by predicate
        /// </summary>
        /// <returns>Removed items count</returns>
        public static int RemoveAllBy<TItem>
        (
            this IList<TItem> collection,
            Predicate<TItem> predicate
        )
        {
            var removedCount = 0;
            for (var i = 0; i < collection.Count; i++)
            {
                if (predicate(collection[i]))
                {
                    collection.RemoveAt(i);
                    i--; // because we removed an item, we need to decrement the index
                    removedCount++;
                }
            }

            return removedCount;
        }

        public static bool RemoveFirstBy<TItem, TKey>
        (
            this IList<TItem> collection,
            TKey key,
            Func<TKey, TItem, bool> predicate
        )
        {
            for (var i = 0; i < collection.Count; i++)
            {
                if (predicate(key, collection[i]))
                {
                    collection.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }
    }
}