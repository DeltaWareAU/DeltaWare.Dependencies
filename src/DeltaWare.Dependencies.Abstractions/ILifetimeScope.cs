using DeltaWare.Dependencies.Abstractions.Resolver;
using System;

namespace DeltaWare.Dependencies.Abstractions
{
    public interface ILifetimeScope : IDisposable
    {
        IDependencyProvider BuildProvider();

        ILifetimeScope CreateScope();

        IDependencyResolver Resolver { get; }
    }
}
