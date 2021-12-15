using DeltaWare.Dependencies.Abstractions;

namespace DeltaWare.Dependencies.Tests.Types
{
    public class DependencyWithScope
    {
        public IDependencyScope Scope { get; }

        public DependencyWithScope(IDependencyScope scope)
        {
            Scope = scope;
        }
    }
}