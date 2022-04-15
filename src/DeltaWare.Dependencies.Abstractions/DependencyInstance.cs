using DeltaWare.Dependencies.Abstractions.Descriptors.Enums;
using System;
using System.Diagnostics;
using System.Linq;

namespace DeltaWare.Dependencies.Abstractions
{
    /// <inheritdoc cref="IDependencyInstance"/>
    [DebuggerDisplay("Type: {Type.Name} | Lifetime: {Lifetime} - Binding: {Binding}")]
    internal class DependencyInstance : IDependencyInstance
    {
        /// <inheritdoc cref="IDependencyInstance.Binding"/>
        public Binding Binding { get; }

        /// <inheritdoc cref="IDependencyInstance.Instance"/>
        public object Instance { get; }

        public bool IsDisposable { get; }

        /// <inheritdoc cref="IDependencyInstance.Lifetime"/>
        public Lifetime Lifetime { get; }

        /// <inheritdoc cref="IDependencyInstance.Type"/>
        public Type Type { get; }

        /// <summary>
        /// Creates a new instance of <see cref="DependencyInstance"/>.
        /// </summary>
        /// <param name="instance">The instance of the dependency.</param>
        /// <param name="type">Specifies the type of the dependency.</param>
        /// <param name="lifetime">Specifies the lifetime of the dependency.</param>
        /// <param name="binding">Specifies the binding on the dependency.</param>
        public DependencyInstance(object instance, Type type, Lifetime lifetime, Binding binding)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Instance = instance ?? throw new ArgumentNullException(nameof(instance));
            Binding = binding;
            Lifetime = lifetime;
            IsDisposable = instance.GetType().GetInterfaces().Contains(typeof(IDisposable));
        }

        #region IDisposable

        private volatile bool _disposed;

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            if (Binding != Binding.Bound)
            {
                return;
            }

            Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the current instance of the dependency.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing && Instance is IDisposable disposableImplementation)
            {
                disposableImplementation.Dispose();
            }

            _disposed = true;
        }

        #endregion IDisposable
    }
}
