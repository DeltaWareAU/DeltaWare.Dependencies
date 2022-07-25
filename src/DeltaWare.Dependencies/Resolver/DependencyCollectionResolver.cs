using DeltaWare.Dependencies.Abstractions.Descriptor;
using DeltaWare.Dependencies.Abstractions.Resolver;
using System;

namespace DeltaWare.Dependencies.Resolver
{
    public class DependencyCollectionResolver : DependencyResolverBase
    {
        private readonly DependencyCollection _dependencyCollection;

        public DependencyCollectionResolver(DependencyCollection dependencyCollection)
        {
            _dependencyCollection = dependencyCollection ?? throw new ArgumentNullException(nameof(dependencyCollection));
        }

        protected override bool TryGetDependency(Type definition, out IDependencyDescriptor descriptor)
        {
            descriptor = _dependencyCollection.Get(definition);

            return descriptor != null;
        }

        protected override bool InnerHasDependency(Type definition)
        {
            return _dependencyCollection.Contains(definition);
        }
    }
}
