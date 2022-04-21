using DeltaWare.Dependencies.Abstractions;
using DeltaWare.Dependencies.Abstractions.Descriptor;
using DeltaWare.Dependencies.Abstractions.Descriptor.Enums;
using DeltaWare.Dependencies.Abstractions.Exceptions;
using DeltaWare.Dependencies.Descriptors;
using System;

namespace DeltaWare.Dependencies.Resolver
{
    internal class LifetimeScopeResolver : DependencyResolverBase
    {
        private readonly IDependencyDescriptor _scopeDescriptor;

        public LifetimeScopeResolver(LifetimeScope lifetimeScope, IDependencyResolver innerResolver)
        {
            if (lifetimeScope == null)
            {
                throw new ArgumentNullException(nameof(lifetimeScope));
            }

            InnerResolver = (DependencyResolverBase)innerResolver ?? throw new ArgumentNullException(nameof(innerResolver));

            _scopeDescriptor = new ReferenceDependencyDescriptor(typeof(ILifetimeScope), _ => lifetimeScope.ParentScope ?? throw new InvalidScopeException())
            {
                Lifetime = Lifetime.Singleton,
                Binding = Binding.Unbound
            };
        }

        protected override bool TryGetDependency(Type definition, out IDependencyDescriptor descriptor)
        {
            if (definition == typeof(ILifetimeScope))
            {
                descriptor = _scopeDescriptor;

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
