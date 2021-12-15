using DeltaWare.Dependencies.Abstractions;
using System;
using System.Collections.Generic;

namespace DeltaWare.Dependencies
{
    internal class DependencyScope : IDependencyScope
    {
        private readonly Dictionary<Type, IDependencyInstance> _scopedInstances = new();

        private readonly List<IDisposable> _disposableInstances = new();

        private readonly List<IDisposable> _childScopes = new();

        private readonly IReadOnlyDependencyCollection _sourceCollection;

        public DependencyScope(IReadOnlyDependencyCollection sourceCollection)
        {
            _sourceCollection = sourceCollection;
        }

        public void RegisterInstance(IDependencyInstance instance)
        {
            if (instance.Lifetime != Lifetime.Singleton)
            {
                throw new ArgumentException("A DependencyScope can only register Singletons", nameof(instance));
            }

            _scopedInstances.Add(instance.Type, instance);

            if (instance.IsDisposable && instance.Binding == Binding.Bound)
            {
                _disposableInstances.Add((IDisposable)instance.Instance);
            }
        }

        public bool TryGetInstance(IDependencyDescriptor descriptor, out IDependencyInstance instance)
        {
            return _scopedInstances.TryGetValue(descriptor.Type, out instance);
        }

        #region IDisposable

        private volatile bool _disposed;

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposed all scoped instances.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                foreach (IDisposable disposable in _childScopes)
                {
                    disposable.Dispose();
                }

                foreach (IDisposable disposable in _disposableInstances)
                {
                    disposable.Dispose();
                }
            }

            _disposed = true;
        }

        #endregion IDisposable

        public IDependencyProvider BuildProvider()
        {
            DependencyProvider provider = new DependencyProvider(this, _sourceCollection);

            _childScopes.Add(provider);

            return provider;
        }

        public IDependencyScope CreateScope()
        {
            DependencyScope scope = new DependencyScope(_sourceCollection);

            _childScopes.Add(scope);

            return scope;
        }
    }
}