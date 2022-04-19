namespace DeltaWare.Dependencies.Abstractions.Configuration
{
    public interface IConfiguration
    {
        void Configure(IDependencyProvider provider, object instance);
    }
}
