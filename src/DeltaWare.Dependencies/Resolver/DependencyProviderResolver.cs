using DeltaWare.Dependencies.Abstractions;
using DeltaWare.Dependencies.Abstractions.Descriptor;
using DeltaWare.Dependencies.Abstractions.Descriptor.Enums;
using DeltaWare.Dependencies.Descriptors;
using System;

namespace DeltaWare.Dependencies.Resolver
{
    internal class DependencyProviderResolver : DependencyResolverBase
    {
        private readonly IDependencyDescriptor _providerDescriptor;

        public DependencyProviderResolver(IDependencyProvider provider, IDependencyResolver innerResolver)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            InnerResolver = (DependencyResolverBase)innerResolver ?? throw new ArgumentNullException(nameof(innerResolver));

            _providerDescriptor = new ReferenceDependencyDescriptor(typeof(IDependencyProvider), _ => provider)
            {
                Lifetime = Lifetime.Scoped,
                Binding = Binding.Unbound
            };
        }

        protected override bool TryGetDependency(Type definition, out IDependencyDescriptor descriptor)
        {
            if (definition == typeof(IDependencyProvider))
            {
                descriptor = _providerDescriptor;

                return true;
            }

            descriptor = null;

            return false;
        }

        protected override bool InnerHasDependency(Type definition)
        {
            return definition == typeof(ILifetimeScope);
        }
    }
}
