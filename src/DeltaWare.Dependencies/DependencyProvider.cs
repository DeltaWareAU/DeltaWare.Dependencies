using DeltaWare.Dependencies.Abstractions;
using DeltaWare.Dependencies.Abstractions.Configuration;
using DeltaWare.Dependencies.Abstractions.Enums;
using DeltaWare.Dependencies.Abstractions.Exceptions;
using DeltaWare.Dependencies.Abstractions.Stack;
using DeltaWare.Dependencies.Extensions;
using DeltaWare.Dependencies.Stack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DeltaWare.Dependencies
{
    internal class DependencyProvider : IDependencyProvider
    {
        private readonly object _concurrencyLock = new();
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

        public IDependencyInstance InternalGetInstance(IDependencyDescriptor descriptor)
        {
            lock (_concurrencyLock)
            {
                return InternalGetInstance(descriptor, null);
            }
        }

        public bool TryGetInstance(IDependencyDescriptor descriptor, out IDependencyInstance instance)
        {
            return _scopedInstances.TryGetValue(descriptor.Type, out instance);
        }

        protected virtual void ConfigureInstance(IDependencyDescriptor descriptor, object instance)
        {
            foreach (IConfiguration configuration in descriptor.Configuration)
            {
                switch (configuration)
                {
                    case ITypeConfiguration typeConfiguration:
                        typeConfiguration.Configurator.Invoke(instance);
                        break;

                    case IProviderConfiguration providerConfiguration:
                        providerConfiguration.Configurator.Invoke(this, instance);
                        break;
                }
            }
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

        private IDependencyInstance InternalCreateInstance(IDependencyDescriptor descriptor, IStack<IDependencyDescriptor> parentStack)
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
                    throw new MultipleDependencyConstructorsException(descriptor.ImplementationType);
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

                        paramInstance = InternalGetInstance(parameterDescriptor, parentStack).Instance;
                    }

                    arguments[i] = paramInstance;
                }

                instance = Activator.CreateInstance(descriptor.ImplementationType, arguments);
            }
            else
            {
                throw new UnresolvableDependency(descriptor.Type);
            }

            if (instance == null)
            {
                throw new NullDependencyInstanceException(descriptor.Type);
            }

            ConfigureInstance(descriptor, instance);

            return descriptor.ToInstance(instance);
        }

        private IDependencyInstance InternalGetInstance(IDependencyDescriptor descriptor, IStack<IDependencyDescriptor> parentStack)
        {
            if (parentStack == null)
            {
                parentStack = new DependencyStack(descriptor);
            }
            else
            {
                parentStack = parentStack.CreateChild(descriptor);

                parentStack.EnsureNoCircularDependencies();
            }

            if (descriptor.Lifetime == Lifetime.Singleton && _parentScope.TryGetInstance(descriptor, out IDependencyInstance instance))
            {
                return instance;
            }

            if (descriptor.Lifetime == Lifetime.Scoped && TryGetInstance(descriptor, out instance))
            {
                return instance;
            }

            instance = InternalCreateInstance(descriptor, parentStack);

            RegisterInstance(instance);

            return instance;
        }

        #endregion Instantiation

        public IDependencyScope CreateScope()
        {
            return _parentScope.CreateScope();
        }

        public IEnumerable<object> GetDependencies(Type dependencyType)
        {
            List<object> instances = new List<object>();

            foreach (IDependencyDescriptor descriptor in _sourceCollection.GetDependencyDescriptors(dependencyType))
            {
                object instance = InternalGetInstance(descriptor).Instance;

                instances.Add(instance);
            }

            if (instances.Count == 0)
            {
                return new List<object>();
            }

            return instances;
        }

        public object GetDependency(Type dependencyType)
        {
            IDependencyDescriptor descriptor = _sourceCollection.GetDependencyDescriptor(dependencyType);

            if (descriptor == null)
            {
                return null;
            }

            return InternalGetInstance(descriptor).Instance;
        }

        public bool HasDependency(Type dependencyType)
        {
            return _sourceCollection.HasDependency(dependencyType);
        }

        public bool TryGetDependencies(Type dependencyType, out IEnumerable<object> instances)
        {
            List<object> dependencyInstances = GetDependencies(dependencyType).ToList();

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
            instance = GetDependency(dependencyType);

            return instance != null;
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