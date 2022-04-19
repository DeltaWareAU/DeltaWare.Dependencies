using System;

namespace DeltaWare.Dependencies.Abstractions.Descriptor
{
    public interface IDependencyInstance : IDisposable
    {
        object Instance { get; }

        IDependencyDescriptor Descriptor { get; }

        bool IsDisposable { get; }
    }
}
