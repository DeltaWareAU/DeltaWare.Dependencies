using System;

namespace DeltaWare.Dependencies.Abstractions
{
    /// <summary>
    /// <see cref="Binding"/> controls how <see cref="IDisposable"/> will be handled when the
    /// dependency is disposed of.
    /// </summary>
    public enum Binding
    {
        /// <summary>
        /// The objects lifetime is bound to the dependency and will be disposed of.
        /// </summary>
        Bound,

        /// <summary>
        /// The objects lifetime is unbound from the dependency and will not be disposed of.
        /// </summary>
        Unbound
    }
}