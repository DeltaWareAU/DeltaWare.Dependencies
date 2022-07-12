using DeltaWare.Dependencies.Abstractions;
using DeltaWare.Dependencies.Abstractions.Exceptions;
using DeltaWare.Dependencies.Tests.Types;
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

            collection.Register(() => new TestDisposable
            {
                IntValue = 171,
                StringValue = "Hello World"
            }).AsTransient();

            collection.Register<TestDependency>().AsTransient();
            
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

            collection.Register(() => new TestDisposable
            {
                IntValue = 171,
                StringValue = "Hello World"
            }).AsTransient();

            collection.Register<TestDependency>().AsTransient();

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

            collection.CheckIfAdded<TestDisposable>(c =>
            {
                c.TryRegister(() => new TestDisposable
                {
                    IntValue = 171,
                    StringValue = "Hello World"
                }).AsTransient();
            }).ShouldBeTrue();

            collection.CheckIfAdded<TestDependency>(c =>
            {
                c.TryRegister<TestDependency>().AsTransient();
            }).ShouldBeTrue();

            collection.CheckIfAdded<TestDisposable>(c =>
            {
                c.TryRegister(() => new TestDisposable()).AsTransient();
            }).ShouldBeFalse();

            collection.CheckIfAdded<TestDependency>(c =>
            {
                c.TryRegister<TestDependency>().AsTransient();
            }).ShouldBeFalse();
            
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

            collection.Register(() => new TestDisposable
            {
                IntValue = 171,
                StringValue = "Hello World"
            }).AsScoped();

            collection.Register<TestDependency>().AsScoped();
            
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

            collection.Register(() => new TestDisposable
            {
                IntValue = 171,
                StringValue = "Hello World"
            }).AsScoped();

            collection.Register<TestDependency>().AsScoped();
            
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

            collection.CheckIfAdded<TestDisposable>(c =>
            {
                c.TryRegister(() => new TestDisposable
                {
                    IntValue = 171,
                    StringValue = "Hello World"
                }).AsScoped();
            }).ShouldBeTrue();


            collection.CheckIfAdded<TestDependency>(c =>
            {
                c.TryRegister<TestDependency>().AsScoped();
            }).ShouldBeTrue();

            collection.CheckIfAdded<TestDisposable>(c =>
            {
                c.TryRegister(() => new TestDisposable()).AsScoped();
            }).ShouldBeFalse();


            collection.CheckIfAdded<TestDependency>(c =>
            {
                c.TryRegister<TestDependency>().AsScoped();
            }).ShouldBeFalse();
            
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

            collection.Register(() => new TestDisposable
            {
                IntValue = 171,
                StringValue = "Hello World"
            }).AsSingleton();

            collection.Register<TestDependency>().AsSingleton();

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

            collection.Register(() => new TestDisposable
            {
                IntValue = 171,
                StringValue = "Hello World"
            }).AsSingleton();

            collection.Register<TestDependency>().AsSingleton();

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

            collection.CheckIfAdded<TestDisposable>(c =>
            {
                c.TryRegister(() => new TestDisposable
                {
                    IntValue = 171,
                    StringValue = "Hello World"
                }).AsSingleton();
            }).ShouldBeTrue();

            collection.CheckIfAdded<TestDependency>(c =>
            {
                c.TryRegister<TestDependency>().AsSingleton();
            }).ShouldBeTrue();

            collection.CheckIfAdded<TestDisposable>(c =>
            {
                c.TryRegister(() => new TestDisposable
                {
                    IntValue = 171,
                    StringValue = "Hello World"
                }).AsSingleton();
            }).ShouldBeFalse();

            collection.CheckIfAdded<TestDependency>(c =>
            {
                c.TryRegister<TestDependency>().AsSingleton();
            }).ShouldBeFalse();

            collection.HasDependency<TestDisposable>().ShouldBeTrue();
            collection.HasDependency<TestDependency>().ShouldBeTrue();
        }

        #endregion Singleton

        [Fact]
        public void ThrowDependencyNotFoundException()
        {
            IDependencyCollection collection = new DependencyCollection();

            using ILifetimeScope scope = collection.CreateScope();

            using IDependencyProvider provider = Should.NotThrow(scope.BuildProvider);

            Should.Throw<DependencyNotFoundException>(provider.GetRequiredDependency<TestDisposable>);
        }
    }
}