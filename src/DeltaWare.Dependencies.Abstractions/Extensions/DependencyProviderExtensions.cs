using DeltaWare.Dependencies.Abstractions.Exceptions;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace DeltaWare.Dependencies.Abstractions
{
    public static class DependencyProviderExtensions
    {
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
    }
}