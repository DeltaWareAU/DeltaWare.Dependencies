using System;

namespace DeltaWare.Dependencies.Abstractions
{
    public interface IDependencyScope : IDisposable
    {
        IDependencyProvider BuildProvider();

        IDependencyScope CreateScope();
    }
}