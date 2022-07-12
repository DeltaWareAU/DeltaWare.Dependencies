using DeltaWare.Dependencies.Abstractions.Descriptor;
using System;

namespace DeltaWare.Dependencies.Abstractions.Resolver
{
    public interface IDependencyResolver
    {
        IDependencyDescriptor GetDependency(Type definition);

        bool HasDependency(Type definition);
    }
}
