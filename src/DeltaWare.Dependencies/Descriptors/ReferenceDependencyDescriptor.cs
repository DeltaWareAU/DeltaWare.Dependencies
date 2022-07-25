﻿using DeltaWare.Dependencies.Abstractions;
using System;
using System.Diagnostics;

namespace DeltaWare.Dependencies.Descriptors
{
    [DebuggerDisplay("ReferenceType: {ImplementationType.Name} | Lifetime: {Lifetime} - Binding: {Binding}")]
    internal class ReferenceDependencyDescriptor : DependencyDescriptorBase
    {
        private readonly Func<IDependencyProvider, object> _referenceBuilder;

        protected ReferenceDependencyDescriptor(Type implementationType) : base(implementationType)
        {
        }

        public ReferenceDependencyDescriptor(Type implementationType, Func<IDependencyProvider, object> referenceBuilder) : base(implementationType)
        {
            _referenceBuilder = referenceBuilder ?? throw new ArgumentNullException(nameof(referenceBuilder));
        }

        protected override object InternalCreateInstance(IDependencyProvider provider)
        {
            return _referenceBuilder.Invoke(provider);
        }
    }
}
