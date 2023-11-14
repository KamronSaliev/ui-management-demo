using System.Collections.Generic;

namespace Utilities.ExtensionMethods.Collections
{
    public static class CollectionsExtensions
    {
        public static void AddRange<T>(this IList<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                collection.Add(item);
            }
        }

        public static List<T> ToList<T>(this HashSet<T> set)
        {
            var result = new List<T>();

            foreach (var element in set)
            {
                result.Add(element);
            }

            return result;
        }

        public static List<T> ToList<T>(this Queue<T> queue)
        {
            var result = new List<T>();

            for (var i = 0; i < queue.Count; i++)
            {
                result.Add(queue.Dequeue());
            }

            return result;
        }

        public static List<T> ToList<T>(this Stack<T> stack)
        {
            var result = new List<T>();

            for (var i = 0; i < stack.Count; i++)
            {
                result.Add(stack.Pop());
            }

            return result;
        }

        public static HashSet<T> ToHashSet<T>(this List<T> list)
        {
            var result = new HashSet<T>();

            foreach (var element in list)
            {
                result.Add(element);
            }

            return result;
        }

        public static Queue<T> ToQueue<T>(this List<T> list)
        {
            var result = new Queue<T>();

            foreach (var element in list)
            {
                result.Enqueue(element);
            }

            return result;
        }

        public static Stack<T> ToStack<T>(this List<T> list)
        {
            var result = new Stack<T>();

            for (var i = list.Count; i >= 0; i--)
            {
                result.Push(list[i]);
            }

            return result;
        }
    }
}