using DeltaWare.Dependencies.Abstractions;
using System;

namespace DeltaWare.Dependencies.Types
{
    public class DependencyDescriptor : IDependencyDescriptor
    {
        public Binding Binding { get; }
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
    }
}