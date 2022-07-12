using DeltaWare.Dependencies.Abstractions.Exceptions;
using System;
using System.Reflection;

namespace DeltaWare.Dependencies.Abstractions
{
    public interface IDependencyProvider : IDisposable
    {
        object GetDependency(Type definition);
        bool TryGetDependency(Type definition, out object instance);

        object CreateInstance(Type definition);

        bool HasDependency(Type definition);
    }

    public static class DependencyProviderExtensions
    {
        public static bool TryGetDependency<T>(this IDependencyProvider provider, out T instance)
        {
            if (provider.TryGetDependency(typeof(T), out object value))
            {
                instance = (T) value;

                return true;
            }

            instance = default;

            return false;
        }

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
            if (provider.TryGetDependency(definition, out object instance))
            {
                return instance;
            }

            throw new DependencyNotFoundException(definition);
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
