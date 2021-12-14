using System;

namespace DeltaWare.Dependencies.Abstractions
{
    /// <summary>
    /// Contains a collection of dependencies.
    /// </summary>
    public interface IDependencyCollection : IDisposable
    {
        void AddDependency<TDependency>(Lifetime lifetime, Binding binding = Binding.Bound);

        /// <summary>
        /// Adds a dependency, if the dependency was previously add it will be overriden.
        /// </summary>
        /// <typeparam name="TDependency">Specifies the dependencies type.</typeparam>
        /// <param name="dependency">Specifies how to instantiate the dependency.</param>
        /// <param name="lifetime">Specifies the lifetime of the dependency.</param>
        /// <param name="binding">Specifies the binding of a dependency.</param>
        /// <exception cref="ArgumentNullException">Thrown when a null value is provided.</exception>
        void AddDependency<TDependency>(Func<TDependency> dependency, Lifetime lifetime, Binding binding = Binding.Bound);

        /// <summary>
        /// Adds a dependency, if the dependency was previously add it will be overriden.
        /// </summary>
        /// <typeparam name="TDependency">Specifies the dependencies type.</typeparam>
        /// <param name="dependency">
        /// Specifies how to instantiate the dependency, including a provider to get existing dependencies.
        /// </param>
        /// <param name="lifetime">Specifies the lifetime of the dependency.</param>
        /// <param name="binding">Specifies the binding of a dependency.</param>
        /// <exception cref="ArgumentNullException">Thrown when a null value is provided.</exception>
        void AddDependency<TDependency>(Func<IDependencyProvider, TDependency> dependency, Lifetime lifetime, Binding binding = Binding.Bound);

        void AddDependency<TDependency, TImplementation>(Lifetime lifetime, Binding binding = Binding.Bound) where TImplementation : TDependency;

        /// <summary>
        /// Builds a <see cref="IDependencyProvider"/>.
        /// </summary>
        IDependencyProvider BuildProvider();

        /// <summary>
        /// Specifies if the dependency has been added.
        /// </summary>
        /// <typeparam name="TDependency">Specifies the type of the dependency.</typeparam>
        bool HasDependency<TDependency>();

        bool HasDependency(Type dependencyType);

        /// <summary>
        /// Adds a dependency, if the dependency was previously added the specified dependency will
        /// not be.
        /// </summary>
        /// <typeparam name="TDependency">Specifies the dependencies type.</typeparam>
        /// <param name="dependency">Specifies how to instantiate the dependency.</param>
        /// <param name="lifetime">Specifies the lifetime of the dependency.</param>
        /// <param name="binding">Specifies the binding of a dependency.</param>
        /// <exception cref="ArgumentNullException">Thrown when a null value is provided.</exception>
        bool TryAddDependency<TDependency>(Func<TDependency> dependency, Lifetime lifetime, Binding binding = Binding.Bound);

        /// <summary>
        /// Adds a dependency, if the dependency was previously added the specified dependency will
        /// not be.
        /// </summary>
        /// <typeparam name="TDependency">Specifies the dependencies type.</typeparam>
        /// <param name="dependency">
        /// Specifies how to instantiate the dependency, including a provider to get existing dependencies.
        /// </param>
        /// <param name="lifetime">Specifies the lifetime of the dependency.</param>
        /// <param name="binding">Specifies the binding of a dependency.</param>
        /// <exception cref="ArgumentNullException">Thrown when a null value is provided.</exception>
        bool TryAddDependency<TDependency>(Func<IDependencyProvider, TDependency> dependency, Lifetime lifetime, Binding binding = Binding.Bound);

        bool TryAddDependency<TDependency, TImplementation>(Lifetime lifetime, Binding binding = Binding.Bound) where TImplementation : TDependency;

        bool TryAddDependency<TDependency>(Lifetime lifetime, Binding binding = Binding.Bound);
    }
}