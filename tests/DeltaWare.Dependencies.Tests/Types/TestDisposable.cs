using System;

namespace DeltaWare.Dependencies.Tests.Types
{
    public class TestDisposable : IDisposable
    {
        public int IntValue { get; set; }
        public bool IsDisposed { get; private set; }
        public string StringValue { get; set; }

        public void Dispose()
        {
            IsDisposed = true;
        }
    }
}