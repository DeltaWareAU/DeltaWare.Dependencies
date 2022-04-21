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

        private readonly DependencyCollection _dependencyCollection;

        private readonly bool _overrideDescriptor;

        public RegistrationBuilder(DependencyCollection dependencyCollection, DependencyDescriptorBase dependencyDescriptor, bool overrideDescriptor)
        {
            _dependencyCollection = dependencyCollection ?? throw new ArgumentNullException(nameof(dependencyCollection));
            _dependencyDescriptor = dependencyDescriptor ?? throw new ArgumentNullException(nameof(dependencyDescriptor));
            _overrideDescriptor = overrideDescriptor;

            Register(typeof(TImplementation));
        }

        public IRegistrationDefinition<TImplementation> DefineAs<TDefinition>() where TDefinition : TImplementation
        {
            Register(typeof(TDefinition));

            return this;
        }

        public IRegistrationBinding<TImplementation> AsSingleton()
        {
            _dependencyDescriptor.Lifetime = Lifetime.Singleton;

            return this;
        }

        public IRegistrationBinding<TImplementation> AsScoped()
        {
            _dependencyDescriptor.Lifetime = Lifetime.Scoped;

            return this;
        }

        public IRegistrationBinding<TImplementation> AsTransient()
        {
            _dependencyDescriptor.Lifetime = Lifetime.Transient;

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

        private void Register(Type definition)
        {
            if (_overrideDescriptor)
            {
                _dependencyCollection.InternalRegister(definition, _dependencyDescriptor);
            }
            else
            {
                _dependencyCollection.InternalTryRegister(definition, _dependencyDescriptor);
            }
        }
    }
}
