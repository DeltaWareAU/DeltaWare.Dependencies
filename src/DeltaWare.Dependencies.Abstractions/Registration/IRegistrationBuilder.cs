namespace DeltaWare.Dependencies.Abstractions.Registration
{
    public interface IRegistrationBuilder<in TImplementation>: IRegistrationLifetime
    {
        IRegistrationLifetime DefineAs<TDefinition>();
    }
}
