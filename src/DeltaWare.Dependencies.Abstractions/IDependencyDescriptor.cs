using System;

namespace DeltaWare.Dependencies.Abstractions
{
    /// <summary>
    /// Describes a dependency.
    /// </summary>
    public interface IDependencyDescriptor
    {
        /// <summary>
        /// Specifies the <see cref="Binding"/> of the <see cref="IDependencyDescriptor"/>.
        /// </summary>
        Binding Binding { get; }

        public Func<IDependencyProvider, object> ImplementationFactory { get; }
        Func<object> ImplementationInstance { get; }
        public Type ImplementationType { get; }

        /// <summary>
        /// Specifies the <see cref="Lifetime"/> of the <see cref="IDependencyDescriptor"/>.
        /// </summary>
        Lifetime Lifetime { get; }

        /// <summary>
        /// Specified the <see cref="Type"/> of the <see cref="IDependencyDescriptor"/>,
        /// </summary>
        Type Type { get; }
    }
}