using System;
using DeltaWare.Dependencies.Abstractions.Descriptor;

namespace DeltaWare.Dependencies.Abstractions.Resolver
{
    public interface IDependencyResolver
    {
        IDependencyDescriptor GetDependency(Type definition);

        bool HasDependency(Type definition);
    }
}
