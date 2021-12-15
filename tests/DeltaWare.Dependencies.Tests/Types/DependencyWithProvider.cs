using DeltaWare.Dependencies.Abstractions;

namespace DeltaWare.Dependencies.Tests.Types
{
    public class DependencyWithProvider
    {
        public IDependencyProvider Provider { get; }

        public DependencyWithProvider(IDependencyProvider provider)
        {
            Provider = provider;
        }
    }
}