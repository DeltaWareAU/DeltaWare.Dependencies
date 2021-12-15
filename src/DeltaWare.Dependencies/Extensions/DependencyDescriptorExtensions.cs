using DeltaWare.Dependencies.Abstractions;
using DeltaWare.Dependencies.Types;

namespace DeltaWare.Dependencies.Extensions
{
    internal static class DependencyDescriptorExtensions
    {
        public static IDependencyInstance ToInstance(this IDependencyDescriptor descriptor, object instance)
        {
            return new DependencyInstance(instance, descriptor.Type, descriptor.Lifetime, descriptor.Binding);
        }
    }
}