using DeltaWare.Dependencies.Abstractions;
using DeltaWare.Dependencies.Abstractions.Descriptor;
using DeltaWare.Dependencies.Abstractions.Registration;
using DeltaWare.Dependencies.Abstractions.Resolver;
using DeltaWare.Dependencies.Configuration;
using DeltaWare.Dependencies.Descriptors;
using DeltaWare.Dependencies.Registrations;
using DeltaWare.Dependencies.Resolver;
using System;
using System.Collections.Generic;

namespace DeltaWare.Dependencies
{
    public class DependencyCollection : IDependencyCollection
    {
        private readonly Dictionary<Type, DependencyDescriptorBase> _descriptors = new();

        private readonly DependencyResolverBase _resolver;

        public DependencyCollection()
        {
            _resolver = new DependencyCollectionResolver(this);
        }

        public IDependencyDescriptor Get(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (_descriptors.TryGetValue(type, out DependencyDescriptorBase descriptor))
            {
                return descriptor;
            }

            return null;
        }

        internal void InternalRegister(Type type, DependencyDescriptorBase descriptor)
        {
            if (_descriptors.ContainsKey(type))
            {
                _descriptors[type] = descriptor;
            }
            else
            {
                _descriptors.Add(type, descriptor);
            }
        }

        internal bool InternalTryRegister(Type type, DependencyDescriptorBase descriptor)
        {
            if (_descriptors.ContainsKey(type))
            {
                return false;
            }

            _descriptors.Add(type, descriptor);

            return true;
        }

        public IRegistrationDefinition<TImplementation> Register<TImplementation>(Func<TImplementation> builder)
        {
            return Register<TImplementation>(new ReferenceDependencyDescriptor(typeof(TImplementation), _ => builder.Invoke()));
        }

        public IRegistrationDefinition<TImplementation> Register<TImplementation>(Func<IDependencyProvider, TImplementation> builder)
        {
            return Register<TImplementation>(new ReferenceDependencyDescriptor(typeof(TImplementation), p => builder.Invoke(p)));
        }

        public IRegistrationDefinition<TImplementation> Register<TImplementation>()
        {
            return Register<TImplementation>(new TypeDependencyDescriptor(typeof(TImplementation)));
        }

        public IRegistrationDefinition<TImplementation> TryRegister<TImplementation>(Func<TImplementation> builder)
        {
            return TryRegister<TImplementation>(new ReferenceDependencyDescriptor(typeof(TImplementation), _ => builder.Invoke()));
        }

        public IRegistrationDefinition<TImplementation> TryRegister<TImplementation>(Func<IDependencyProvider, TImplementation> builder)
        {
            return TryRegister<TImplementation>(new ReferenceDependencyDescriptor(typeof(TImplementation), p => builder.Invoke(p)));
        }

        public IRegistrationDefinition<TImplementation> TryRegister<TImplementation>()
        {
            return TryRegister<TImplementation>(new TypeDependencyDescriptor(typeof(TImplementation)));
        }

        public IRegistrationDefinition<TImplementation> Register<TImplementation>(DependencyDescriptorBase descriptor)
        {
            return new RegistrationBuilder<TImplementation>(this, descriptor, true);
        }

        public IRegistrationDefinition<TImplementation> TryRegister<TImplementation>(DependencyDescriptorBase descriptor)
        {
            return new RegistrationBuilder<TImplementation>(this, descriptor, false);
        }

        public ILifetimeScope CreateScope()
        {
            return new LifetimeScope(_resolver);
        }

        public IDependencyProvider BuildProvider()
        {
            return new DependencyProvider(new LifetimeScope(_resolver));
        }

        public bool Contains(Type type)
        {
            return _descriptors.ContainsKey(type);
        }

        public bool Remove(Type definition)
        {
            return _descriptors.Remove(definition);
        }

        public void Configure<TDefinition>(Action<TDefinition> configuration)
        {
            if (_descriptors.TryGetValue(typeof(TDefinition), out DependencyDescriptorBase descriptor))
            {
                descriptor.AddConfiguration(new InstanceConfiguration<TDefinition>(configuration));
            }
        }

        public void Configure<TDefinition>(Action<IDependencyProvider, TDefinition> configuration)
        {
            if (_descriptors.TryGetValue(typeof(TDefinition), out DependencyDescriptorBase descriptor))
            {
                descriptor.AddConfiguration(new ProviderConfiguration<TDefinition>(configuration));
            }
        }

        public void AddResolver(DependencyResolverBase dependencyResolver)
        {
            var resolver = _resolver;

            while (resolver.InnerResolver != null)
            {
                resolver = resolver.InnerResolver;
            }

            resolver.InnerResolver = dependencyResolver;
        }

        public void AddResolver<TResolver>() where TResolver : DependencyResolverBase
        {
            AddResolver(Activator.CreateInstance<TResolver>());
        }
    }
}
