using DeltaWare.Dependencies.Abstractions.Descriptors;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace DeltaWare.Dependencies.Abstractions
{
    public static class ReadonlyDependencyCollectionExtensions
    {
        public static IDependencyDescriptor GetDependencyDescriptor<TDependency>(this IReadOnlyDependencyCollection dependencies) where TDependency : class
        {
            return dependencies.GetDependencyDescriptor(typeof(TDependency));
        }

        public static IEnumerable<IDependencyDescriptor> GetDependencyDescriptors<TDependency>(this IReadOnlyDependencyCollection dependencies) where TDependency : class
        {
            return dependencies.GetDependencyDescriptors(typeof(TDependency));
        }

        public static bool HasDependency<TDependency>(this IReadOnlyDependencyCollection dependencies) where TDependency : class
        {
            return dependencies.HasDependency(typeof(TDependency));
        }
    }
}