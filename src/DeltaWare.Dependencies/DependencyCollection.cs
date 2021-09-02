using DeltaWare.Dependencies.Abstractions;
using DeltaWare.Dependencies.Abstractions.Exceptions;
using DeltaWare.Dependencies.Interfaces;
using DeltaWare.Dependencies.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeltaWare.Dependencies
{
    /// <inheritdoc cref="IDependencyCollection"/>
    public class DependencyCollection : IDependencyCollection, ICloneable
    {
        private readonly Dictionary<Type, IDependencyDescriptor> _dependencies = new();

        private readonly object _scopeLock = new();
        private readonly Dictionary<Type, IDependencyInstance> _singletonInstances = new();

        public DependencyCollection()
        {
        }

        private DependencyCollection(Dictionary<Type, IDependencyDescriptor> dependencies)
        {
            _dependencies = dependencies;
        }

        public void AddDependency<TDependency>(Lifetime lifetime, Binding binding = Binding.Bound)
        {
            Type dependencyType = typeof(TDependency);

            IDependencyDescriptor dependencyDescriptor = new DependencyDescriptor<TDependency>(lifetime, binding);

            if (!_dependencies.TryAdd(dependencyType, dependencyDescriptor))
            {
                _dependencies[dependencyType] = dependencyDescriptor;
            }
        }

        /// <inheritdoc cref="IDependencyCollection.AddDependency{TDependency}(Func{TDependency}, Lifetime, Binding)"/>
        public void AddDependency<TDependency>(Func<TDependency> dependency, Lifetime lifetime, Binding binding = Binding.Bound)
        {
            if (dependency == null)
            {
                throw new ArgumentNullException(nameof(dependency));
            }

            Type dependencyType = typeof(TDependency);

            IDependencyDescriptor dependencyDescriptor = new ActionDescriptor<TDependency>(dependency, lifetime, binding);

            if (!_dependencies.TryAdd(dependencyType, dependencyDescriptor))
            {
                _dependencies[dependencyType] = dependencyDescriptor;
            }
        }

        /// <inheritdoc cref="IDependencyCollection.AddDependency{TDependency}(Func{IDependencyProvider, TDependency}, Lifetime, Binding)"/>
        public void AddDependency<TDependency>(Func<IDependencyProvider, TDependency> dependency, Lifetime lifetime, Binding binding = Binding.Bound)
        {
            if (dependency == null)
            {
                throw new ArgumentNullException(nameof(dependency));
            }

            Type dependencyType = typeof(TDependency);

            IDependencyDescriptor dependencyDescriptor = new ActionDescriptor<TDependency>(dependency, lifetime, binding);

            if (!_dependencies.TryAdd(dependencyType, dependencyDescriptor))
            {
                _dependencies[dependencyType] = dependencyDescriptor;
            }
        }

        public void AddDependency<TDependency, TImplementation>(Lifetime lifetime, Binding binding = Binding.Bound) where TDependency : TImplementation
        {
            Type dependencyType = typeof(TDependency);

            IDependencyDescriptor dependencyDescriptor = new ImplementationDescriptor<TDependency, TImplementation>(lifetime, binding);

            if (!_dependencies.TryAdd(dependencyType, dependencyDescriptor))
            {
                _dependencies[dependencyType] = dependencyDescriptor;
            }
        }

        /// <inheritdoc cref="IDependencyCollection.BuildProvider"/>
        public IDependencyProvider BuildProvider()
        {
            return new DependencyProvider(this);
        }

        public object Clone()
        {
            Dictionary<Type, IDependencyDescriptor> dependencies = new();

            foreach (KeyValuePair<Type, IDependencyDescriptor> dependency in _dependencies)
            {
                if (dependency.Value is ICloneable cloneable)
                {
                    dependencies.Add(dependency.Key, (IDependencyDescriptor)cloneable.Clone());
                }
                else
                {
                    dependencies.Add(dependency.Key, dependency.Value);
                }
            }

            return new DependencyCollection(dependencies);
        }

        public IDependencyDescriptor GetDependencyDescriptor(Type dependencyType)
        {
            if (_dependencies.TryGetValue(dependencyType, out IDependencyDescriptor descriptor))
            {
                return descriptor;
            }

            throw new DependencyNotFoundException(dependencyType);
        }

        public List<IDependencyDescriptor> GetDependencyDescriptors<TDependency>()
        {
            return _dependencies
                .Where(d => d.Key.GetInterfaces().Contains(typeof(TDependency)))
                .Select(d => d.Value)
                .ToList();
        }

        public IDependencyInstance GetSingletonInstance(IDependencyDescriptor descriptor, IDependencyProvider provider)
        {
            lock (_scopeLock)
            {
                if (_singletonInstances.TryGetValue(descriptor.Type, out IDependencyInstance instance))
                {
                    return instance;
                }

                instance = descriptor.CreateInstance(provider);

                _singletonInstances.Add(descriptor.Type, instance);

                return instance;
            }
        }

        /// <inheritdoc cref="IDependencyCollection.HasDependency{TDependency}"/>
        public bool HasDependency<TDependency>()
        {
            return HasDependency(typeof(TDependency));
        }

        public bool HasDependency(Type dependencyType)
        {
            return _dependencies.ContainsKey(dependencyType);
        }

        /// <inheritdoc cref="IDependencyCollection.TryAddDependency{TDependency}(Func{TDependency}, Lifetime, Binding)"/>
        public bool TryAddDependency<TDependency>(Func<TDependency> dependency, Lifetime lifetime, Binding binding = Binding.Bound)
        {
            if (dependency == null)
            {
                throw new ArgumentNullException(nameof(dependency));
            }

            Type dependencyType = typeof(TDependency);

            IDependencyDescriptor dependencyDescriptor = new ActionDescriptor<TDependency>(dependency, lifetime, binding);

            return _dependencies.TryAdd(dependencyType, dependencyDescriptor);
        }

        /// <inheritdoc cref="IDependencyCollection.TryAddDependency{TDependency}(Func{IDependencyProvider, TDependency}, Lifetime, Binding)"/>
        public bool TryAddDependency<TDependency>(Func<IDependencyProvider, TDependency> dependency, Lifetime lifetime, Binding binding = Binding.Bound)
        {
            if (dependency == null)
            {
                throw new ArgumentNullException(nameof(dependency));
            }

            Type dependencyType = typeof(TDependency);

            IDependencyDescriptor dependencyDescriptor = new ActionDescriptor<TDependency>(dependency, lifetime, binding);

            return _dependencies.TryAdd(dependencyType, dependencyDescriptor);
        }

        public bool TryAddDependency<TDependency, TImplementation>(Lifetime lifetime, Binding binding = Binding.Bound) where TDependency : TImplementation
        {
            Type dependencyType = typeof(TDependency);

            IDependencyDescriptor dependencyDescriptor = new ImplementationDescriptor<TDependency, TImplementation>(lifetime, binding);

            return _dependencies.TryAdd(dependencyType, dependencyDescriptor);
        }

        public bool TryAddDependency<TDependency>(Lifetime lifetime, Binding binding = Binding.Bound)
        {
            Type dependencyType = typeof(TDependency);

            IDependencyDescriptor dependencyDescriptor = new DependencyDescriptor<TDependency>(lifetime, binding);

            return _dependencies.TryAdd(dependencyType, dependencyDescriptor);
        }

        #region IDisposable

        private volatile bool _disposed;

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes all bound instances of singleton dependencies.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                lock (_scopeLock)
                {
                    foreach (IDependencyInstance dependencyInstance in _singletonInstances.Values)
                    {
                        if (dependencyInstance.Instance is IDisposable disposable)
                        {
                            disposable.Dispose();
                        }
                    }
                }
            }

            _disposed = true;
        }

        #endregion IDisposable
    }
}