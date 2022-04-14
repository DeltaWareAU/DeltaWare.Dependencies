namespace DeltaWare.Dependencies.Abstractions.Descriptors
{
    public interface IInstanceDescriptor : IDependencyDescriptor
    {
        IDependencyInstance CreateInstance(IDependencyProvider provider);
    }
}
