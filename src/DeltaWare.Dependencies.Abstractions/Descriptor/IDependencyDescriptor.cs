using DeltaWare.Dependencies.Abstractions.Descriptor.Enums;
using System;

namespace DeltaWare.Dependencies.Abstractions.Descriptor
{
    public interface IDependencyDescriptor
    {
        /// <summary>
        /// Specifies the <see cref="Binding"/>.
        /// </summary>
        Binding Binding { get; }

        /// <summary>
        /// Specifies the <see cref="Lifetime"/>.
        /// </summary>
        Lifetime Lifetime { get; }

        public Type ImplementationType { get; }

        IDependencyInstance CreateInstance(IDependencyProvider provider);
    }
}
