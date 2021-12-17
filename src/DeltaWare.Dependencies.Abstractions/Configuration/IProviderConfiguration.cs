using System;

namespace DeltaWare.Dependencies.Abstractions.Configuration
{
    public interface IProviderConfiguration : IConfiguration
    {
        Action<IDependencyProvider, object> Configurator { get; }
    }
}