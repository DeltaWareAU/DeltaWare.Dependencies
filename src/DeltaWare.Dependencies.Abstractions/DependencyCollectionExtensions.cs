using System;
using DeltaWare.Dependencies.Abstractions.Descriptor;

namespace DeltaWare.Dependencies.Abstractions
{
    public static class DependencyCollectionExtensions
    {
        public static bool HasDependency<TDefinition>(this IDependencyCollection collection)
        {
            return collection.Contains(typeof(TDefinition));
        }

        public static bool Remove<TDefinition>(this IDependencyCollection collection)
        {
            return collection.Remove(typeof(TDefinition));
        }

        public static IDependencyDescriptor Get<TDefinition>(this IDependencyCollection collection)
        {
            return collection.Get(typeof(TDefinition));
        }

        public static void Configure<TDefinition, TImplementation>(this IDependencyCollection collection, Action<TImplementation> configuration) where TImplementation : TDefinition
        {
            collection.Configure<TDefinition>(d => configuration.Invoke((TImplementation)d));
        }

        public static void Configure<TDefinition, TImplementation>(this IDependencyCollection collection, Action<IDependencyProvider, TImplementation> configuration) where TImplementation : TDefinition
        {
            collection.Configure<TDefinition>((p, d) => configuration.Invoke(p, (TImplementation)d));
        }

        public static bool CheckIfAdded<TDefinition>(this IDependencyCollection collection, Action<IDependencyCollection> register)
        {
            IDependencyDescriptor descriptor = collection.Get<TDefinition>();

            if (descriptor == null)
            {
                register.Invoke(collection);

                return collection.HasDependency<TDefinition>();
            }

            int oldHashCode = descriptor.GetHashCode();

            register.Invoke(collection);

            descriptor = collection.Get<TDefinition>();

            if (descriptor == null)
            {
                return false;
            }

            return oldHashCode != descriptor.GetHashCode();
        }
    }
}
