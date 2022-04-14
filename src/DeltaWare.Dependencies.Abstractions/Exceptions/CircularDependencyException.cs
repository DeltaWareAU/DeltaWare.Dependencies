using DeltaWare.Dependencies.Abstractions.Descriptors;
using DeltaWare.SDK.Core.Collections;
using System;

namespace DeltaWare.Dependencies.Abstractions.Exceptions
{
    public class CircularDependencyException : Exception
    {
        public ITreeNode<IDependencyDescriptor> DependencyCallStack { get; }

        public CircularDependencyException(ITreeNode<IDependencyDescriptor> dependencyCallStack) : base($"Circular dependencies found for {dependencyCallStack.Value.Type.Name}")
        {
            DependencyCallStack = dependencyCallStack;
        }
    }
}