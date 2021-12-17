using DeltaWare.Dependencies.Abstractions;
using DeltaWare.Dependencies.Abstractions.Configuration;
using System;

namespace DeltaWare.Dependencies.Configuration
{
    internal class ProviderConfiguration<T> : IProviderConfiguration where T : class
    {
        public Action<IDependencyProvider, object> Configurator { get; }

        public ProviderConfiguration(Action<IDependencyProvider, T> configurator)
        {
            Configurator = (p, o) => configurator.Invoke(p, (T)o);
        }
    }
}