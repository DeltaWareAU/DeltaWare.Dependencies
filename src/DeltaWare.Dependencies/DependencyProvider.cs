using DeltaWare.Dependencies.Abstractions;
using DeltaWare.Dependencies.Abstractions.Enums;
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
        private readonly List<IDependencyInstance> _disposableInstances = new();
        private readonly bool _internalScope;
        private readonly DependencyScope _parentScope;
        private readonly Dictionary<Type, IDependencyInstance> _scopedInstances = new();
        private readonly IReadOnlyDependencyCollection _sourceCollection;

        public DependencyProvider(IReadOnlyDependencyCollection sourceCollection, DependencyScope scope)
        {
            _sourceCollection = sourceCollection;

            _parentScope = scope;
            _internalScope = false;
        }

        public DependencyProvider(IReadOnlyDependencyCollection sourceCollection)
        {
            _sourceCollection = sourceCollection;

            _parentScope = new DependencyScope(sourceCollection);
            _internalScope = true;
        }

        #region Instantiation

        public IDependencyInstance GetInstance(IDependencyDescriptor descriptor, List<IDependencyDescriptor> dependencyStack = null)
        {
            if (dependencyStack != null)
            {
                if (dependencyStack.Contains(descriptor))
                {
                    throw new CircularDependencyException(dependencyStack, descriptor);
                }

                dependencyStack.Add(descriptor);
            }

            if (descriptor.Lifetime == Lifetime.Singleton && _parentScope.TryGetInstance(descriptor, out IDependencyInstance instance))
            {
                return instance;
            }

            if (descriptor.Lifetime == Lifetime.Scoped && TryGetInstance(descriptor, out instance))
            {
                return instance;
            }

            instance = CreateInstance(descriptor, dependencyStack);

            RegisterInstance(instance);

            return instance;
        }

        protected virtual IDependencyInstance CreateInstance(IDependencyDescriptor descriptor, List<IDependencyDescriptor> dependencyStack = null)
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
                    object paramInstance;

                    if (parameters[i].ParameterType == typeof(IDependencyScope))
                    {
                        paramInstance = _parentScope;
                    }
                    else if (parameters[i].ParameterType == typeof(IDependencyProvider))
                    {
                        if (descriptor.Lifetime == Lifetime.Singleton)
                        {
                            throw new SingletonDependencyException(descriptor.Type);
                        }

                        paramInstance = this;
                    }
                    else
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

                        dependencyStack ??= new List<IDependencyDescriptor> { descriptor };

                        paramInstance = GetInstance(parameterDescriptor, dependencyStack).Instance;
                    }

                    arguments[i] = paramInstance;
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

        #endregion Instantiation

        public IDependencyScope CreateScope()
        {
            return _parentScope.CreateScope();
        }

        public IEnumerable<TDependency> GetDependencies<TDependency>() where TDependency : class
        {
            List<TDependency> instances = new List<TDependency>();

            foreach (IDependencyDescriptor descriptor in _sourceCollection.GetDependencyDescriptors<TDependency>())
            {
                TDependency instance = GetInstance(descriptor).Instance<TDependency>();

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

            return GetInstance(descriptor).Instance<TDependency>();
        }

        public bool HasDependency<TDependency>() where TDependency : class
        {
            return _sourceCollection.HasDependency<TDependency>();
        }

        public bool TryGetDependencies<TDependency>(out IEnumerable<TDependency> instances) where TDependency : class
        {
            List<TDependency> dependencyInstances = new List<TDependency>();

            foreach (IDependencyDescriptor descriptor in _sourceCollection.GetDependencyDescriptors<TDependency>())
            {
                TDependency dependencyInstance = GetInstance(descriptor).Instance<TDependency>();

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

        public bool TryGetDependency<TDependency>(out TDependency instance) where TDependency : class
        {
            IDependencyDescriptor descriptor = _sourceCollection.GetDependencyDescriptor<TDependency>();

            if (descriptor == null)
            {
                instance = null;

                return false;
            }

            instance = GetInstance(descriptor).Instance<TDependency>();

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

            instance = GetInstance(descriptor).Instance;

            return true;
        }

        public bool TryGetInstance(IDependencyDescriptor descriptor, out IDependencyInstance instance)
        {
            return _scopedInstances.TryGetValue(descriptor.Type, out instance);
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
                _disposableInstances.Add(instance);
            }
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
                if (_internalScope)
                {
                    _parentScope.Dispose();
                }

                foreach (IDependencyInstance instance in _disposableInstances)
                {
                    instance.Dispose();
                }
            }

            _disposed = true;
        }

        #endregion IDisposable
    }
}