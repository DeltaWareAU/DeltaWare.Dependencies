using DeltaWare.Dependencies.Abstractions.Enums;
using System;

namespace DeltaWare.Dependencies.Abstractions.Descriptors
{
    /// <summary>
    /// Describes a dependencies lifetime and implementation.
    /// </summary>
    public interface IDependencyDescriptor
    {
        /// <summary>
        /// Specifies the <see cref="Binding"/>.
        /// </summary>
        Binding Binding { get; }

        Type Type { get; }

        /// <summary>
        /// Specifies the <see cref="Lifetime"/>.
        /// </summary>
        Lifetime Lifetime { get; }
    }
}