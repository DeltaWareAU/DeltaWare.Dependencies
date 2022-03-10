using System;

namespace DeltaWare.Dependencies.Abstractions.Exceptions
{
    /// <summary>
    /// Thrown when a dependency that does not have a lifetime of Singleton requires a dependency for instantiation that has a lifetime of Singleton.
    /// </summary>
    /// <remarks>This can be alleviated by using the <see cref="IDependencyProvider"/>.</remarks>
    public class SingletonDependencyException : Exception
    {
        public SingletonDependencyException(Type type) : base($"The dependency {type.Name} is a Singleton. Singletons can only use dependencies that are also a singleton.")
        {
        }
    }
}