using DeltaWare.Dependencies.Abstractions;
using System;
using System.Diagnostics;

namespace DeltaWare.Dependencies.Descriptors
{
    [DebuggerDisplay("ReferenceType: {ImplementationType.Name} | Lifetime: {Lifetime} - Binding: {Binding}")]
    internal sealed class ReferenceDependencyDescriptor<TImplementation> : ReferenceDependencyDescriptor
    {
        private readonly Func<IDependencyProvider, TImplementation> _referenceBuilder;

        public ReferenceDependencyDescriptor(Func<IDependencyProvider, TImplementation> referenceBuilder) : base(typeof(TImplementation))
        {
            _referenceBuilder = referenceBuilder ?? throw new ArgumentNullException(nameof(referenceBuilder));
        }

        protected override object InternalCreateInstance(IDependencyProvider provider)
        {
            return _referenceBuilder.Invoke(provider);
        }
    }
}
