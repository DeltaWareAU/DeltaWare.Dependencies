using DeltaWare.Dependencies.Abstractions;
using DeltaWare.Dependencies.Abstractions.Descriptor;
using DeltaWare.Dependencies.Abstractions.Exceptions;
using DeltaWare.Dependencies.Descriptors;
using System;

namespace DeltaWare.Dependencies
{
    internal class DependencyProvider : IDependencyProvider, IDisposable
    {
        private readonly IDependencyResolver _dependencyResolver;

        private readonly LifetimeScope _providerScope;

        private readonly object _concurrencyLock = new();

        public DependencyProvider(IDependencyResolver dependencyResolver, LifetimeScope providerScope)
        {
            _dependencyResolver = dependencyResolver ?? throw new ArgumentNullException(nameof(dependencyResolver));
            _providerScope = providerScope ?? throw new ArgumentNullException(nameof(providerScope));
        }

        public object GetDependency(Type type)
        {
            lock (_concurrencyLock)
            {
                return InternalGetDependency(type);
            }
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

            if (_providerScope.TryGetInstance(dependency, out IDependencyInstance instance))
            {
                return instance.Instance;
            }

            instance = dependency.CreateInstance(providerCallStack);

            if (instance == null)
            {
                throw NullDependencyInstanceException.NullInstance(type);
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
