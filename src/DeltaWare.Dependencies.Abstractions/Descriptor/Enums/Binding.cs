namespace DeltaWare.Dependencies.Abstractions.Descriptor.Enums
{
    /// <summary>
    /// Defines if the dependency is disposed of at the end of its lifetime.
    /// </summary>
    /// <remarks>
    /// Caution - Removing a dependencies binding to its parent scope can cause memory leaks. If the
    /// dependency is not manually disposed of.
    /// </remarks>
    public enum Binding
    {
        /// <summary>
        /// The dependencies lifetime is bound to the <see cref="IDependencyProvider"/> and will be
        /// disposed of.
        /// </summary>
        Bound,

        /// <summary>
        /// The dependencies lifetime is not bound to the <see cref="IDependencyProvider"/> and will
        /// not be disposed of.
        /// </summary>
        Unbound
    }
}
