using DeltaWare.Dependencies.Abstractions;
using DeltaWare.Dependencies.Abstractions.Descriptor.Enums;
using DeltaWare.Dependencies.Abstractions.Registration;
using DeltaWare.Dependencies.Configuration;
using DeltaWare.Dependencies.Descriptors;
using System;

namespace DeltaWare.Dependencies.Registrations
{
    internal class RegistrationBuilder<TImplementation> : IRegistrationBuilder<TImplementation>
    {
        private readonly DependencyDescriptorBase _dependencyDescriptor;

        public RegistrationBuilder(DependencyDescriptorBase dependencyDescriptor)
        {
            _dependencyDescriptor = dependencyDescriptor ?? throw new ArgumentNullException(nameof(dependencyDescriptor));
            _dependencyDescriptor.DefinitionType = typeof(TImplementation);
        }

        public IRegistrationLifetime<TImplementation> DefineAs<TDefinition>()
        {
            Type definitionType = typeof(TDefinition);

            if (!_dependencyDescriptor.DefinitionType.IsAssignableFrom(definitionType))
            {
                throw new ArgumentException();
            }

            _dependencyDescriptor.DefinitionType = definitionType;

            return this;
        }

        public IRegistrationBinding<TImplementation> AsSingleton()
        {
            _dependencyDescriptor.Lifetime = Lifetime.Singleton;

            return this;
        }

        public IRegistrationBinding<TImplementation> AsScoped()
        {
            _dependencyDescriptor.Lifetime = Lifetime.Singleton;

            return this;
        }

        public IRegistrationBinding<TImplementation> AsTransient()
        {
            _dependencyDescriptor.Lifetime = Lifetime.Singleton;

            return this;
        }

        public IRegistrationInitialization<TImplementation> DoNotBind()
        {
            _dependencyDescriptor.Binding = Binding.Unbound;

            return this;
        }

        public void OnInitialization(Action<TImplementation> onInitialization)
        {
            _dependencyDescriptor.AddConfiguration(new InstanceConfiguration<TImplementation>(onInitialization));
        }

        public void OnInitialization(Action<IDependencyProvider, TImplementation> onInitialization)
        {
            _dependencyDescriptor.AddConfiguration(new ProviderConfiguration<TImplementation>(onInitialization));
        }
    }
}
