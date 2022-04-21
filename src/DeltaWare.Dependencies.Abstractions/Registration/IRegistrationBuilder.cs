namespace DeltaWare.Dependencies.Abstractions.Registration
{
    public interface IRegistrationBuilder<TImplementation> : IRegistrationDefinition<TImplementation>, IRegistrationBinding<TImplementation>
    {
    }
}
