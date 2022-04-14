using DeltaWare.Dependencies.Abstractions.Descriptors;
using System;
using System.Collections.Generic;

namespace DeltaWare.Dependencies.Abstractions
{
    public interface IReadOnlyDependencyCollection
    {
        /// <summary>
        /// Builds a new instance of a <see cref="IDependencyProvider"/>.
        /// </summary>
        /// <returns>Returns an <see cref="IDependencyProvider"/> with an internal scope.</returns>
        /// <remarks>
        /// ALL dependencies will be disposed of along with the <see cref="IDependencyProvider"/>.
        /// Including Singleton instances.
        /// </remarks>
        IDependencyProvider BuildProvider();

        IDependencyScope CreateScope();

        IDependencyDescriptor GetDependencyDescriptor(Type dependencyType);

        IEnumerable<IDependencyDescriptor> GetDependencyDescriptors(Type dependencyType);

        bool HasDependency(Type dependencyType);
    }
}