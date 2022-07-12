using DeltaWare.Dependencies.Abstractions;
using DeltaWare.Dependencies.Abstractions.Descriptors;
using DeltaWare.Dependencies.Abstractions.Enums;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;

namespace DeltaWare.Dependencies.Extensions.DependencyInjection
{
    [DebuggerDisplay("Type: {Type.Name} | Lifetime: {Lifetime} - Binding: DependencyInjection")]
    internal class ServiceProviderDescriptor : DependencyDescriptorBase, IInstanceDescriptor
    {
        public ServiceProviderDescriptor(Type type, Lifetime lifetime) : base(type, lifetime, Binding.Unbound)
        {
        }

        public IDependencyInstance CreateInstance(IDependencyProvider provider)
        {
            IServiceProvider serviceProvider = provider.GetRequiredDependency<IServiceProvider>();

            object instance = serviceProvider.GetRequiredService(Type);

            return ToDependencyInstance(provider, instance);
        }
    }
}
