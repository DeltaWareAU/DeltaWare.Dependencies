using DeltaWare.Dependencies.Abstractions.Stack;
using System;

namespace DeltaWare.Dependencies.Abstractions.Exceptions
{
    public class CircularDependencyException : Exception
    {
        public IStack<IDependencyDescriptor> DependencyStack { get; }

        public CircularDependencyException(IStack<IDependencyDescriptor> dependencyStack) : base($"Circular dependencies found for {dependencyStack.Value.Type.Name}")
        {
            DependencyStack = dependencyStack;
        }
    }
}