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

        /// <summary>
        /// Provides a collection of <see cref="Action{T}"/> used to configure the Dependency.
        /// </summary>
        IReadOnlyList<Action<object>> Configuration { get; }

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