using DeltaWare.Dependencies.Abstractions;
using DeltaWare.Dependencies.Abstractions.Stack;
using System;
using System.Collections.Generic;

namespace DeltaWare.Dependencies.Stack
{
    internal class DependencyStack : IStack<IDependencyDescriptor>
    {
        public List<IStack<IDependencyDescriptor>> Children { get; } = new();
        public IStack<IDependencyDescriptor> ParentStack { get; }
        public IDependencyDescriptor Value { get; }

        public DependencyStack(IDependencyDescriptor value)
        {
            Value = value;
        }

        private DependencyStack(IDependencyDescriptor value, IStack<IDependencyDescriptor> parent)
        {
            Value = value;
            ParentStack = parent ?? throw new ArgumentNullException(nameof(parent));
        }

        public IStack<IDependencyDescriptor> CreateChild(IDependencyDescriptor value)
        {
            DependencyStack childNode = new DependencyStack(value, this);

            Children.Add(childNode);

            return childNode;
        }
    }
}