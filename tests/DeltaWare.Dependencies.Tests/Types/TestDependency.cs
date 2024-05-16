using System;

namespace DeltaWare.Dependencies.Tests.Types
{
    public class TestDependency
    {
        public TestDisposable TestDisposable { get; }

        public TestDependency(TestDisposable testDisposable, IDisposable message = null)
        {
            TestDisposable = testDisposable;
        }
    }
}