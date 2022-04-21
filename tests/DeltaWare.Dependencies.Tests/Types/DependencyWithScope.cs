using DeltaWare.Dependencies.Abstractions;

namespace DeltaWare.Dependencies.Tests.Types
{
    public class DependencyWithScope
    {
        public ILifetimeScope Scope { get; }

        public DependencyWithScope(ILifetimeScope scope)
        {
            Scope = scope;
        }
    }
}