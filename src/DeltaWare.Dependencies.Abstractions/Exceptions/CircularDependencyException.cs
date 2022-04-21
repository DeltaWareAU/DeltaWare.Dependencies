using DeltaWare.Dependencies.Abstractions.Descriptor;
using System;

namespace DeltaWare.Dependencies.Abstractions.Exceptions
{
    public class CircularDependencyException : Exception
    {
        public CircularDependencyException(IDependencyDescriptor type) : base($"Circular dependencies found for {type.ImplementationType.Name}")
        {
        }
    }
}
