using DeltaWare.Dependencies.Abstractions.Descriptor;
using System;

namespace DeltaWare.Dependencies.Abstractions.Resolver
{
    public class TypeDependencyResolver<TDefinition> : DependencyResolverBase
    {
        public TypeDependencyResolver(Func<TDefinition> builder, IDependencyResolver? innerResolver = null)
        {
            InnerResolver = (DependencyResolverBase)innerResolver;


        }

        protected override bool TryGetDependency(Type definition, out IDependencyDescriptor descriptor)
        {
            throw new NotImplementedException();
        }

        protected override bool InnerHasDependency(Type definition)
        {
            throw new NotImplementedException();
        }
    }
}
