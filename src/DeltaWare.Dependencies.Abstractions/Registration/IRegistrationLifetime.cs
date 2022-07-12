namespace DeltaWare.Dependencies.Abstractions.Registration
{
    public interface IRegistrationLifetime<out TImplementation>
    {
        IRegistrationBinding<TImplementation> AsSingleton();

        IRegistrationBinding<TImplementation> AsScoped();

        IRegistrationBinding<TImplementation> AsTransient();
    }
}
