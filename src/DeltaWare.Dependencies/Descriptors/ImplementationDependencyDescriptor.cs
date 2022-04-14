using DeltaWare.Dependencies.Abstractions;
using DeltaWare.Dependencies.Abstractions.Descriptors;
using DeltaWare.Dependencies.Abstractions.Enums;
using System;

namespace DeltaWare.Dependencies.Descriptors
{
    internal class ImplementationDependencyDescriptor : DependencyDescriptorBase, IInstanceDescriptor
    {
        private readonly Func<IDependencyProvider, object> _implementation;

        public ImplementationDependencyDescriptor(Type type, Func<IDependencyProvider, object> implementation, Lifetime lifetime, Binding binding = Binding.Bound) : base(type, lifetime, binding)
        {
            _implementation = implementation ?? throw new ArgumentNullException(nameof(implementation));
        }

        public IDependencyInstance CreateInstance(IDependencyProvider provider)
        {
            object instance = _implementation.Invoke(provider);

            return ToDependencyInstance(provider, instance);
        }
    }
}
