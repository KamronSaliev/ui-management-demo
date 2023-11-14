using System;

namespace Utilities.ExtensionMethods
{
    public static class EnumAttributeExtensions
    {
        public static T GetAttributeOfType<T>(this Enum enumVal) where T : Attribute
        {
            var type = enumVal.GetType();
            var memberInfos = type.GetMember(enumVal.ToString());
            var attributes = memberInfos[0].GetCustomAttributes(typeof(T), false);
            return attributes.Length > 0 ? (T)attributes[0] : null;
        }
    }
}