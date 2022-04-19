using DeltaWare.Dependencies.Abstractions.Descriptor;
using DeltaWare.Dependencies.Abstractions.Descriptor.Enums;
using DeltaWare.Dependencies.Extensions;
using System;
using System.Diagnostics;

namespace DeltaWare.Dependencies.Descriptors
{
    [DebuggerDisplay("DefinitionType: {Type.Name} | Lifetime: {Lifetime} - Binding: {Binding}")]
    internal sealed class DependencyInstance : IDependencyInstance
    {
        public IDependencyDescriptor Descriptor { get; }

        /// <inheritdoc cref="IDependencyInstance.Instance"/>
        public object Instance { get; }

        public bool IsDisposable { get; }

        /// <summary>
        /// Creates a new instance of <see cref="DependencyInstance"/>.
        /// </summary>
        /// <param name="instance">The instance of the dependency.</param>
        /// <param name="descriptor">The dependency descriptor of this instance.</param>
        public DependencyInstance(object instance, IDependencyDescriptor descriptor)
        {
            Descriptor = descriptor ?? throw new ArgumentNullException(nameof(descriptor));
            Instance = instance ?? throw new ArgumentNullException(nameof(instance));
            IsDisposable = instance.GetType().HasInterface<IDisposable>();
        }

        #region IDisposable

        private volatile bool _disposed;

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            if (Descriptor.Binding != Binding.Bound)
            {
                return;
            }

            if (_disposed)
            {
                return;
            }

            if (Instance is IDisposable disposableImplementation)
            {
                disposableImplementation.Dispose();
            }

            _disposed = true;

            GC.SuppressFinalize(this);
        }

        #endregion IDisposable
    }
}
