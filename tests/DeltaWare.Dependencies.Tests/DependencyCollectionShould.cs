using DeltaWare.Dependencies.Abstractions;
using DeltaWare.Dependencies.Abstractions.Exceptions;
using Shouldly;
using Xunit;

namespace DeltaWare.Dependencies.Tests
{
    public class DependencyCollectionShould
    {
        #region Transient

        [Fact]
        public void AddTransient()
        {
            IDependencyCollection collection = new DependencyCollection();

            collection.HasDependency<TestDisposable>().ShouldBeFalse();
            collection.HasDependency<TestDependency>().ShouldBeFalse();

            collection.AddTransient(() => new TestDisposable
            {
                IntValue = 171,
                StringValue = "Hello World"
            });

            collection.AddTransient<TestDependency, TestDependency>();

            collection.HasDependency<TestDisposable>().ShouldBeTrue();
            collection.HasDependency<TestDependency>().ShouldBeTrue();
        }

        [Fact]
        public void RemoveTransient()
        {
            IDependencyCollection collection = new DependencyCollection();

            collection.HasDependency<TestDisposable>().ShouldBeFalse();
            collection.HasDependency<TestDependency>().ShouldBeFalse();

            collection.Remove<TestDependency>().ShouldBeFalse();

            collection.AddTransient(() => new TestDisposable
            {
                IntValue = 171,
                StringValue = "Hello World"
            });

            collection.AddTransient<TestDependency, TestDependency>();

            collection.HasDependency<TestDisposable>().ShouldBeTrue();
            collection.HasDependency<TestDependency>().ShouldBeTrue();

            collection.Remove<TestDependency>().ShouldBeTrue();

            collection.HasDependency<TestDisposable>().ShouldBeTrue();
            collection.HasDependency<TestDependency>().ShouldBeFalse();
        }

        [Fact]
        public void TryAddTransient()
        {
            IDependencyCollection collection = new DependencyCollection();

            collection.HasDependency<TestDisposable>().ShouldBeFalse();
            collection.HasDependency<TestDependency>().ShouldBeFalse();

            collection.TryAddTransient(() => new TestDisposable
            {
                IntValue = 171,
                StringValue = "Hello World"
            }).ShouldBeTrue();

            collection.TryAddTransient<TestDependency, TestDependency>().ShouldBeTrue();

            collection.TryAddTransient(() => new TestDisposable()).ShouldBeFalse();
            collection.TryAddTransient<TestDependency, TestDependency>().ShouldBeFalse();

            collection.HasDependency<TestDisposable>().ShouldBeTrue();
            collection.HasDependency<TestDependency>().ShouldBeTrue();
        }

        #endregion Transient

        #region Scoped

        [Fact]
        public void AddScoped()
        {
            IDependencyCollection collection = new DependencyCollection();

            collection.HasDependency<TestDisposable>().ShouldBeFalse();
            collection.HasDependency<TestDependency>().ShouldBeFalse();

            collection.AddScoped(() => new TestDisposable
            {
                IntValue = 171,
                StringValue = "Hello World"
            });

            collection.AddScoped<TestDependency, TestDependency>();

            collection.HasDependency<TestDisposable>().ShouldBeTrue();
            collection.HasDependency<TestDependency>().ShouldBeTrue();
        }

        [Fact]
        public void RemoveScoped()
        {
            IDependencyCollection collection = new DependencyCollection();

            collection.HasDependency<TestDisposable>().ShouldBeFalse();
            collection.HasDependency<TestDependency>().ShouldBeFalse();

            collection.Remove<TestDependency>().ShouldBeFalse();

            collection.AddScoped(() => new TestDisposable
            {
                IntValue = 171,
                StringValue = "Hello World"
            });

            collection.AddScoped<TestDependency, TestDependency>();

            collection.HasDependency<TestDisposable>().ShouldBeTrue();
            collection.HasDependency<TestDependency>().ShouldBeTrue();

            collection.Remove<TestDependency>().ShouldBeTrue();

            collection.HasDependency<TestDisposable>().ShouldBeTrue();
            collection.HasDependency<TestDependency>().ShouldBeFalse();
        }

        [Fact]
        public void TryAddScoped()
        {
            IDependencyCollection collection = new DependencyCollection();

            collection.HasDependency<TestDisposable>().ShouldBeFalse();
            collection.HasDependency<TestDependency>().ShouldBeFalse();

            collection.TryAddScoped(() => new TestDisposable
            {
                IntValue = 171,
                StringValue = "Hello World"
            }).ShouldBeTrue();

            collection.TryAddScoped<TestDependency, TestDependency>().ShouldBeTrue();

            collection.TryAddScoped(() => new TestDisposable()).ShouldBeFalse();
            collection.TryAddScoped<TestDependency, TestDependency>().ShouldBeFalse();

            collection.HasDependency<TestDisposable>().ShouldBeTrue();
            collection.HasDependency<TestDependency>().ShouldBeTrue();
        }

        #endregion Scoped

        #region Singleton

        [Fact]
        public void AddSingleton()
        {
            IDependencyCollection collection = new DependencyCollection();

            collection.HasDependency<TestDisposable>().ShouldBeFalse();
            collection.HasDependency<TestDependency>().ShouldBeFalse();

            collection.AddSingleton(() => new TestDisposable
            {
                IntValue = 171,
                StringValue = "Hello World"
            });

            collection.AddSingleton<TestDependency, TestDependency>();

            collection.HasDependency<TestDisposable>().ShouldBeTrue();
            collection.HasDependency<TestDependency>().ShouldBeTrue();
        }

        [Fact]
        public void RemoveSingleton()
        {
            IDependencyCollection collection = new DependencyCollection();

            collection.HasDependency<TestDisposable>().ShouldBeFalse();
            collection.HasDependency<TestDependency>().ShouldBeFalse();

            collection.Remove<TestDependency>().ShouldBeFalse();

            collection.AddSingleton(() => new TestDisposable
            {
                IntValue = 171,
                StringValue = "Hello World"
            });

            collection.AddSingleton<TestDependency, TestDependency>();

            collection.HasDependency<TestDisposable>().ShouldBeTrue();
            collection.HasDependency<TestDependency>().ShouldBeTrue();

            collection.Remove<TestDependency>().ShouldBeTrue();

            collection.HasDependency<TestDisposable>().ShouldBeTrue();
            collection.HasDependency<TestDependency>().ShouldBeFalse();
        }

        [Fact]
        public void TryAddSingleton()
        {
            IDependencyCollection collection = new DependencyCollection();

            collection.HasDependency<TestDisposable>().ShouldBeFalse();
            collection.HasDependency<TestDependency>().ShouldBeFalse();

            collection.TryAddSingleton(() => new TestDisposable
            {
                IntValue = 171,
                StringValue = "Hello World"
            }).ShouldBeTrue();

            collection.TryAddSingleton<TestDependency, TestDependency>().ShouldBeTrue();

            collection.TryAddSingleton(() => new TestDisposable()).ShouldBeFalse();
            collection.TryAddSingleton<TestDependency, TestDependency>().ShouldBeFalse();

            collection.HasDependency<TestDisposable>().ShouldBeTrue();
            collection.HasDependency<TestDependency>().ShouldBeTrue();
        }

        #endregion Singleton

        [Fact]
        public void ThrowDependencyNotFoundException()
        {
            IDependencyCollection collection = new DependencyCollection();

            using IDependencyScope scope = collection.CreateScope();

            using IDependencyProvider provider = Should.NotThrow(scope.BuildProvider);

            Should.Throw<DependencyNotFoundException>(provider.GetDependency<TestDisposable>);
        }
    }
}