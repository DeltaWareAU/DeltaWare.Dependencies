using DeltaWare.Dependencies.Abstractions;
using System;
using System.Diagnostics;

namespace DeltaWare.Dependencies.Descriptors
{
    [DebuggerDisplay("InstanceType: {ImplementationType.Name} | Lifetime: {Lifetime} - Binding: {Binding}")]
    internal sealed class InstanceDependencyDescriptor<TImplementation> : DependencyDescriptorBase
    {
        private readonly Func<TImplementation> _referenceBuilder;

        public InstanceDependencyDescriptor(Func<TImplementation> referenceBuilder) : base(typeof(TImplementation))
        {
            _referenceBuilder = referenceBuilder ?? throw new ArgumentNullException(nameof(referenceBuilder));
        }

        protected override object InternalCreateInstance(IDependencyProvider provider)
        {
            return _referenceBuilder.Invoke();
        }
    }
}
