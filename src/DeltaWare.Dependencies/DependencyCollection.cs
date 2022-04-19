using DeltaWare.Dependencies.Abstractions;
using DeltaWare.Dependencies.Abstractions.Registration;
using DeltaWare.Dependencies.Descriptors;
using DeltaWare.Dependencies.Registrations;
using System;

namespace DeltaWare.Dependencies
{
    public class DependencyCollection : IDependencyCollection
    {
        public IRegistrationDefinition<TImplementation> Register<TImplementation>(Func<TImplementation> builder)
        {
            return new RegistrationBuilder<TImplementation>(new ReferenceDependencyDescriptor(_ => builder.Invoke()));
        }

        public IRegistrationDefinition<TImplementation> Register<TImplementation>(Func<IDependencyProvider, TImplementation> builder)
        {
            return new RegistrationBuilder<TImplementation>(new ReferenceDependencyDescriptor(p => builder.Invoke(p)));
        }

        public IRegistrationDefinition<TImplementation> Register<TImplementation>()
        {
            return new RegistrationBuilder<TImplementation>(new TypeDependencyDescriptor(typeof(TImplementation)));
        }
    }
}
