using DeltaWare.Dependencies.Abstractions;
using DeltaWare.Dependencies.Abstractions.Configuration;
using System;

namespace DeltaWare.Dependencies.Configuration
{
    internal class InstanceConfiguration<T> : IConfiguration
    {
        protected Action<T> Configuration { get; }

        public InstanceConfiguration(Action<T> configuration)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void Configure(IDependencyProvider provider, object instance)
        {
            InternalConfigure((T)instance);
        }

        protected virtual void InternalConfigure(T instance)
        {
            Configuration.Invoke(instance);
        }
    }
}
