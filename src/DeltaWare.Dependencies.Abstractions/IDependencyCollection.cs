using DeltaWare.Dependencies.Abstractions.Registration;
using DeltaWare.Dependencies.Abstractions.Resolver;
using System;
using DeltaWare.Dependencies.Abstractions.Descriptor;

namespace DeltaWare.Dependencies.Abstractions
{
    public interface IDependencyCollection
    {
        IRegistrationDefinition<TReference> Register<TReference>(Func<TReference> builder);
        IRegistrationDefinition<TReference> Register<TReference>(Func<IDependencyProvider, TReference> builder);
        IRegistrationDefinition<TImplementation> Register<TImplementation>();

        IRegistrationDefinition<TReference> TryRegister<TReference>(Func<TReference> builder);
        IRegistrationDefinition<TReference> TryRegister<TReference>(Func<IDependencyProvider, TReference> builder);
        IRegistrationDefinition<TImplementation> TryRegister<TImplementation>();

        ILifetimeScope CreateScope();

        IDependencyProvider BuildProvider();

        IDependencyDescriptor Get(Type definition);

        bool Contains(Type definition);

        bool Remove(Type definition);

        void Configure<TDefinition>(Action<TDefinition> configuration);
        void Configure<TDefinition>(Action<IDependencyProvider, TDefinition> configuration);

        void AddResolver(DependencyResolverBase dependencyResolver);
        void AddResolver<TResolver>() where TResolver : DependencyResolverBase;
    }
}
