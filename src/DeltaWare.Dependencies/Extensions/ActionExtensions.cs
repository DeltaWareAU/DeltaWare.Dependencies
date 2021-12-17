// ReSharper disable once CheckNamespace
namespace System
{
    public static class ActionExtensions
    {
        public static Action<object> Convert<T>(this Action<T> action)
        {
            return o => action((T)o);
        }
    }
}