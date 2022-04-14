using DeltaWare.Dependencies.Abstractions;
using DeltaWare.Dependencies.Abstractions.Exceptions;
using DeltaWare.Dependencies.Tests.Types;
using Shouldly;
using System;
using Xunit;

namespace DeltaWare.Dependencies.Tests
{
    public class DependencyProviderShould
    {
        [Fact]
        public void ConfigureDependency()
        {
            DateTime testDateTime = new DateTime(2000, 06, 01);
            int testInteger = 5;
            string testString = "Look, A wild string has appeared!";

            IDependencyCollection collection = new DependencyCollection();

            collection.AddSingleton<DependencyWithConfigure>();
            collection.Configure<DependencyWithConfigure>(c =>
            {
                c.MyDate = testDateTime;
            });
            collection.Configure<DependencyWithConfigure>(c =>
            {
                c.MyInt = testInteger;
                c.MyString = testString;
            });

            using IDependencyProvider provider = collection.BuildProvider();

            DependencyWithConfigure configureDependency = provider.GetDependency<DependencyWithConfigure>();

            configureDependency.ShouldNotBeNull();

            configureDependency.MyDate.ShouldBe(testDateTime);
            configureDependency.MyInt.ShouldBe(testInteger);
            configureDependency.MyString.ShouldBe(testString);
        }

        [Fact]
        public void ConfigureDependencyWithProvider()
        {
            string name = "John";
            int age = 26;
            DateTime birthDate = new DateTime(1908, 8, 5);

            IDependencyCollection collection = new DependencyCollection();

            collection.AddSingleton<Person>();
            collection.Configure<Person>(p =>
            {
                p.Name = name;
                p.Age = age;
                p.BirthDate = birthDate;
            });
            collection.AddScoped<DependencyWithConfigure>();
            collection.Configure<DependencyWithConfigure>((p, c) =>
            {
                Person person = p.GetDependency<Person>();

                c.MyString = person.Name;
                c.MyInt = person.Age;
                c.MyDate = person.BirthDate;
            });

            using IDependencyProvider provider = collection.BuildProvider();

            DependencyWithConfigure configureDependency = provider.GetDependency<DependencyWithConfigure>();

            configureDependency.ShouldNotBeNull();

            configureDependency.MyDate.ShouldBe(birthDate);
            configureDependency.MyInt.ShouldBe(age);
            configureDependency.MyString.ShouldBe(name);
        }

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
        public void GetDependencyWithProvider()
        {
            IDependencyCollection collection = new DependencyCollection();

            collection.AddScoped<DependencyWithProvider, DependencyWithProvider>();

            using (IDependencyProvider provider = collection.BuildProvider())
            {
                DependencyWithProvider dependency = Should.NotThrow(provider.GetDependency<DependencyWithProvider>);

                dependency.ShouldNotBeNull();
                dependency.Provider.ShouldNotBeNull();
                dependency.Provider.ShouldBe(provider);
            }

            collection = new DependencyCollection();

            collection.AddSingleton<DependencyWithProvider, DependencyWithProvider>();

            using (IDependencyProvider provider = collection.BuildProvider())
            {
                Should.Throw<SingletonDependencyException>(provider.GetDependency<DependencyWithProvider>);
            }
        }

        [Fact]
        public void GetDependencyWithScope()
        {
            IDependencyCollection collection = new DependencyCollection();

            collection.AddSingleton<DependencyWithScope, DependencyWithScope>();

            using (IDependencyProvider provider = collection.BuildProvider())
            {
                DependencyWithScope dependency = Should.NotThrow(provider.GetDependency<DependencyWithScope>);

                dependency.ShouldNotBeNull();
                dependency.Scope.ShouldNotBeNull();
            }
        }

        [Fact]
        public void ThrowCircularDependencyException()
        {
            IDependencyCollection collection = new DependencyCollection();

            collection.AddScoped<CircularDependencyA>();
            collection.AddScoped<CircularDependencyB>();

            using IDependencyScope scope = collection.CreateScope();
            using IDependencyProvider provider = scope.BuildProvider();

            Should.Throw<CircularDependencyException>(provider.GetDependency<CircularDependencyA>);
        }

        [Fact]
        public void ThrowDependencyNotFoundException()
        {
            IDependencyCollection collection = new DependencyCollection();

            using IDependencyScope scope = collection.CreateScope();

            using IDependencyProvider provider = Should.NotThrow(scope.BuildProvider);

            Should.Throw<DependencyNotFoundException>(provider.GetRequiredDependency<TestDisposable>);
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

        [Fact]
        public void InstantiateUnregisteredDependency()
        {
            IDependencyCollection collection = new DependencyCollection();

            int intValue = 171;
            string stringValue = "Hello World";

            collection.AddScoped(() => new TestDisposable
            {
                IntValue = intValue,
                StringValue = stringValue
            });
            collection.AddScoped<TestDependency>();

            using IDependencyScope scope = collection.CreateScope();

            using IDependencyProvider provider = Should.NotThrow(scope.BuildProvider);

            UnregisteredDependency unregistered = Should.NotThrow(provider.CreateInstance<UnregisteredDependency>);

            unregistered.IsDisposed.ShouldBeFalse();

            unregistered.Dependency.TestDisposable.IntValue.ShouldBe(intValue);
            unregistered.Dependency.TestDisposable.StringValue.ShouldBe(stringValue);
            unregistered.Dependency.TestDisposable.IsDisposed.ShouldBeFalse();

            provider.Dispose();

            unregistered.IsDisposed.ShouldBeTrue();
            unregistered.Dependency.TestDisposable.IsDisposed.ShouldBeTrue();
        }

        [Fact]
        public void ThrowSingletonDependencyExceptionInstantiateUnregistered()
        {
            IDependencyCollection collection = new DependencyCollection();

            collection.AddScoped<TestDisposable>();
            collection.AddSingleton<TestDependency>();

            using IDependencyScope scope = collection.CreateScope();

            using IDependencyProvider provider = Should.NotThrow(scope.BuildProvider);

            Should.Throw<SingletonDependencyException>(provider.CreateInstance<UnregisteredDependency>);
        }
    }
}