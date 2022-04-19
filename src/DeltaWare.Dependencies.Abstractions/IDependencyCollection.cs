using DeltaWare.Dependencies.Abstractions.Registration;
using System;

namespace DeltaWare.Dependencies.Abstractions
{
    public interface IDependencyCollection
    {
        IRegistrationDefinition<TReference> Register<TReference>(Func<TReference> builder);
        IRegistrationDefinition<TReference> Register<TReference>(Func<IDependencyProvider, TReference> builder);

        IRegistrationDefinition<TImplementation> Register<TImplementation>();
    }
}
