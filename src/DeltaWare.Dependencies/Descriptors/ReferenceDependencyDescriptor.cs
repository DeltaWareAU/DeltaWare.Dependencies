using DeltaWare.Dependencies.Abstractions;
using System;

namespace DeltaWare.Dependencies.Descriptors
{
    internal sealed class ReferenceDependencyDescriptor : DependencyDescriptorBase
    {
        private readonly Func<IDependencyProvider, object> _referenceBuilder;

        public ReferenceDependencyDescriptor(Func<IDependencyProvider, object> referenceBuilder)
        {
            _referenceBuilder = referenceBuilder ?? throw new ArgumentNullException(nameof(referenceBuilder));
        }

        protected override object InternalCreateInstance(IDependencyProvider provider)
        {
            return _referenceBuilder.Invoke(provider);
        }
    }
}
