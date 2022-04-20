using System;

namespace DeltaWare.Dependencies.Abstractions
{
    public interface IDependencyProvider
    {
        object GetDependency(Type type);
    }

    public static class DependencyProviderExtensions
    {
        public static T GetDependency<T>(this IDependencyProvider provider)
        {
            return (T)provider.GetDependency(typeof(T));
        }
    }
}
