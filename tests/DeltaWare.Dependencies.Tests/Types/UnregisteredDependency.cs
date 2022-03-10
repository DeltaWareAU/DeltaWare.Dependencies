using System;

namespace DeltaWare.Dependencies.Tests.Types
{
    internal class UnregisteredDependency: IDisposable
    {
        public TestDependency Dependency { get; }

        public bool IsDisposed { get; private set; }

        public UnregisteredDependency(TestDependency testDependency)
        {
            Dependency = testDependency;
        }

        public void Dispose()
        {
            IsDisposed = true;
        }
    }
}
