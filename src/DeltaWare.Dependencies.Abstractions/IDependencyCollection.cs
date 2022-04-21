using DeltaWare.Dependencies.Abstractions.Registration;
using System;

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

        bool HasDependency(Type definition);

        bool Remove(Type definition);

        void Configure<TDefinition>(Action<TDefinition> configuration);
        void Configure<TDefinition>(Action<IDependencyProvider, TDefinition> configuration);

        void AddResolver(DependencyResolverBase dependencyResolver);
        void AddResolver<TResolver>() where TResolver : DependencyResolverBase;
    }

    public static class DependencyCollectionExtensions
    {
        public static bool HasDependency<TDefinition>(this IDependencyCollection collection)
        {
            return collection.HasDependency(typeof(TDefinition));
        }
        public static bool Remove<TDefinition>(this IDependencyCollection collection)
        {
            return collection.Remove(typeof(TDefinition));
        }

        public static void Configure<TDefinition, TImplementation>(this IDependencyCollection collection, Action<TImplementation> configuration) where TImplementation : TDefinition
        {
            collection.Configure<TDefinition>(d => configuration.Invoke((TImplementation)d));
        }

        public static void Configure<TDefinition, TImplementation>(this IDependencyCollection collection, Action<IDependencyProvider, TImplementation> configuration) where TImplementation : TDefinition
        {
            collection.Configure<TDefinition>((p, d) => configuration.Invoke(p, (TImplementation)d));
        }
    }
}
