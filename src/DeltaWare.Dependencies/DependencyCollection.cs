using DeltaWare.Dependencies.Abstractions;
using DeltaWare.Dependencies.Abstractions.Registration;
using DeltaWare.Dependencies.Descriptors;
using DeltaWare.Dependencies.Registrations;
using System;
using System.Collections.Generic;
using DeltaWare.Dependencies.Abstractions.Descriptor;

namespace DeltaWare.Dependencies
{
    public class DependencyCollection : IDependencyCollection
    {
        private readonly List<DependencyDescriptorBase> _dependencies = new();

        public IRegistrationDefinition<TImplementation> Register<TImplementation>(Func<TImplementation> builder)
        {
            return Register<TImplementation>(new ReferenceDependencyDescriptor(_ => builder.Invoke()));
        }

        public IRegistrationDefinition<TImplementation> Register<TImplementation>(Func<IDependencyProvider, TImplementation> builder)
        {
            return Register<TImplementation>(new ReferenceDependencyDescriptor(p => builder.Invoke(p)));
        }

        public IRegistrationDefinition<TImplementation> Register<TImplementation>()
        {
            return Register<TImplementation>(new TypeDependencyDescriptor(typeof(TImplementation)));
        }

        protected virtual IRegistrationDefinition<TImplementation> Register<TImplementation>(DependencyDescriptorBase dependency)
        {
            _dependencies.Add(dependency);

            return new RegistrationBuilder<TImplementation>(dependency);
        }
    }
}
