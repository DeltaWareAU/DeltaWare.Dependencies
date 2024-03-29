﻿namespace DeltaWare.Dependencies.Abstractions.Enums
{
    /// <summary>
    /// Specifies the lifetime of a dependency.
    /// </summary>
    public enum Lifetime
    {
        /// <summary>
        /// Specifies that the dependencies lifetime is attached to its <see
        /// cref="IDependencyProvider"/> parent <see cref="IDependencyScope"/>.
        /// </summary>
        Singleton,

        /// <summary>
        /// Specifies that the dependencies lifetime is attached to its <see cref="IDependencyProvider"/>.
        /// </summary>
        Scoped,

        /// <summary>
        /// Specifies that the dependencies lifetime is not attached to its <see cref="IDependencyProvider"/>.
        /// </summary>
        Transient
    }
}