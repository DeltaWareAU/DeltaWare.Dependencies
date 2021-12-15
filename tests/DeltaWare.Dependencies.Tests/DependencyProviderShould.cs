using DeltaWare.Dependencies.Abstractions;
using DeltaWare.Dependencies.Abstractions.Exceptions;
using Shouldly;
using Xunit;

namespace DeltaWare.Dependencies.Tests
{
    public class DependencyProviderShould
    {
        [Fact]
        public void GetAddedScoped()
        {
            TestDisposable disposableA;
            TestDisposable disposableB;
            TestDependency dependencyA;

            IDependencyCollection collection = new DependencyCollection();

            collection.AddScoped(() => new TestDisposable
            {
                IntValue = 171,
                StringValue = "Hello World"
            });

            collection.AddScoped<TestDependency, TestDependency>();

            collection.TryAddScoped(() => new TestDisposable()).ShouldBeFalse();
            collection.TryAddScoped<TestDependency, TestDependency>().ShouldBeFalse();

            collection.HasDependency<TestDisposable>().ShouldBeTrue();
            collection.HasDependency<TestDependency>().ShouldBeTrue();

            using (IDependencyScope scope = collection.CreateScope())
            {
                using (IDependencyProvider provider = Should.NotThrow(scope.BuildProvider))
                {
                    provider.HasDependency<TestDisposable>().ShouldBeTrue();

                    disposableA = Should.NotThrow(provider.GetDependency<TestDisposable>);
                    disposableA.IsDisposed.ShouldBeFalse();
                    disposableA.IntValue.ShouldBe(171);
                    disposableA.StringValue.ShouldBe("Hello World");

                    disposableA.IntValue = 1024;
                    disposableA.StringValue = "No longer hello world";

                    disposableB = Should.NotThrow(provider.GetDependency<TestDisposable>);
                    disposableB.IsDisposed.ShouldBeFalse();
                    disposableB.IntValue.ShouldBe(1024);
                    disposableB.StringValue.ShouldBe("No longer hello world");

                    dependencyA = Should.NotThrow(provider.GetDependency<TestDependency>);
                    dependencyA.TestDisposable.IsDisposed.ShouldBeFalse();
                    dependencyA.TestDisposable.IntValue.ShouldBe(1024);
                    dependencyA.TestDisposable.StringValue.ShouldBe("No longer hello world");
                }

                disposableA.IsDisposed.ShouldBeTrue();
                disposableB.IsDisposed.ShouldBeTrue();

                using (IDependencyProvider provider = Should.NotThrow(scope.BuildProvider))
                {
                    provider.HasDependency<TestDisposable>().ShouldBeTrue();

                    disposableA = Should.NotThrow(provider.GetDependency<TestDisposable>);
                    disposableA.IsDisposed.ShouldBeFalse();
                    disposableA.IntValue.ShouldBe(171);
                    disposableA.StringValue.ShouldBe("Hello World");

                    dependencyA = Should.NotThrow(provider.GetDependency<TestDependency>);
                    dependencyA.TestDisposable.IsDisposed.ShouldBeFalse();
                    dependencyA.TestDisposable.IntValue.ShouldBe(171);
                    dependencyA.TestDisposable.StringValue.ShouldBe("Hello World");
                }

                disposableA.IsDisposed.ShouldBeTrue();
                disposableB.IsDisposed.ShouldBeTrue();
            }

            disposableA.IsDisposed.ShouldBeTrue();
            disposableB.IsDisposed.ShouldBeTrue();

            IDependencyScope dependencyScope = collection.CreateScope();

            IDependencyProvider dependencyProvider = dependencyScope.BuildProvider();

            TestDisposable disposable = dependencyProvider.GetDependency<TestDisposable>();

            disposable.IsDisposed.ShouldBeFalse();

            dependencyScope.Dispose();

            disposable.IsDisposed.ShouldBeTrue();
        }

        [Fact]
        public void GetAddedSingleton()
        {
            TestDisposable disposableA;
            TestDisposable disposableB;
            TestDependency dependencyA;

            IDependencyCollection collection = new DependencyCollection();

            collection.AddSingleton(() => new TestDisposable
            {
                IntValue = 171,
                StringValue = "Hello World"
            });

            collection.AddSingleton<TestDependency, TestDependency>();

            collection.TryAddSingleton(() => new TestDisposable()).ShouldBeFalse();
            collection.TryAddSingleton<TestDependency, TestDependency>().ShouldBeFalse();

            collection.HasDependency<TestDisposable>().ShouldBeTrue();
            collection.HasDependency<TestDependency>().ShouldBeTrue();

            using (IDependencyScope scope = collection.CreateScope())
            {
                using (IDependencyProvider provider = Should.NotThrow(scope.BuildProvider))
                {
                    provider.HasDependency<TestDisposable>().ShouldBeTrue();
                    provider.HasDependency<TestDependency>().ShouldBeTrue();

                    disposableA = Should.NotThrow(provider.GetDependency<TestDisposable>);
                    disposableA.IsDisposed.ShouldBeFalse();
                    disposableA.IntValue.ShouldBe(171);
                    disposableA.StringValue.ShouldBe("Hello World");

                    disposableA.IntValue = 1024;
                    disposableA.StringValue = "No longer hello world";

                    disposableB = Should.NotThrow(provider.GetDependency<TestDisposable>);
                    disposableB.IsDisposed.ShouldBeFalse();
                    disposableB.IntValue.ShouldBe(1024);
                    disposableB.StringValue.ShouldBe("No longer hello world");

                    dependencyA = Should.NotThrow(provider.GetDependency<TestDependency>);
                    dependencyA.TestDisposable.IsDisposed.ShouldBeFalse();
                    dependencyA.TestDisposable.IntValue.ShouldBe(1024);
                    dependencyA.TestDisposable.StringValue.ShouldBe("No longer hello world");
                }

                disposableA.IsDisposed.ShouldBeFalse();
                disposableB.IsDisposed.ShouldBeFalse();

                using (IDependencyProvider provider = Should.NotThrow(scope.BuildProvider))
                {
                    provider.HasDependency<TestDisposable>().ShouldBeTrue();

                    disposableA = Should.NotThrow(provider.GetDependency<TestDisposable>);
                    disposableA.IsDisposed.ShouldBeFalse();
                    disposableA.IntValue.ShouldBe(1024);
                    disposableA.StringValue.ShouldBe("No longer hello world");

                    dependencyA = Should.NotThrow(provider.GetDependency<TestDependency>);
                    dependencyA.TestDisposable.IsDisposed.ShouldBeFalse();
                    dependencyA.TestDisposable.IntValue.ShouldBe(1024);
                    dependencyA.TestDisposable.StringValue.ShouldBe("No longer hello world");
                }

                disposableA.IsDisposed.ShouldBeFalse();
                disposableB.IsDisposed.ShouldBeFalse();
            }

            disposableA.IsDisposed.ShouldBeTrue();
            disposableB.IsDisposed.ShouldBeTrue();

            IDependencyScope dependencyScope = collection.CreateScope();

            IDependencyProvider dependencyProvider = dependencyScope.BuildProvider();

            TestDisposable disposable = dependencyProvider.GetDependency<TestDisposable>();

            disposable.IsDisposed.ShouldBeFalse();

            dependencyScope.Dispose();

            disposable.IsDisposed.ShouldBeTrue();
        }

        [Fact]
        public void GetAddedTransient()
        {
            TestDisposable disposableA;
            TestDisposable disposableB;
            TestDependency dependencyA;

            IDependencyCollection collection = new DependencyCollection();

            collection.AddTransient(() => new TestDisposable
            {
                IntValue = 171,
                StringValue = "Hello World"
            });

            collection.AddTransient<TestDependency, TestDependency>();

            collection.TryAddTransient(() => new TestDisposable()).ShouldBeFalse();
            collection.TryAddTransient<TestDependency, TestDependency>().ShouldBeFalse();

            collection.HasDependency<TestDisposable>().ShouldBeTrue();
            collection.HasDependency<TestDependency>().ShouldBeTrue();

            using (IDependencyScope scope = collection.CreateScope())
            {
                using (IDependencyProvider provider = Should.NotThrow(scope.BuildProvider))
                {
                    provider.HasDependency<TestDisposable>().ShouldBeTrue();

                    disposableA = Should.NotThrow(provider.GetDependency<TestDisposable>);
                    disposableA.IsDisposed.ShouldBeFalse();
                    disposableA.IntValue.ShouldBe(171);
                    disposableA.StringValue.ShouldBe("Hello World");

                    disposableA.IntValue = 1024;
                    disposableA.StringValue = "No longer hello world";

                    disposableB = Should.NotThrow(provider.GetDependency<TestDisposable>);
                    disposableB.IsDisposed.ShouldBeFalse();
                    disposableB.IntValue.ShouldBe(171);
                    disposableB.StringValue.ShouldBe("Hello World");

                    dependencyA = Should.NotThrow(provider.GetDependency<TestDependency>);
                    dependencyA.TestDisposable.IsDisposed.ShouldBeFalse();
                    dependencyA.TestDisposable.IntValue.ShouldBe(171);
                    dependencyA.TestDisposable.StringValue.ShouldBe("Hello World");
                }

                disposableA.IsDisposed.ShouldBeTrue();
                disposableB.IsDisposed.ShouldBeTrue();

                using (IDependencyProvider provider = Should.NotThrow(scope.BuildProvider))
                {
                    provider.HasDependency<TestDisposable>().ShouldBeTrue();

                    disposableA = Should.NotThrow(provider.GetDependency<TestDisposable>);
                    disposableA.IsDisposed.ShouldBeFalse();
                    disposableA.IntValue.ShouldBe(171);
                    disposableA.StringValue.ShouldBe("Hello World");

                    dependencyA = Should.NotThrow(provider.GetDependency<TestDependency>);
                    dependencyA.TestDisposable.IsDisposed.ShouldBeFalse();
                    dependencyA.TestDisposable.IntValue.ShouldBe(171);
                    dependencyA.TestDisposable.StringValue.ShouldBe("Hello World");
                }

                disposableA.IsDisposed.ShouldBeTrue();
                disposableB.IsDisposed.ShouldBeTrue();
            }

            disposableA.IsDisposed.ShouldBeTrue();
            disposableB.IsDisposed.ShouldBeTrue();

            IDependencyScope dependencyScope = collection.CreateScope();

            IDependencyProvider dependencyProvider = dependencyScope.BuildProvider();

            TestDisposable disposable = dependencyProvider.GetDependency<TestDisposable>();

            disposable.IsDisposed.ShouldBeFalse();

            dependencyScope.Dispose();

            disposable.IsDisposed.ShouldBeTrue();
        }

        [Fact]
        public void ThrowDependencyNotFoundException()
        {
            IDependencyCollection collection = new DependencyCollection();

            using IDependencyScope scope = collection.CreateScope();

            using IDependencyProvider provider = Should.NotThrow(scope.BuildProvider);

            Should.Throw<DependencyNotFoundException>(provider.GetDependency<TestDisposable>);
        }

        [Fact]
        public void ThrowSingletonDependencyException()
        {
            IDependencyCollection collection = new DependencyCollection();

            collection.AddScoped<TestDisposable>();
            collection.AddSingleton<TestDependency>();

            using IDependencyScope scope = collection.CreateScope();

            using IDependencyProvider provider = Should.NotThrow(scope.BuildProvider);

            Should.Throw<SingletonDependencyException>(provider.GetDependency<TestDependency>);
        }
    }
}