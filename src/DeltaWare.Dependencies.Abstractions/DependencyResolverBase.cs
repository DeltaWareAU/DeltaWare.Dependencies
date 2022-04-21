using DeltaWare.Dependencies.Abstractions.Descriptor;
using System;

namespace DeltaWare.Dependencies.Abstractions
{
    public abstract class DependencyResolverBase : IDependencyResolver
    {
        public DependencyResolverBase InnerResolver { get; set; }

        public IDependencyDescriptor GetDependency(Type definition)
        {
            if (TryGetDependency(definition, out IDependencyDescriptor descriptor))
            {
                return descriptor;
            }

            return InnerResolver?.GetDependency(definition);
        }

        public bool HasDependency(Type definition)
        {
            if (InnerHasDependency(definition))
            {
                return true;
            }

            return InnerResolver.HasDependency(definition);
        }

        protected abstract bool TryGetDependency(Type definition, out IDependencyDescriptor descriptor);

        protected abstract bool InnerHasDependency(Type definition);
    }
}
