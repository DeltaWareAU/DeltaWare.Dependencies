using System;

namespace DeltaWare.Dependencies.Abstractions.Exceptions
{
    public class NullDependencyInstanceException : Exception
    {
        public NullDependencyInstanceException(Type type) : base($"The instance for the dependency {type.Name} could not be created")
        {
        }
    }
}