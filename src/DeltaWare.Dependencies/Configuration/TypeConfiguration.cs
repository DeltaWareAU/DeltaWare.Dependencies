using DeltaWare.Dependencies.Abstractions.Configuration;
using System;

namespace DeltaWare.Dependencies.Configuration
{
    internal class TypeConfiguration<T> : ITypeConfiguration where T : class
    {
        public Action<object> Configurator { get; }

        public TypeConfiguration(Action<T> configurator)
        {
            Configurator = o => configurator((T)o);
        }
    }
}