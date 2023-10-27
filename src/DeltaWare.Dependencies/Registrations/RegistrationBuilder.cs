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
        private readonly DependencyDescriptorBase _descriptor;

        private readonly DependencyCollection _dependencyCollection;

        private readonly bool _overrideDescriptor;

        public RegistrationBuilder(DependencyCollection collection, DependencyDescriptorBase descriptor, bool overrideDescriptor)
        {
            _dependencyCollection = collection ?? throw new ArgumentNullException(nameof(collection));
            _descriptor = descriptor ?? throw new ArgumentNullException(nameof(descriptor));
            _overrideDescriptor = overrideDescriptor;

            if (descriptor is ReferenceDependencyDescriptor)
            {
                Register(typeof(TImplementation), false);
            }
            else
            {
                Register(typeof(TImplementation));
            }
        }

        public IRegistrationDefinition<TImplementation> DefineAs<TDefinition>()
        {
            Type definition = typeof(TDefinition);

            if (!definition.IsAssignableFrom(_descriptor.ImplementationType))
            {
                throw new Exception($"Cannot assign the implementing type {_descriptor.ImplementationType.Name} to the defining type {definition.Name}");
            }

            Register(definition);

            return this;
        }

        public IRegistrationBinding<TImplementation> AsSingleton()
        {
            _descriptor.Lifetime = Lifetime.Singleton;

            return this;
        }

        public IRegistrationBinding<TImplementation> AsScoped()
        {
            _descriptor.Lifetime = Lifetime.Scoped;

            return this;
        }

        public IRegistrationBinding<TImplementation> AsTransient()
        {
            _descriptor.Lifetime = Lifetime.Transient;

            return this;
        }

        public IRegistrationInitialization<TImplementation> DoNotBind()
        {
            _descriptor.Binding = Binding.Unbound;

            return this;
        }

        public void OnInitialization(Action<TImplementation> onInitialization)
        {
            _descriptor.AddConfiguration(new InstanceConfiguration<TImplementation>(onInitialization));
        }

        public void OnInitialization(Action<IDependencyProvider, TImplementation> onInitialization)
        {
            _descriptor.AddConfiguration(new ProviderConfiguration<TImplementation>(onInitialization));
        }

        private void Register(Type definition)
        {
            Register(definition, _overrideDescriptor);
        }

        private void Register(Type definition, bool overrideDescriptor)
        {
            if (overrideDescriptor)
            {
                _dependencyCollection.InternalRegister(definition, _descriptor);
            }
            else
            {
                _dependencyCollection.InternalTryRegister(definition, _descriptor);
            }
        }
    }
}
