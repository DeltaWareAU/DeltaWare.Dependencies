using DeltaWare.Dependencies.Abstractions;
using DeltaWare.Dependencies.Abstractions.Descriptors;
using DeltaWare.Dependencies.Abstractions.Enums;
using System;

namespace DeltaWare.Dependencies.Descriptors
{
    internal class InstanceDependencyDescriptor : DependencyDescriptorBase, IInstanceDescriptor
    {
        private readonly Func<object> _instance;

        public InstanceDependencyDescriptor(Type type, Func<object> instance, Lifetime lifetime, Binding binding = Binding.Bound) : base(type, lifetime, binding)
        {
            _instance = instance ?? throw new ArgumentNullException(nameof(instance));
        }

        public IDependencyInstance CreateInstance(IDependencyProvider provider)
        {
            object instance = _instance.Invoke();

            return ToDependencyInstance(provider, instance);
        }
    }
}
