using System;

namespace DeltaWare.Dependencies.Abstractions.Registration
{
    public interface IRegistrationInitialization<out TImplementation>
    {
        void OnInitialization(Action<TImplementation> onInitialization);
        void OnInitialization(Action<IDependencyProvider, TImplementation> onInitialization);
    }
}
