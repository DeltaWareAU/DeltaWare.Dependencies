using DeltaWare.Dependencies.Abstractions.Configuration;
using DeltaWare.Dependencies.Abstractions.Enums;
using System;
using System.Collections.Generic;

namespace DeltaWare.Dependencies.Abstractions
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

        IReadOnlyList<IConfiguration> Configuration { get; }

        /// <summary>
        /// Defines how to get an instance of the dependency.
        /// </summary>
        public Func<IDependencyProvider, object> ImplementationFactory { get; }

        /// <summary>
        /// Defines how to get an instance of the dependency.
        /// </summary>
        Func<object> ImplementationInstance { get; }

        /// <summary>
        /// Defines the implementation <see cref="Type"/>.
        /// </summary>
        public Type ImplementationType { get; }

        /// <summary>
        /// Specifies the <see cref="Lifetime"/>.
        /// </summary>
        Lifetime Lifetime { get; }

        /// <summary>
        /// Specified the <see cref="Type"/>.
        /// </summary>
        Type Type { get; }
    }
}