namespace DeltaWare.Dependencies.Abstractions.Registration
{
    public interface IRegistrationDefinition<TImplementation> : IRegistrationLifetime<TImplementation>
    {
        IRegistrationDefinition<TImplementation> DefineAs<TDefinition>() where TDefinition : TImplementation;
    }
}
