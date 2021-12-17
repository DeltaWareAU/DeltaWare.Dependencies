using DeltaWare.Dependencies.Abstractions.Enums;
using System;

namespace DeltaWare.Dependencies.Abstractions
{
    /// <summary>
    /// Contains a collection of dependencies.
    /// </summary>
    public interface IDependencyCollection : IReadOnlyDependencyCollection
    {
        IDependencyDescriptor AddDependency<TDependency>(Lifetime lifetime, Binding binding) where TDependency : class;

        /// <summary>
        /// Adds a dependency, if the dependency was previously add it will be overriden.
        /// </summary>
        /// <typeparam name="TDependency">Specifies the dependencies type.</typeparam>
        /// <param name="dependency">Specifies how to instantiate the dependency.</param>
        /// <param name="lifetime">Specifies the lifetime of the dependency.</param>
        /// <param name="binding">Specifies the binding of a dependency.</param>
        /// <exception cref="ArgumentNullException">Thrown when a null value is provided.</exception>
        IDependencyDescriptor AddDependency<TDependency>(Func<TDependency> dependency, Lifetime lifetime, Binding binding) where TDependency : class;

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
        IDependencyDescriptor AddDependency<TDependency>(Func<IDependencyProvider, TDependency> dependency, Lifetime lifetime, Binding binding) where TDependency : class;

        IDependencyDescriptor AddDependency<TDependency, TImplementation>(Lifetime lifetime, Binding binding) where TImplementation : TDependency where TDependency : class;

        void AddDependency(IDependencyDescriptor dependencyDescriptor);

        void Configure<TDependency>(Action<TDependency> configuration) where TDependency : class;

        void Configure<TDependency>(Action<IDependencyProvider, TDependency> configuration) where TDependency : class;

        bool Remove<TDependency>() where TDependency : class;

        bool TryAddDependency(IDependencyDescriptor dependencyDescriptor);

        /// <summary>
        /// Adds a dependency, if the dependency was previously added the specified dependency will
        /// not be.
        /// </summary>
        /// <typeparam name="TDependency">Specifies the dependencies type.</typeparam>
        /// <param name="dependency">Specifies how to instantiate the dependency.</param>
        /// <param name="lifetime">Specifies the lifetime of the dependency.</param>
        /// <param name="binding">Specifies the binding of a dependency.</param>
        /// <exception cref="ArgumentNullException">Thrown when a null value is provided.</exception>
        bool TryAddDependency<TDependency>(Func<TDependency> dependency, Lifetime lifetime, Binding binding, out IDependencyDescriptor dependencyDescriptor) where TDependency : class;

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
        bool TryAddDependency<TDependency>(Func<IDependencyProvider, TDependency> dependency, Lifetime lifetime, Binding binding, out IDependencyDescriptor dependencyDescriptor) where TDependency : class;

        bool TryAddDependency<TDependency, TImplementation>(Lifetime lifetime, Binding binding, out IDependencyDescriptor dependencyDescriptor) where TImplementation : TDependency where TDependency : class;

        bool TryAddDependency<TDependency>(Lifetime lifetime, Binding binding, out IDependencyDescriptor dependencyDescriptor) where TDependency : class;
    }
}