using System;

namespace DeltaWare.Dependencies.Abstractions.Exceptions
{
    public class SingletonDependencyException : Exception
    {
        public SingletonDependencyException(Type type) : base($"The dependency {type.Name} is a Singleton. Singletons can only use dependencies that are also a singleton.")
        {
        }
    }
}