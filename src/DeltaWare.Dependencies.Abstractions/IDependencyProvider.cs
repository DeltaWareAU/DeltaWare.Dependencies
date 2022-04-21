using DeltaWare.Dependencies.Abstractions.Exceptions;
using System;

namespace DeltaWare.Dependencies.Abstractions
{
    public interface IDependencyProvider : IDisposable
    {
        object GetDependency(Type definition);

        object CreateInstance(Type definition);

        bool HasDependency(Type definition);
    }

    public static class DependencyProviderExtensions
    {
        public static T GetDependency<T>(this IDependencyProvider provider)
        {
            return (T)provider.GetDependency(typeof(T));
        }

        public static T CreateInstance<T>(this IDependencyProvider provider)
        {
            return (T)provider.CreateInstance(typeof(T));
        }

        public static object GetRequiredDependency(this IDependencyProvider provider, Type definition)
        {
            object dependency = provider.GetDependency(definition);

            if (dependency == null)
            {
                throw new DependencyNotFoundException(definition);
            }

            return dependency;
        }

        public static T GetRequiredDependency<T>(this IDependencyProvider provider)
        {
            return (T)provider.GetRequiredDependency(typeof(T));
        }

        public static bool HasDependency<T>(this IDependencyProvider provider)
        {
            return provider.HasDependency(typeof(T));
        }
    }
}
