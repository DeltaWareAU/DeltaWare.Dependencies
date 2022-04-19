using DeltaWare.Dependencies.Abstractions;
using DeltaWare.Dependencies.Abstractions.Descriptor;
using DeltaWare.Dependencies.Stack;
using System;

namespace DeltaWare.Dependencies.Descriptors
{
    internal class DependencyProviderCallStack : IDependencyProvider
    {
        private readonly DependencyProvider _innerProvider;

        private readonly DependencyStack _dependencyStack;

        public DependencyProviderCallStack(DependencyProvider innerProvider, IDependencyDescriptor descriptor)
        {
            _innerProvider = innerProvider ?? throw new ArgumentNullException(nameof(innerProvider));
            _dependencyStack = new DependencyStack(descriptor ?? throw new ArgumentNullException(nameof(descriptor)));
        }

        private DependencyProviderCallStack(DependencyProvider innerProvider, DependencyStack dependencyStack)
        {
            _innerProvider = innerProvider ?? throw new ArgumentNullException(nameof(innerProvider));
            _dependencyStack = dependencyStack ?? throw new ArgumentNullException(nameof(dependencyStack));
        }

        public object GetDependency(Type type)
        {
            return _innerProvider.InternalGetDependency(type);
        }

        public ILifetimeScope CreateScope()
        {
            return _innerProvider.CreateScope();
        }

        public DependencyProviderCallStack CreateChild(IDependencyDescriptor descriptor)
        {
            if (descriptor == null)
            {
                throw new ArgumentNullException(nameof(descriptor));
            }

            DependencyStack childStack = _dependencyStack.CreateChild(descriptor);

            childStack.EnsureNoCircularDependencies();

            return new DependencyProviderCallStack(_innerProvider, childStack);
        }
    }
}
