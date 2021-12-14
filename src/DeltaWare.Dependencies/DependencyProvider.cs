using DeltaWare.Dependencies.Abstractions;
using System;
using System.Collections.Generic;

namespace DeltaWare.Dependencies
{
    /// <inheritdoc cref="IDependencyProvider"/>
    internal class DependencyProvider : IDependencyProvider
    {
        private readonly List<object> _disposableDependencies = new();
        private readonly Dictionary<Type, IDependencyInstance> _scopedInstances = new();

        private readonly object _scopeLock = new();

        private readonly DependencyCollection _sourceCollection;

        public DependencyProvider(DependencyCollection sourceCollection)
        {
            _sourceCollection = sourceCollection ?? throw new ArgumentNullException(nameof(sourceCollection));

            sourceCollection.AddScoped<IDependencyProvider>(() => this, Binding.Unbound);
        }

        /// <inheritdoc cref="IDependencyProvider.GetDependencies{TDependency}"/>
        public List<TDependency> GetDependencies<TDependency>() where TDependency : class
        {
            // Get all registered dependencies that inherit the specified type.
            List<TDependency> dependencies = new List<TDependency>();

            foreach (IDependencyDescriptor descriptor in _sourceCollection.GetDependencyDescriptors<TDependency>())
            {
                dependencies.Add((TDependency)GetDependency(descriptor));
            }

            return dependencies;
        }

        /// <inheritdoc cref="IDependencyProvider.GetDependency{TDependency}"/>
        public TDependency GetDependency<TDependency>() where TDependency : class
        {
            return (TDependency)GetDependency(typeof(TDependency));
        }

        public object GetDependency(Type dependencyType)
        {
            IDependencyDescriptor descriptor = _sourceCollection.GetDependencyDescriptor(dependencyType);

            return GetDependency(descriptor);
        }

        public object GetDependency(IDependencyDescriptor descriptor)
        {
            if (descriptor.Lifetime == Lifetime.Singleton)
            {
                return _sourceCollection.GetSingletonInstance(descriptor, this).Instance;
            }

            lock (_scopeLock)
            {
                if (_scopedInstances.TryGetValue(descriptor.Type, out IDependencyInstance instance))
                {
                    return instance.Instance;
                }

                instance = descriptor.CreateInstance(this);

                // Only scoped are tracked by the Provider so they are added. Transient are not
                // tracked at all, unless they are disposable.
                if (instance.Lifetime == Lifetime.Scoped)
                {
                    _scopedInstances.Add(descriptor.Type, instance);
                }

                if (instance.Binding == Binding.Bound && instance.IsDisposable)
                {
                    // Track all bound disposable dependencies.
                    _disposableDependencies.Add(instance);
                }

                return instance.Instance;
            }
        }

        /// <inheritdoc cref="IDependencyProvider.HasDependency{TDependency}"/>
        public bool HasDependency<TDependency>() where TDependency : class
        {
            return HasDependency(typeof(TDependency));
        }

        public bool HasDependency(Type dependencyType)
        {
            return _sourceCollection.HasDependency(dependencyType);
        }

        /// <inheritdoc cref="IDependencyProvider.TryGetDependency{TDependency}"/>
        public bool TryGetDependency<TDependency>(out TDependency dependencyInstance) where TDependency : class
        {
            bool found = TryGetDependency(typeof(TDependency), out object instance);

            dependencyInstance = (TDependency)instance;

            return found;
        }

        public bool TryGetDependency(Type dependencyType, out object dependencyInstance)
        {
            if (HasDependency(dependencyType))
            {
                dependencyInstance = GetDependency(dependencyType);

                return true;
            }

            dependencyInstance = default;

            return false;
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
        /// Disposes all bound instances of scoped dependencies.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                foreach (IDisposable disposable in _disposableDependencies)
                {
                    disposable.Dispose();
                }
            }

            _disposed = true;
        }

        #endregion IDisposable
    }
}