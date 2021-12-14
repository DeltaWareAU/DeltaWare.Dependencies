using DeltaWare.Dependencies.Abstractions.Exceptions;
using System;
using System.Collections.Generic;

namespace DeltaWare.Dependencies.Abstractions
{
    /// <summary>
    /// Provides instances of dependency whilst handling their lifetimes.
    /// </summary>
    public interface IDependencyProvider : IDisposable
    {
        /// <summary>
        /// Gets all instances of the specified dependency.
        /// </summary>
        /// <typeparam name="TDependency">The dependency instances to be retrieved.</typeparam>
        /// <remarks>Searches for dependencies based off of their inheritance.</remarks>
        List<TDependency> GetDependencies<TDependency>() where TDependency : class;

        /// <summary>
        /// Gets an instance of the specified dependency.
        /// </summary>
        /// <typeparam name="TDependency">The dependency instance to be retrieved.</typeparam>
        /// <exception cref="DependencyNotFoundException">
        /// Thrown when no instance of the specified dependency can be found.
        /// </exception>
        TDependency GetDependency<TDependency>() where TDependency : class;

        /// <summary>
        /// Returns a <see cref="bool"/> specifying if the dependency was found.
        /// </summary>
        /// <typeparam name="TDependency">The dependency instance to be checked for.</typeparam>
        bool HasDependency<TDependency>() where TDependency : class;

        /// <summary>
        /// Tries to get an instance of the specified dependency. Returns a <see cref="bool"/>
        /// specifying if the dependency was found.
        /// </summary>
        /// <typeparam name="TDependency">The dependency instance to be retrieved.</typeparam>
        /// <param name="dependencyInstance">The retrieved instance of the dependency.</param>
        bool TryGetDependency<TDependency>(out TDependency dependencyInstance) where TDependency : class;

        bool TryGetDependency(Type dependencyType, out object dependencyInstance);
    }
}