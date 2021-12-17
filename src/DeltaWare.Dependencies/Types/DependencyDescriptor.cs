using DeltaWare.Dependencies.Abstractions;
using DeltaWare.Dependencies.Abstractions.Configuration;
using DeltaWare.Dependencies.Abstractions.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DeltaWare.Dependencies.Types
{
    [DebuggerDisplay("Type: {Type.Name} | Lifetime: {Lifetime} - Binding: {Binding}")]
    public class DependencyDescriptor : IDependencyDescriptor
    {
        private readonly List<IConfiguration> _configuration = new();

        public Binding Binding { get; }
        public IReadOnlyList<IConfiguration> Configuration => _configuration;
        public Func<IDependencyProvider, object> ImplementationFactory { get; }
        public Func<object> ImplementationInstance { get; }
        public Type ImplementationType { get; }
        public Lifetime Lifetime { get; }
        public Type Type { get; }

        public DependencyDescriptor(Type dependencyType, Func<IDependencyProvider, object> dependency, Lifetime lifetime, Binding binding = Binding.Bound)
        {
            Type = dependencyType ?? throw new ArgumentNullException(nameof(dependencyType));
            ImplementationFactory = dependency ?? throw new ArgumentNullException(nameof(dependency));
            Binding = binding;
            Lifetime = lifetime;
        }

        public DependencyDescriptor(Type dependencyType, Func<object> instance, Lifetime lifetime, Binding binding = Binding.Bound)
        {
            Type = dependencyType ?? throw new ArgumentNullException(nameof(dependencyType));
            ImplementationInstance = instance ?? throw new ArgumentNullException(nameof(instance));
            Binding = binding;
            Lifetime = lifetime;
        }

        public DependencyDescriptor(Type dependencyType, Type implementationType, Lifetime lifetime, Binding binding = Binding.Bound)
        {
            Type = dependencyType ?? throw new ArgumentNullException(nameof(dependencyType));
            ImplementationType = implementationType ?? throw new ArgumentNullException(nameof(implementationType));
            Binding = binding;
            Lifetime = lifetime;
        }

        public void AddConfiguration(IConfiguration configuration)
        {
            _configuration.Add(configuration);
        }
    }
}