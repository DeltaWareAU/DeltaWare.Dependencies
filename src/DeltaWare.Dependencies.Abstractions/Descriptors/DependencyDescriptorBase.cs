using DeltaWare.Dependencies.Abstractions.Descriptors.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using DeltaWare.Dependencies.Abstractions.Configuration;

namespace DeltaWare.Dependencies.Abstractions.Descriptors
{
    [DebuggerDisplay("Type: {Type.Name} | Lifetime: {Lifetime} - Binding: {Binding}")]
    public abstract class DependencyDescriptorBase : IDependencyDescriptor
    {
        private readonly List<IConfiguration> _configuration = new();

        public Binding Binding { get; internal set; }

        public Lifetime Lifetime { get; internal set; }

        public Type Type { get; internal set; }
        
        protected virtual void ConfigureInstance(IDependencyProvider provider, object instance)
        {
            foreach (IConfiguration configuration in _configuration)
            {
                configuration.Configurator.Invoke(provider, instance);
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
