using DeltaWare.Dependencies.Abstractions.Exceptions;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace DeltaWare.Dependencies.Abstractions
{
    public static class DependencyProviderExtensions
    {
        public static TDependency GetDependency<TDependency>(this IDependencyProvider provider) where TDependency : class
        {
            return (TDependency)provider.GetDependency(typeof(TDependency));
        }

        public static IEnumerable<TDependency> GetRequiredDependencies<TDependency>(this IDependencyProvider provider) where TDependency : class
        {
            if (provider.TryGetDependencies(out IEnumerable<TDependency> instances))
            {
                return instances;
            }

            throw new DependencyNotFoundException(typeof(TDependency));
        }

        public static TDependency GetRequiredDependency<TDependency>(this IDependencyProvider provider) where TDependency : class
        {
            if (provider.TryGetDependency(out TDependency instance))
            {
                return instance;
            }

            throw new DependencyNotFoundException(typeof(TDependency));
        }

        public static bool HasDependency<TDependency>(this IDependencyProvider provider) where TDependency : class
        {
            return provider.HasDependency(typeof(TDependency));
        }

        public static bool TryGetDependencies<TDependency>(this IDependencyProvider provider, out IEnumerable<TDependency> instances) where TDependency : class
        {
            if (!provider.TryGetDependencies(typeof(TDependency), out IEnumerable<object> objectInstances))
            {
                instances = null;

                return false;
            }

            instances = objectInstances.Cast<TDependency>();

            return true;
        }

        public static bool TryGetDependency<TDependency>(this IDependencyProvider provider, out TDependency instance) where TDependency : class
        {
            if (!provider.TryGetDependency(typeof(TDependency), out object objectInstance))
            {
                instance = null;

                return false;
            }

            instance = (TDependency)objectInstance;

            return true;
        }
    }
}