namespace DeltaWare.Dependencies.Abstractions.Registration
{
    public interface IRegistrationLifetime
    {
        IRegistrationBinding AsSingleton();

        IRegistrationBinding AsScoped();

        IRegistrationBinding AsTransient();
    }
}
