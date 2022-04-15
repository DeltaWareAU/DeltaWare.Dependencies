using System;
using DeltaWare.Dependencies.Abstractions.Descriptors;
using DeltaWare.Dependencies.Abstractions.Descriptors.Enums;

namespace DeltaWare.Dependencies.Abstractions.Registration
{
    internal class RegistrationBuilder<TImplementation> : IRegistrationBuilder<TImplementation>
    {
        private readonly Type _implementationType;

        private Type _definitionType;

        private readonly DependencyDescriptorBase _dependencyDescriptor;

        public RegistrationBuilder(DependencyDescriptorBase dependencyDescriptor)
        {
            _dependencyDescriptor = dependencyDescriptor ?? throw new ArgumentNullException(nameof(dependencyDescriptor));
            _implementationType = typeof(TImplementation);

            if (_implementationType.IsInterface)
            {
                throw new ArgumentException();
            }
        }

        public IRegistrationLifetime DefineAs<TDefinition>()
        {
            _definitionType = typeof(TDefinition);

            if (!_implementationType.IsAssignableFrom(_definitionType))
            {
                throw new ArgumentException();
            }

            return this;
        }

        public IRegistrationBinding AsSingleton()
        {
            _dependencyDescriptor.Lifetime = Lifetime.Singleton;

            throw new System.NotImplementedException();
        }

        public IRegistrationBinding AsScoped()
        {
            throw new System.NotImplementedException();
        }

        public IRegistrationBinding AsTransient()
        {
            throw new System.NotImplementedException();
        }
    }
}
