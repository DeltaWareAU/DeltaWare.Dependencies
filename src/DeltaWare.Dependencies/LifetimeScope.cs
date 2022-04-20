using DeltaWare.Dependencies.Abstractions;
using DeltaWare.Dependencies.Abstractions.Descriptor;
using DeltaWare.Dependencies.Abstractions.Descriptor.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeltaWare.Dependencies
{
    internal class LifetimeScope : ILifetimeScope
    {
        private readonly LifetimeScope _parentScope;

        private readonly List<LifetimeScope> _childScopes = new();

        private readonly List<IDisposable> _disposables = new();

        private readonly List<IDependencyInstance> _scopedInstances = new();

        private readonly IDependencyResolver _dependencyResolver;

        public LifetimeScope(IDependencyResolver dependencyResolver)
        {
            _dependencyResolver = dependencyResolver ?? throw new ArgumentNullException(nameof(dependencyResolver));
        }

        private LifetimeScope(IDependencyResolver dependencyResolver, LifetimeScope parentScope) : this(dependencyResolver)
        {
            _parentScope = parentScope ?? throw new ArgumentNullException(nameof(parentScope));
        }

        public IDependencyProvider BuildProvider()
        {
            return new DependencyProvider(_dependencyResolver, (LifetimeScope)CreateScope());
        }

        public ILifetimeScope CreateScope()
        {
            var childScope = new LifetimeScope(_dependencyResolver, this);

            _childScopes.Add(childScope);

            return childScope;
        }

        public bool TryGetInstance(IDependencyDescriptor descriptor, out IDependencyInstance instance)
        {
            if (_parentScope != null && descriptor.Lifetime == Lifetime.Singleton)
            {
                return TryGetInstance(descriptor, out instance);
            }

            instance = _scopedInstances.FirstOrDefault(i => i.Descriptor == descriptor);

            return instance != null;
        }

        public void RegisterInstance(IDependencyInstance instance)
        {
            if (_parentScope != null && instance.Descriptor.Lifetime == Lifetime.Singleton)
            {
                _parentScope.RegisterInstance(instance);

                return;
            }

            if (instance.Descriptor.Lifetime != Lifetime.Transient)
            {
                _scopedInstances.Add(instance);
            }

            if (instance.IsDisposable && instance.Descriptor.Binding == Binding.Bound)
            {
                _disposables.Add(instance);
            }
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

            foreach (IDisposable instance in _disposables)
            {
                instance.Dispose();
            }

            foreach (LifetimeScope childScope in _childScopes)
            {
                childScope.Dispose();
            }

            _disposed = true;

            GC.SuppressFinalize(this);
        }


        #endregion IDisposable
    }
}
