namespace DeltaWare.Dependencies.Abstractions.Registration
{
    public interface IRegistrationBinding<out TImplementation> : IRegistrationInitialization<TImplementation>
    {
        IRegistrationInitialization<TImplementation> DoNotBind();
    }
}
