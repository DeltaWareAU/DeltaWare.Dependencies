using DeltaWare.Dependencies.Abstractions.Configuration;
using DeltaWare.Dependencies.Abstractions.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DeltaWare.Dependencies.Abstractions.Descriptors
{
    [DebuggerDisplay("Type: {Type.Name} | Lifetime: {Lifetime} - Binding: {Binding}")]
    public abstract class DependencyDescriptorBase : IDependencyDescriptor
    {
        private readonly List<IConfiguration> _configuration = new();

        public Binding Binding { get; }

        public Lifetime Lifetime { get; }

        public Type Type { get; }

        protected DependencyDescriptorBase(Type type, Lifetime lifetime, Binding binding = Binding.Bound)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Binding = binding;
            Lifetime = lifetime;
        }

        protected virtual void ConfigureInstance(IDependencyProvider provider, object instance)
        {
            foreach (IConfiguration configuration in _configuration)
            {
                switch (configuration)
                {
                    case ITypeConfiguration typeConfiguration:
                        typeConfiguration.Configurator.Invoke(instance);
                        break;

                    case IProviderConfiguration providerConfiguration:
                        providerConfiguration.Configurator.Invoke(provider, instance);
                        break;
                }
            }
        }

        protected virtual IDependencyInstance ToDependencyInstance(IDependencyProvider provider, object instance)
        {
            ConfigureInstance(provider, instance);

            return new DependencyInstance(instance, Type, Lifetime, Binding);
        }

        public void AddConfiguration(IConfiguration configuration)
        {
            _configuration.Add(configuration);
        }
    }
}
