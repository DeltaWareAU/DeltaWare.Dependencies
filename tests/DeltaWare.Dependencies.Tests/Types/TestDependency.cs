using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace DeltaWare.Dependencies.Tests.Types
{
    public class TestDependency
    {
        public TestDisposable TestDisposable { get; }

        public TestDependency(TestDisposable testDisposable, IMessageLogger message = null)
        {
            TestDisposable = testDisposable;
        }
    }
}