using DeltaWare.Dependencies.Abstractions;
using DeltaWare.Dependencies.Abstractions.Attributes;
using DeltaWare.Dependencies.Abstractions.Configuration;
using DeltaWare.Dependencies.Abstractions.Descriptor;
using DeltaWare.Dependencies.Abstractions.Descriptor.Enums;
using DeltaWare.Dependencies.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace DeltaWare.Dependencies.Descriptors
{
    [DebuggerDisplay("DefinitionType: {ImplementationType.Name} | Lifetime: {Lifetime} - Binding: {Binding}")]
    public abstract class DependencyDescriptorBase : IDependencyDescriptor
    {
        private readonly List<IConfiguration> _configuration = new();

        public Binding Binding { get; internal set; } = Binding.Bound;

        public Lifetime Lifetime { get; internal set; } = Lifetime.Transient;

        public Type ImplementationType { get; }

        protected DependencyDescriptorBase(Type implementationType)
        {
            ImplementationType = implementationType ?? throw new ArgumentNullException(nameof(implementationType));
        }

        public IDependencyInstance CreateInstance(IDependencyProvider provider)
        {
            object instance = InternalCreateInstance(provider);

            if (instance == null)
            {
                return null;
            }

            SetInjectableProperties(provider, instance);

            return ToDependencyInstance(provider, instance);
        }

        protected abstract object InternalCreateInstance(IDependencyProvider provider);

        protected virtual void ConfigureInstance(IDependencyProvider provider, object instance)
        {
            foreach (IConfiguration configuration in _configuration)
            {
                configuration.Configure(provider, instance);
            }
        }

        protected virtual void SetInjectableProperties(IDependencyProvider provider, object instance)
        {
            foreach (PropertyInfo injectableProperty in GetInjectableProperties(instance.GetType()))
            {
                bool cannotBeNull = injectableProperty.GetValue(instance) == null;

                object dependencyInstance = provider.GetDependency(injectableProperty.PropertyType);

                if (cannotBeNull && dependencyInstance == null)
                {
                    NullDependencyInstanceException.NullInjectablePropertyException(injectableProperty);
                }

                injectableProperty.SetValue(instance, provider.GetDependency(injectableProperty.PropertyType));
            }
        }

        private IEnumerable<PropertyInfo> GetInjectableProperties(Type type)
        {
            PropertyInfo[] publicProperties = type.GetProperties(BindingFlags.Public);

            foreach (PropertyInfo property in publicProperties)
            {
                if (property.HasAttribute<InjectAttribute>())
                {
                    yield return property;
                }
            }
        }

        internal DependencyInstance ToDependencyInstance(IDependencyProvider provider, object instance)
        {
            ConfigureInstance(provider, instance);

            return new DependencyInstance(instance, this);
        }

        public void AddConfiguration(IConfiguration configuration)
        {
            _configuration.Add(configuration);
        }
    }
}
