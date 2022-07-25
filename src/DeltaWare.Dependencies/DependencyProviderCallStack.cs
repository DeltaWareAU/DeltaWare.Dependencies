using DeltaWare.Dependencies.Abstractions.Descriptor;
using DeltaWare.Dependencies.Stack;
using System;

namespace DeltaWare.Dependencies
{
    internal class DependencyProviderCallStack : DependencyProvider
    {
        private readonly DependencyStack _dependencyStack;

        public DependencyProviderCallStack(LifetimeScope lifetimeScope, IDependencyDescriptor descriptor) : base(lifetimeScope)
        {
            _dependencyStack = new DependencyStack(descriptor ?? throw new ArgumentNullException(nameof(descriptor)));
        }

        private DependencyProviderCallStack(LifetimeScope lifetimeScope, DependencyStack dependencyStack) : base(lifetimeScope)
        {
            _dependencyStack = dependencyStack ?? throw new ArgumentNullException(nameof(dependencyStack));
        }

        public override object GetDependency(Type definition)
        {
            return InternalGetDependency(definition, this);
        }

        public override bool TryGetDependency(Type definition, out object instance)
        {
            instance = InternalGetDependency(definition, this);

            return instance != null;
        }

        internal DependencyProviderCallStack CreateChild(IDependencyDescriptor descriptor)
        {
            if (descriptor == null)
            {
                throw new ArgumentNullException(nameof(descriptor));
            }

            DependencyStack childStack = _dependencyStack.CreateChild(descriptor);

            childStack.EnsureParentNotSingleton();
            childStack.EnsureNoCircularDependencies();

            return new DependencyProviderCallStack(LifetimeScope, childStack);
        }
    }
}
