using System;
using DeltaWare.Dependencies.Abstractions.Resolver;

namespace DeltaWare.Dependencies.Abstractions
{
    public interface ILifetimeScope : IDisposable
    {
        IDependencyProvider BuildProvider();

        ILifetimeScope CreateScope();

        IDependencyResolver Resolver { get; }
    }
}
