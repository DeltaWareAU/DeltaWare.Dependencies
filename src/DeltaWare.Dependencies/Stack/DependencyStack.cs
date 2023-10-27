using DeltaWare.Dependencies.Abstractions.Descriptor;
using DeltaWare.Dependencies.Abstractions.Descriptor.Enums;
using DeltaWare.Dependencies.Abstractions.Exceptions;
using System;
using System.Collections.Generic;

namespace DeltaWare.Dependencies.Stack
{
    internal class DependencyStack
    {
        public List<DependencyStack> Children { get; } = new List<DependencyStack>();

        public DependencyStack ParentStack { get; }

        public IDependencyDescriptor Value { get; }

        public DependencyStack(IDependencyDescriptor value)
        {
            Value = value;
        }

        private DependencyStack(IDependencyDescriptor value, DependencyStack parent)
        {
            Value = value;
            ParentStack = parent ?? throw new ArgumentNullException(nameof(parent));
        }

        public DependencyStack CreateChild(IDependencyDescriptor value)
        {
            DependencyStack childNode = new DependencyStack(value, this);

            Children.Add(childNode);

            return childNode;
        }

        public void EnsureParentNotSingleton()
        {
            if (Value.Lifetime == Lifetime.Singleton || ParentStack == null)
            {
                return;
            }

            if (ParentStack.Value.Lifetime == Lifetime.Singleton)
            {
                throw new SingletonDependencyException(ParentStack.Value.ImplementationType);
            }
        }

        public void EnsureNoCircularDependencies()
        {
            DependencyStack dependencyStack = this;

            while (dependencyStack.ParentStack != null)
            {
                dependencyStack = dependencyStack.ParentStack;

                if (dependencyStack.Value == Value)
                {
                    throw new CircularDependencyException(Value);
                }
            }
        }
    }
}
