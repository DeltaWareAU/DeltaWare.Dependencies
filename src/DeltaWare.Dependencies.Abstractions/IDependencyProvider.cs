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

        IEnumerable<TDependency> GetDependencies<TDependency>() where TDependency : class;

        TDependency GetDependency<TDependency>() where TDependency : class;

        bool HasDependency<TDependency>() where TDependency : class;

        bool TryGetDependencies<TDependency>(out IEnumerable<TDependency> instances) where TDependency : class;

        bool TryGetDependency<TDependency>(out TDependency instance) where TDependency : class;
    }
}