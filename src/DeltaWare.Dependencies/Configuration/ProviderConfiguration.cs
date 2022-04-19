using DeltaWare.Dependencies.Abstractions;
using DeltaWare.Dependencies.Abstractions.Configuration;
using System;

namespace DeltaWare.Dependencies.Configuration
{
    internal class ProviderConfiguration<T> : IConfiguration
    {
        protected Action<IDependencyProvider, T> Configuration { get; }

        public ProviderConfiguration(Action<IDependencyProvider, T> configuration)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void Configure(IDependencyProvider provider, object instance)
        {
            InternalConfigure(provider, (T)instance);
        }

        protected virtual void InternalConfigure(IDependencyProvider provider, T instance)
        {
            Configuration.Invoke(provider, instance);
        }
    }
}
