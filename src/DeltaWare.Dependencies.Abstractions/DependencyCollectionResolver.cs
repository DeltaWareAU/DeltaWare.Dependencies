using DeltaWare.Dependencies.Abstractions.Descriptor;
using System;

namespace DeltaWare.Dependencies.Abstractions
{
    public class DependencyCollectionResolver : DependencyResolverBase
    {
        private readonly IDependencyCollection _dependencyCollection;

        public DependencyCollectionResolver(IDependencyCollection dependencyCollection)
        {
            _dependencyCollection = dependencyCollection ?? throw new ArgumentNullException(nameof(dependencyCollection));
        }

        protected override bool TryGetDependency(Type definition, out IDependencyDescriptor descriptor)
        {
            throw new NotImplementedException();
        }
    }
}
