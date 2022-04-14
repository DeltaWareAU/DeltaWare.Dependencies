using DeltaWare.Dependencies.Abstractions;
using DeltaWare.Dependencies.Abstractions.Descriptors;
using DeltaWare.Dependencies.Abstractions.Enums;
using DeltaWare.Dependencies.Abstractions.Exceptions;
using DeltaWare.Dependencies.Configuration;
using DeltaWare.Dependencies.Descriptors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DeltaWare.Dependencies
{
    /// <inheritdoc cref="IDependencyCollection"/>
    [DebuggerDisplay("Count {_dependencies.Count}")]
    public class DependencyCollection : IDependencyCollection, ICloneable
    {
        private readonly Dictionary<Type, IDependencyDescriptor> _dependencies = new();

        public DependencyCollection()
        {
        }

        private DependencyCollection(Dictionary<Type, IDependencyDescriptor> dependencies)
        {
            _dependencies = dependencies;
        }

        public void AddDependency(IDependencyDescriptor dependencyDescriptor)
        {
            if (dependencyDescriptor == null)
            {
                throw new ArgumentNullException(nameof(dependencyDescriptor));
            }

            if (!_dependencies.TryAdd(dependencyDescriptor.Type, dependencyDescriptor))
            {
                _dependencies[dependencyDescriptor.Type] = dependencyDescriptor;
            }
        }

        public IDependencyDescriptor AddDependency<TDependency>(Lifetime lifetime, Binding binding = Binding.Bound) where TDependency : class
        {
            IDependencyDescriptor dependencyDescriptor = new TypeDependencyDescriptor(typeof(TDependency), typeof(TDependency), lifetime, binding);

            AddDependency(dependencyDescriptor);

            return dependencyDescriptor;
        }

        /// <inheritdoc cref="IDependencyCollection.AddDependency{TDependency}(Func{TDependency}, Lifetime, Binding)"/>
        public IDependencyDescriptor AddDependency<TDependency>(Func<TDependency> dependency, Lifetime lifetime, Binding binding = Binding.Bound) where TDependency : class
        {
            if (dependency == null)
            {
                throw new ArgumentNullException(nameof(dependency));
            }

            IDependencyDescriptor dependencyDescriptor = new InstanceDependencyDescriptor(typeof(TDependency), dependency, lifetime, binding);

            AddDependency(dependencyDescriptor);

            return dependencyDescriptor;
        }

        /// <inheritdoc cref="IDependencyCollection.AddDependency{TDependency}(Func{IDependencyProvider, TDependency}, Lifetime, Binding)"/>
        public IDependencyDescriptor AddDependency<TDependency>(Func<IDependencyProvider, TDependency> dependency, Lifetime lifetime, Binding binding = Binding.Bound) where TDependency : class
        {
            if (dependency == null)
            {
                throw new ArgumentNullException(nameof(dependency));
            }

            IDependencyDescriptor dependencyDescriptor = new ImplementationDependencyDescriptor(typeof(TDependency), dependency, lifetime, binding);

            AddDependency(dependencyDescriptor);

            return dependencyDescriptor;
        }

        public IDependencyDescriptor AddDependency<TDependency, TImplementation>(Lifetime lifetime, Binding binding = Binding.Bound) where TImplementation : TDependency where TDependency : class
        {
            IDependencyDescriptor dependencyDescriptor = new TypeDependencyDescriptor(typeof(TDependency), typeof(TImplementation), lifetime, binding);

            AddDependency(dependencyDescriptor);

            return dependencyDescriptor;
        }

        public IDependencyProvider BuildProvider()
        {
            return new DependencyProvider((IReadOnlyDependencyCollection)Clone());
        }

        public object Clone()
        {
            Dictionary<Type, IDependencyDescriptor> dependencies = new();

            foreach ((Type key, IDependencyDescriptor value) in _dependencies)
            {
                dependencies.Add(key, value);
            }

            return new DependencyCollection(dependencies);
        }

        public void Configure<TDependency>(Action<TDependency> configuration) where TDependency : class
        {
            if (!_dependencies.TryGetValue(typeof(TDependency), out IDependencyDescriptor dependencyDescriptor))
            {
                throw new DependencyNotFoundException(typeof(TDependency));
            }

            DependencyDescriptorBase descriptor = (DependencyDescriptorBase)dependencyDescriptor;

            descriptor.AddConfiguration(new TypeConfiguration<TDependency>(configuration));
        }

        public void Configure<TDependency>(Action<IDependencyProvider, TDependency> configuration) where TDependency : class
        {
            if (!_dependencies.TryGetValue(typeof(TDependency), out IDependencyDescriptor dependencyDescriptor))
            {
                throw new DependencyNotFoundException(typeof(TDependency));
            }

            DependencyDescriptorBase descriptor = (DependencyDescriptorBase)dependencyDescriptor;

            descriptor.AddConfiguration(new ProviderConfiguration<TDependency>(configuration));
        }

        public IDependencyScope CreateScope()
        {
            return new DependencyScope((IReadOnlyDependencyCollection)Clone());
        }

        public IDependencyDescriptor GetDependencyDescriptor(Type dependencyType)
        {
            if (_dependencies.TryGetValue(dependencyType, out IDependencyDescriptor descriptor))
            {
                return descriptor;
            }

            return null;
        }

        public IEnumerable<IDependencyDescriptor> GetDependencyDescriptors(Type dependencyType)
        {
            return _dependencies
                .Where(d => d.Key.GetInterfaces().Contains(dependencyType))
                .Select(d => d.Value);
        }

        public bool HasDependency(Type dependencyType)
        {
            return _dependencies.ContainsKey(dependencyType);
        }

        public bool Remove<TDependency>() where TDependency : class
        {
            return Remove(typeof(TDependency));
        }

        public bool Remove(Type dependencyType)
        {
            return _dependencies.Remove(dependencyType);
        }

        public bool TryAddDependency(IDependencyDescriptor dependencyDescriptor)
        {
            return _dependencies.TryAdd(dependencyDescriptor.Type, dependencyDescriptor);
        }

        public bool TryAddDependency<TDependency>(Func<TDependency> dependency, Lifetime lifetime, Binding binding, out IDependencyDescriptor dependencyDescriptor) where TDependency : class
        {
            if (dependency == null)
            {
                throw new ArgumentNullException(nameof(dependency));
            }

            IDependencyDescriptor descriptor = new InstanceDependencyDescriptor(typeof(TDependency), dependency, lifetime, binding);

            bool added = TryAddDependency(descriptor);

            dependencyDescriptor = added ? descriptor : null;

            return added;
        }

        public bool TryAddDependency<TDependency>(Func<IDependencyProvider, TDependency> dependency, Lifetime lifetime, Binding binding, out IDependencyDescriptor dependencyDescriptor) where TDependency : class
        {
            if (dependency == null)
            {
                throw new ArgumentNullException(nameof(dependency));
            }

            IDependencyDescriptor descriptor = new ImplementationDependencyDescriptor(typeof(TDependency), dependency, lifetime, binding);

            bool added = TryAddDependency(descriptor);

            dependencyDescriptor = added ? descriptor : null;

            return added;
        }

        public bool TryAddDependency<TDependency, TImplementation>(Lifetime lifetime, Binding binding, out IDependencyDescriptor dependencyDescriptor) where TImplementation : TDependency where TDependency : class
        {
            IDependencyDescriptor descriptor = new TypeDependencyDescriptor(typeof(TDependency), typeof(TImplementation), lifetime, binding);

            bool added = TryAddDependency(descriptor);

            dependencyDescriptor = added ? descriptor : null;

            return added;
        }

        public bool TryAddDependency<TDependency>(Lifetime lifetime, Binding binding, out IDependencyDescriptor dependencyDescriptor) where TDependency : class
        {
            IDependencyDescriptor descriptor = new TypeDependencyDescriptor(typeof(TDependency), typeof(TDependency), lifetime, binding);

            bool added = TryAddDependency(descriptor);

            dependencyDescriptor = added ? descriptor : null;

            return added;
        }
    }
}