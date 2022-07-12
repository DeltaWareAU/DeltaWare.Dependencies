namespace DeltaWare.Dependencies.Abstractions.Registration
{
    public interface IRegistrationDefinition<out TImplementation> : IRegistrationLifetime<TImplementation>
    {
        IRegistrationDefinition<TImplementation> DefineAs<TDefinition>();
    }
}
