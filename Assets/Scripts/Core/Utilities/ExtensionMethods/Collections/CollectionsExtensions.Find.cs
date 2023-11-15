using System;
using System.Collections.Generic;

namespace Utilities.ExtensionMethods.Collections
{
    public static class CollectionsExtensions_Find
    {
        /// <summary>
        ///     Find item by key and predicate
        /// </summary>
        /// <param name="param">Key</param>
        /// <returns>Found item or default</returns>
        public static bool FindBy<TItem, TParam>
        (
            this IList<TItem> collection,
            TParam param,
            out TItem result,
            Func<TParam, TItem, bool> predicate
        )
        {
            result = default;

            for (var i = 0; i < collection.Count; i++)
            {
                var item = collection[i];
                if (predicate(param, item))
                {
                    result = item;
                    return true;
                }
            }

            return false;
        }

        public static TItem FindByOrDefault<TItem, TParam>(
            this IList<TItem> collection,
            TParam param,
            Func<TParam, TItem, bool> predicate
        )
        {
            for (var i = 0; i < collection.Count; i++)
            {
                var item = collection[i];
                if (predicate(param, item))
                {
                    return item;
                }
            }

            return default;
        }
    }
}