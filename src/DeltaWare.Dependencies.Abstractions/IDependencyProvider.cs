using System;
using System.Collections.Generic;

namespace DeltaWare.Dependencies.Abstractions
{
    /// <summary>
    /// Provides instances of dependency whilst handling their lifetimes.
    /// </summary>
    public interface IDependencyProvider : IDisposable
    {
        IDependencyScope CreateScope();

        IEnumerable<object> GetDependencies(Type dependencyType);

        object GetDependency(Type dependencyType);

        bool HasDependency(Type dependencyType);

        bool TryGetDependencies(Type dependencyType, out IEnumerable<object> instances);

        bool TryGetDependency(Type dependencyType, out object instance);

        /// <summary>
        /// Instantiates an instance of the specified <see cref="Type"/>.
        /// </summary>
        /// <remarks>This is intended for instantiating an unregistered <see cref="Type"/>. The instance is <strong>Scoped</strong> and <strong>Bound</strong> to the <see cref="IDependencyProvider"/>.</remarks>
        /// <param name="type">The <see cref="Type"/> to be instantiated.</param>
        /// <returns>A new instance of the specified <see cref="Type"/> or <see langword="null"/> if it could not be instantiated.</returns>
        object CreateInstance(Type type);

        /// <summary>
        /// Instantiates an instance of the specified <see cref="Type"/>.
        /// </summary>
        /// <remarks>This is intended for instantiating an unregistered <see cref="Type"/>. The instance is <strong>Scoped</strong> and <strong>Bound</strong> to the <see cref="IDependencyProvider"/>.</remarks>
        /// <param name="type">The <see cref="Type"/> to be instantiated.</param>
        /// <param name="instance">An instance of the specified <see cref="Type"/>.</param>
        /// <returns><see langword="true"/> if an instance was instantiated or <see langword="false"/> if an instance could not be instantiated.</returns>
        bool TryCreateInstance(Type type, out object instance);
    }
}