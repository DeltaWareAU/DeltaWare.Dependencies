using DeltaWare.Dependencies.Abstractions.Descriptor;
using System;

namespace DeltaWare.Dependencies.Abstractions
{
    public interface IDependencyResolver
    {
        public IDependencyDescriptor GetDependency(Type definition);
    }
}
