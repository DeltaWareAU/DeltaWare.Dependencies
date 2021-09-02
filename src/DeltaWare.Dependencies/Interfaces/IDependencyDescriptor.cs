using DeltaWare.Dependencies.Abstractions;
using System;

namespace DeltaWare.Dependencies.Interfaces
{
    /// <summary>
    /// Describes a dependency.
    /// </summary>
    public interface IDependencyDescriptor
    {
        /// <summary>
        /// Specifies the <see cref="Binding"/> of the <see cref="IDependencyDescriptor"/>.
        /// </summary>
        Binding Binding { get; }

        /// <summary>
        /// Specifies the <see cref="Lifetime"/> of the <see cref="IDependencyDescriptor"/>.
        /// </summary>
        Lifetime Lifetime { get; }

        /// <summary>
        /// Specified the <see cref="Type"/> of the <see cref="IDependencyDescriptor"/>,
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// Gets an instance of the dependency.
        /// </summary>
        /// <param name="provider">Specifies the provider used to instantiate the dependency.</param>
        /// <exception cref="ArgumentNullException">Thrown when a null value is provided.</exception>
        /// <exception cref="NullReferenceException">Thrown when no instance could be found.</exception>
        IDependencyInstance CreateInstance(IDependencyProvider provider);
    }
}