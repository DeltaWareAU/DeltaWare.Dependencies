using DeltaWare.Dependencies.Abstractions.Descriptor;
using System;

namespace DeltaWare.Dependencies.Abstractions
{
    public interface IDependencyResolver
    {
        IDependencyDescriptor GetDependency(Type definition);

        bool HasDependency(Type definition);
    }
}
