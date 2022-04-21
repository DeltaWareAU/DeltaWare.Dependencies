using DeltaWare.Dependencies.Abstractions;
using DeltaWare.Dependencies.Abstractions.Descriptor;
using DeltaWare.Dependencies.Abstractions.Exceptions;
using DeltaWare.Dependencies.Descriptors;
using DeltaWare.Dependencies.Resolver;
using System;

namespace DeltaWare.Dependencies
{
    internal class DependencyProvider : IDependencyProvider
    {
        private readonly IDependencyResolver _dependencyResolver;

        private readonly LifetimeScope _providerScope;

        private readonly object _concurrencyLock = new();

        public DependencyProvider(LifetimeScope providerScope)
        {
            _providerScope = providerScope ?? throw new ArgumentNullException(nameof(providerScope));
            _dependencyResolver = new DependencyProviderResolver(this, providerScope.Resolver);
        }

        public object GetDependency(Type definition)
        {
            lock (_concurrencyLock)
            {
                return InternalGetDependency(definition);
            }
        }

        public object CreateInstance(Type definition)
        {
            lock (_concurrencyLock)
            {
                IDependencyDescriptor descriptor = _dependencyResolver.GetDependency(definition) ?? new TypeDependencyDescriptor(definition);

                return InternalGetDependency(descriptor);
            }
        }

        public bool HasDependency(Type definition)
        {
            return _dependencyResolver.HasDependency(definition);
        }

        internal object InternalGetDependency(Type type, DependencyProviderCallStack providerCallStack = null)
        {
            IDependencyDescriptor dependency = _dependencyResolver.GetDependency(type);

            if (dependency == null)
            {
                return null;
            }

            return InternalGetDependency(dependency, providerCallStack);
        }
        internal object InternalGetDependency(IDependencyDescriptor dependency, DependencyProviderCallStack providerCallStack = null)
        {
            if (providerCallStack == null)
            {
                providerCallStack = new DependencyProviderCallStack(this, dependency);
            }
            else
            {
                providerCallStack = providerCallStack.CreateChild(dependency);
            }

            if (_providerScope.TryGetInstance(dependency, out IDependencyInstance instance))
            {
                return instance.Instance;
            }

            instance = dependency.CreateInstance(providerCallStack);

            if (instance == null)
            {
                throw NullDependencyInstanceException.NullInstance(dependency.ImplementationType);
            }

            _providerScope.RegisterInstance(instance);

            return instance.Instance;
        }

        #region IDisposable

        private volatile bool _disposed;

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _providerScope.Dispose();

            _disposed = true;

            GC.SuppressFinalize(this);
        }


        #endregion IDisposable
    }
}
