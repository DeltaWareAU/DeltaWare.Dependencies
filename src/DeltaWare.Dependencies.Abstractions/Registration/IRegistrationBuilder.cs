namespace DeltaWare.Dependencies.Abstractions.Registration
{
    public interface IRegistrationBuilder<out TImplementation> : IRegistrationDefinition<TImplementation>, IRegistrationBinding<TImplementation>
    {
    }
}
