using DeltaWare.Dependencies.Abstractions;
using System;

namespace DeltaWare.Dependencies.Types
{
    /// <inheritdoc cref="IDependencyDescriptor"/>
    public class ActionDescriptor<TDependency> : IDependencyDescriptor, ICloneable
    {
        private readonly Func<TDependency> _dependency;

        private readonly Func<IDependencyProvider, TDependency> _providerDependency;

        /// <inheritdoc cref="IDependencyDescriptor.Binding"/>
        public Binding Binding { get; }

        /// <inheritdoc cref="IDependencyDescriptor.Lifetime"/>
        public Lifetime Lifetime { get; }

        /// <inheritdoc cref="IDependencyDescriptor.Type"/>
        public Type Type { get; } = typeof(TDependency);

        /// <summary>
        /// Creates a new instance of <see cref="ActionDescriptor{TDependency}"/>.
        /// </summary>
        /// <param name="dependency">Specifies how to instantiate the dependency.</param>
        /// <param name="lifetime">Specifies the lifetime of the dependency.</param>
        /// <param name="binding">Specifies the binding of a dependency.</param>
        /// <exception cref="ArgumentNullException">Thrown when a null value is provided.</exception>
        public ActionDescriptor(Func<TDependency> dependency, Lifetime lifetime, Binding binding = Binding.Bound)
        {
            _dependency = dependency ?? throw new ArgumentNullException(nameof(dependency));
            Binding = binding;
            Lifetime = lifetime;
        }

        /// <summary>
        /// Creates a new instance of <see cref="ActionDescriptor{TDependency}"/>.
        /// </summary>
        /// <param name="dependency">
        /// Specifies how to instantiate the dependency, including a provider to get existing dependencies.
        /// </param>
        /// <param name="lifetime">Specifies the lifetime of the dependency.</param>
        /// <param name="binding">Specifies the binding of a dependency.</param>
        /// <exception cref="ArgumentNullException">Thrown when a null value is provided.</exception>
        public ActionDescriptor(Func<IDependencyProvider, TDependency> dependency, Lifetime lifetime, Binding binding = Binding.Bound)
        {
            _providerDependency = dependency ?? throw new ArgumentNullException(nameof(dependency));
            Binding = binding;
            Lifetime = lifetime;
        }

        public object Clone()
        {
            if (_dependency != null)
            {
                return new ActionDescriptor<TDependency>(_dependency, Lifetime, Binding);
            }

            return new ActionDescriptor<TDependency>(_providerDependency, Lifetime, Binding);
        }

        /// <inheritdoc cref="IDependencyDescriptor.CreateInstance"/>
        public IDependencyInstance CreateInstance(IDependencyProvider provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            IDependencyInstance instance;

            if (_dependency != null)
            {
                instance = new DependencyInstance(_dependency.Invoke(), Type, Lifetime, Binding);
            }
            else if (_providerDependency != null)
            {
                instance = new DependencyInstance(_providerDependency.Invoke(provider), Type, Lifetime, Binding);
            }
            else
            {
                throw new NullReferenceException("No instance could be found.");
            }

            return instance;
        }
    }
}