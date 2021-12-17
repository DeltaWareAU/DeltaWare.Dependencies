using System;

namespace DeltaWare.Dependencies.Abstractions.Configuration
{
    public interface ITypeConfiguration : IConfiguration
    {
        Action<object> Configurator { get; }
    }
}