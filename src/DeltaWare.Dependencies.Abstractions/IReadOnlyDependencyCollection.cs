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

        IDependencyDescriptor GetDependencyDescriptor<TDependency>() where TDependency : class;

        IDependencyDescriptor GetDependencyDescriptor(Type dependencyType);

        IEnumerable<IDependencyDescriptor> GetDependencyDescriptors<TDependency>() where TDependency : class;

        /// <summary>
        /// Specifies if the dependency has been added.
        /// </summary>
        /// <typeparam name="TDependency">Specifies the type of the dependency.</typeparam>
        bool HasDependency<TDependency>() where TDependency : class;

        bool HasDependency(Type dependencyType);
    }
}