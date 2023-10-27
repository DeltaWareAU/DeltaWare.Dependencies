using DeltaWare.Dependencies.Abstractions;
using DeltaWare.Dependencies.Abstractions.Descriptor;
using DeltaWare.Dependencies.Abstractions.Descriptor.Enums;
using DeltaWare.Dependencies.Abstractions.Resolver;
using DeltaWare.Dependencies.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeltaWare.Dependencies
{
    internal class LifetimeScope : ILifetimeScope
    {
        public LifetimeScope ParentScope { get; }

        private readonly List<LifetimeScope> _childScopes = new List<LifetimeScope>();

        private readonly List<IDisposable> _disposables = new List<IDisposable>();

        private readonly List<IDependencyInstance> _scopedInstances = new List<IDependencyInstance>();

        public IDependencyResolver Resolver { get; }

        public LifetimeScope(IDependencyResolver dependencyResolver)
        {
            Resolver = new LifetimeScopeResolver(this, dependencyResolver);
        }

        private LifetimeScope(IDependencyResolver dependencyResolver, LifetimeScope parentScope) : this(dependencyResolver)
        {
            ParentScope = parentScope ?? throw new ArgumentNullException(nameof(parentScope));
        }

        public IDependencyProvider BuildProvider()
        {
            var scope = InternalCreateScope();

            var provider = new DependencyProvider(scope);

            return provider;
        }

        public ILifetimeScope CreateScope()
        {
            return InternalCreateScope();
        }

        private LifetimeScope InternalCreateScope()
        {
            var childScope = new LifetimeScope(Resolver, this);

            _childScopes.Add(childScope);

            return childScope;
        }

        public bool TryGetInstance(IDependencyDescriptor descriptor, out IDependencyInstance instance)
        {
            if (ParentScope != null && descriptor.Lifetime == Lifetime.Singleton)
            {
                return ParentScope.TryGetInstance(descriptor, out instance);
            }

            instance = _scopedInstances.FirstOrDefault(i => i.Descriptor == descriptor);

            return instance != null;
        }

        public void RegisterInstance(IDependencyInstance instance)
        {
            if (ParentScope != null && instance.Descriptor.Lifetime == Lifetime.Singleton)
            {
                ParentScope.RegisterInstance(instance);

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
