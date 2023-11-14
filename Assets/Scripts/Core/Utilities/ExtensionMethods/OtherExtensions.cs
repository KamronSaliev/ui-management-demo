using UnityEngine;

namespace Utilities.ExtensionMethods
{
    public static class OtherExtensions
    {
        public static TObject Nullable<TObject>(this TObject obj) where TObject : Object
        {
            if (obj is null)
            {
                return null;
            }

            return !obj ? null : obj;
        }

        public static bool IsUnityNull<T>(this T obj) where T : class
        {
            if (obj is Object uObj)
            {
                return uObj.Nullable() is null;
            }

            return obj is null;
        }
    }
}