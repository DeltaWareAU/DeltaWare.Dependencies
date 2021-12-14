using System;
using System.Collections.Generic;

namespace DeltaWare.Dependencies.Abstractions
{
    public interface IReadOnlyDependencyCollection : IDisposable
    {
        /// <summary>
        /// Builds a <see cref="IDependencyProvider"/>.
        /// </summary>
        IDependencyProvider BuildProvider();

        IDependencyDescriptor GetDependencyDescriptor<TDependency>() where TDependency : class;

        IEnumerable<IDependencyDescriptor> GetDependencyDescriptors<TDependency>() where TDependency : class;

        /// <summary>
        /// Specifies if the dependency has been added.
        /// </summary>
        /// <typeparam name="TDependency">Specifies the type of the dependency.</typeparam>
        bool HasDependency<TDependency>() where TDependency : class;
    }
}