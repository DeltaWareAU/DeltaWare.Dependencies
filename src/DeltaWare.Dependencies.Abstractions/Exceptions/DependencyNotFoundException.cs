using System;

namespace DeltaWare.Dependencies.Abstractions.Exceptions
{
    /// <summary>
    /// Represents that a dependency could not be found.
    /// </summary>
    public class DependencyNotFoundException : Exception
    {
        /// <summary>
        /// Creates a new instance of <see cref="DependencyNotFoundException"/>.
        /// </summary>
        /// <param name="type">Specifies the dependency type that could not be found.</param>
        public DependencyNotFoundException(Type type) : base($"The specified dependency of type {type.Name} could not be found")
        {
        }
    }
}