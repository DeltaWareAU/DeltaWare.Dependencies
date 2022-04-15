using DeltaWare.Dependencies.Abstractions.Registration;

namespace DeltaWare.Dependencies.Abstractions
{
    public interface IDependencyCollection
    {
        IRegistrationBuilder<TReference> Register<TReference>();

        IRegistrationBuilder<TImplementation> RegisterType<TImplementation>();
    }
}
