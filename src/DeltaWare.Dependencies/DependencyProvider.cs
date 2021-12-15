using DeltaWare.Dependencies.Abstractions;
using DeltaWare.Dependencies.Abstractions.Exceptions;
using DeltaWare.Dependencies.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DeltaWare.Dependencies
{
    internal class DependencyProvider : IDependencyProvider
    {
        private readonly Dictionary<Type, IDependencyInstance> _scopedInstances = new();

        private readonly List<IDisposable> _disposableInstances = new();

        private readonly IReadOnlyDependencyCollection _sourceCollection;

        private readonly DependencyScope _parentScope;

        public DependencyProvider(DependencyScope scope, IReadOnlyDependencyCollection sourceCollection)
        {
            _parentScope = scope;
            _sourceCollection = sourceCollection;
        }

        public bool TryGetInstance(IDependencyDescriptor descriptor, out IDependencyInstance instance)
        {
            return _scopedInstances.TryGetValue(descriptor.Type, out instance);
        }

        public IDependencyScope CreateScope()
        {
            return _parentScope.CreateScope();
        }

        public IEnumerable<TDependency> GetDependencies<TDependency>() where TDependency : class
        {
            List<TDependency> instances = new List<TDependency>();

            foreach (IDependencyDescriptor descriptor in _sourceCollection.GetDependencyDescriptors<TDependency>())
            {
                TDependency instance = GetDependency(descriptor).Instance<TDependency>();

                instances.Add(instance);
            }

            if (instances.Count == 0)
            {
                throw new DependencyNotFoundException(typeof(TDependency));
            }

            return instances;
        }

        public TDependency GetDependency<TDependency>() where TDependency : class
        {
            IDependencyDescriptor descriptor = _sourceCollection.GetDependencyDescriptor<TDependency>();

            if (descriptor == null)
            {
                throw new DependencyNotFoundException(typeof(TDependency));
            }

            return GetDependency(descriptor).Instance<TDependency>();
        }

        public IDependencyInstance GetDependency(IDependencyDescriptor descriptor)
        {
            if (descriptor.Lifetime == Lifetime.Singleton && _parentScope.TryGetInstance(descriptor, out IDependencyInstance instance))
            {
                return instance;
            }

            if (descriptor.Lifetime == Lifetime.Scoped && TryGetInstance(descriptor, out instance))
            {
                return instance;
            }

            instance = CreateInstance(descriptor);

            RegisterInstance(instance);

            return instance;
        }

        protected void RegisterInstance(IDependencyInstance instance)
        {
            if (instance.Lifetime == Lifetime.Singleton)
            {
                _parentScope.RegisterInstance(instance);

                return;
            }

            if (instance.Lifetime == Lifetime.Scoped)
            {
                _scopedInstances.Add(instance.Type, instance);
            }

            if (instance.IsDisposable && instance.Binding == Binding.Bound)
            {
                _disposableInstances.Add((IDisposable)instance.Instance);
            }
        }

        protected virtual IDependencyInstance CreateInstance(IDependencyDescriptor descriptor)
        {
            object instance;

            if (descriptor.ImplementationFactory != null)
            {
                instance = descriptor.ImplementationFactory.Invoke(this);
            }
            else if (descriptor.ImplementationInstance != null)
            {
                instance = descriptor.ImplementationInstance.Invoke();
            }
            else if (descriptor.ImplementationType != null)
            {
                ConstructorInfo[] constructs = descriptor.ImplementationType.GetConstructors();

                if (constructs.Length > 1)
                {
                    throw new ArgumentException($"Multiple constructs found for {descriptor.ImplementationType.Name}, only one may exist.");
                }

                ConstructorInfo constructor = constructs.First();

                ParameterInfo[] parameters = constructor.GetParameters();

                object[] arguments = new object[parameters.Length];

                for (int i = 0; i < parameters.Length; i++)
                {
                    IDependencyDescriptor parameterDescriptor = _sourceCollection.GetDependencyDescriptor(parameters[i].ParameterType);

                    if (parameterDescriptor == null)
                    {
                        if (parameters[i].HasDefaultValue)
                        {
                            continue;
                        }

                        throw new DependencyNotFoundException(parameters[i].ParameterType);
                    }

                    if (descriptor.Lifetime == Lifetime.Singleton && parameterDescriptor.Lifetime != Lifetime.Singleton)
                    {
                        throw new SingletonDependencyException(descriptor.Type);
                    }

                    arguments[i] = GetDependency(parameterDescriptor).Instance;
                }

                instance = Activator.CreateInstance(descriptor.ImplementationType, arguments);
            }
            else
            {
                throw new Exception();
            }

            if (instance == null)
            {
                throw new Exception();
            }

            return descriptor.ToInstance(instance);
        }

        public bool HasDependency<TDependency>() where TDependency : class
        {
            return _sourceCollection.HasDependency<TDependency>();
        }

        public bool TryGetDependency<TDependency>(out TDependency instance) where TDependency : class
        {
            IDependencyDescriptor descriptor = _sourceCollection.GetDependencyDescriptor<TDependency>();

            if (descriptor == null)
            {
                instance = null;

                return false;
            }

            instance = GetDependency(descriptor).Instance<TDependency>();

            return true;
        }

        public bool TryGetDependencies<TDependency>(out IEnumerable<TDependency> instances) where TDependency : class
        {
            List<TDependency> dependencyInstances = new List<TDependency>();

            foreach (IDependencyDescriptor descriptor in _sourceCollection.GetDependencyDescriptors<TDependency>())
            {
                TDependency dependencyInstance = GetDependency(descriptor).Instance<TDependency>();

                dependencyInstances.Add(dependencyInstance);
            }

            if (dependencyInstances.Count == 0)
            {
                instances = null;

                return false;
            }

            instances = dependencyInstances;

            return true;
        }

        public bool TryGetDependency(Type dependencyType, out object instance)
        {
            IDependencyDescriptor descriptor = _sourceCollection.GetDependencyDescriptor(dependencyType);

            if (descriptor == null)
            {
                instance = null;

                return false;
            }

            instance = GetDependency(descriptor).Instance;

            return true;
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
        /// Disposes all bound instances of scoped dependencies.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                foreach (IDisposable disposable in _disposableInstances)
                {
                    disposable.Dispose();
                }
            }

            _disposed = true;
        }

        #endregion IDisposable
    }
}