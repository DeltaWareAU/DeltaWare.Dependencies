using DeltaWare.Dependencies.Abstractions;
using DeltaWare.Dependencies.Abstractions.Descriptor;
using DeltaWare.Dependencies.Abstractions.Exceptions;
using DeltaWare.Dependencies.Descriptors;
using System;

namespace DeltaWare.Dependencies
{
    internal sealed class DependencyProvider : IDependencyProvider
    {
        private readonly IDependencyResolver _dependencyResolver;

        private readonly LifetimeScope _lifetimeScope;

        public DependencyProvider(IDependencyResolver dependencyResolver, LifetimeScope lifetimeScope)
        {
            _dependencyResolver = dependencyResolver ?? throw new ArgumentNullException(nameof(dependencyResolver));
            _lifetimeScope = lifetimeScope ?? throw new ArgumentNullException(nameof(lifetimeScope));
        }

        public object GetDependency(Type type)
        {
            return InternalGetDependency(type);
        }

        public ILifetimeScope CreateScope()
        {
            return _lifetimeScope.CreateScope();
        }

        internal object InternalGetDependency(Type type, DependencyProviderCallStack providerCallStack = null)
        {
            IDependencyDescriptor dependency = _dependencyResolver.GetDependency(type);

            if (dependency == null)
            {
                return null;
            }

            if (providerCallStack == null)
            {
                providerCallStack = new DependencyProviderCallStack(this, dependency);
            }
            else
            {
                providerCallStack = providerCallStack.CreateChild(dependency);
            }

            if (_lifetimeScope.TryGetInstance(dependency, out IDependencyInstance instance))
            {
                return instance.Instance;
            }

            instance = dependency.CreateInstance(providerCallStack);

            if (instance == null)
            {
                throw NullDependencyInstanceException.NullInstance(type);
            }

            _lifetimeScope.RegisterInstance(instance);

            return instance.Instance;
        }
    }
}
