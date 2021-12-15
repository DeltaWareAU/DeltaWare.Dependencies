using System;
using System.Collections.Generic;

namespace DeltaWare.Dependencies.Abstractions.Exceptions
{
    public class CircularDependencyException : Exception
    {
        public IReadOnlyList<IDependencyDescriptor> DependencyStack { get; }

        public CircularDependencyException(List<IDependencyDescriptor> dependencyStack, IDependencyDescriptor descriptor) : base($"Circular dependencies found for {descriptor.Type.Name}")
        {
            dependencyStack.Add(descriptor);

            DependencyStack = dependencyStack;
        }
    }
}