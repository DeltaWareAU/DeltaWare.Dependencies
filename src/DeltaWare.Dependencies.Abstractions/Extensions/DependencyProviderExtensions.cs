using DeltaWare.Dependencies.Abstractions.Exceptions;
using System;
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

        /// <summary>
        /// Instantiates an instance of the specified <see cref="Type"/>.
        /// </summary>
        /// <remarks>This is intended for instantiating an unregistered <see cref="Type"/>. The instance is <strong>Scoped</strong> and <strong>Bound</strong> to the <see cref="IDependencyProvider"/>.</remarks>
        /// <typeparam name="T">The <see cref="Type"/> to be instantiated.</typeparam>
        /// <returns>A new instance of the specified <see cref="Type"/> or <see langword="null"/> if it could not be instantiated.</returns>
        public static T CreateInstance<T>(this IDependencyProvider provider) where T : class
        {
            return (T)provider.CreateInstance(typeof(T));
        }

        /// <summary>
        /// Instantiates an instance of the specified <see cref="Type"/>.
        /// </summary>
        /// <remarks>This is intended for instantiating an unregistered <see cref="Type"/>. The instance is <strong>Scoped</strong> and <strong>Bound</strong> to the <see cref="IDependencyProvider"/>.</remarks>
        /// <typeparam name="T">The <see cref="Type"/> to be instantiated.</typeparam>
        /// <param name="instance">An instance of the specified <see cref="Type"/>.</param>
        /// <returns><see langword="true"/> if an instance was instantiated or <see langword="false"/> if an instance could not be instantiated.</returns>
        public static bool TryCreateInstance<T>(this IDependencyProvider provider, out T instance) where T : class
        {
            if (!provider.TryCreateInstance(typeof(T), out object objectInstance))
            {
                instance = null;

                return false;
            }

            instance = (T)objectInstance;

            return true;
        }
    }
}