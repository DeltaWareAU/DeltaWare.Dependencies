// ReSharper disable once CheckNamespace
namespace System.Reflection
{
    internal static class AttributeExtensions
    {
        public static bool HasAttribute<TAttribute>(this PropertyInfo propertyInfo) where TAttribute : Attribute
        {
            return propertyInfo.GetCustomAttribute<TAttribute>(true) != null;
        }

        public static bool HasAttribute<TAttribute>(this ConstructorInfo propertyInfo) where TAttribute : Attribute
        {
            return propertyInfo.GetCustomAttribute<TAttribute>(true) != null;
        }
    }
}
