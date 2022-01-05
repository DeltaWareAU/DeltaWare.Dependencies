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
    }
}