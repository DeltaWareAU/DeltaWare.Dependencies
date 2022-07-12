using DeltaWare.Dependencies.Abstractions;
using DeltaWare.Dependencies.Abstractions.Enums;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DeltaWare.Dependencies.Extensions.DependencyInjection
{
    public static class DependencyCollectionExtensions
    {
        public static void AddServiceCollection(this IDependencyCollection dependencies, IServiceCollection services)
        {
            dependencies.AddSingleton(() => services, Binding.Unbound);
            dependencies.AddScoped<IServiceProvider>(p => p.GetRequiredDependency<IServiceCollection>().BuildServiceProvider());

            foreach (ServiceDescriptor service in services)
            {
                Lifetime lifetime = service.Lifetime switch
                {
                    ServiceLifetime.Singleton => Lifetime.Singleton,
                    ServiceLifetime.Scoped => Lifetime.Scoped,
                    ServiceLifetime.Transient => Lifetime.Transient,
                    _ => throw new ArgumentOutOfRangeException()
                };

                dependencies.AddDependency(new ServiceProviderDescriptor(service.ServiceType, lifetime));
            }
        }
    }
}
