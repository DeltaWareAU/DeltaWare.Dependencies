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

        public object GetDependency(Type definition)
        {
            return _innerProvider.InternalGetDependency(definition, this);
        }

        public object CreateInstance(Type definition)
        {
            return _innerProvider.CreateInstance(definition);
        }

        public bool HasDependency(Type definition)
        {
            return _innerProvider.HasDependency(definition);
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

            return new DependencyProviderCallStack(_innerProvider, childStack);
        }

        #region IDisposable

        private bool _disposed = false;

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _innerProvider?.Dispose();

            _disposed = true;

            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
