using DeltaWare.Dependencies.Abstractions;
using DeltaWare.Dependencies.Abstractions.Descriptors;
using DeltaWare.Dependencies.Abstractions.Enums;
using DeltaWare.Dependencies.Abstractions.Exceptions;
using DeltaWare.Dependencies.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

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

        private void ArgumentsBuilder(IDependencyDescriptor descriptor, ParameterInfo[] parameters, object[] arguments, DependencyCallStack callStack)
        {
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

                    paramInstance = InternalGetInstance(parameterDescriptor, callStack).Instance;
                }

                arguments[i] = paramInstance;
            }
        }

        private IDependencyInstance InternalCreateInstance(IDependencyDescriptor descriptor, DependencyCallStack callStack)
        {
            IDependencyInstance instance;

            if (descriptor is IInstanceDescriptor instanceDescriptor)
            {
                instance = instanceDescriptor.CreateInstance(this);
            }
            else if (descriptor is IParameterDescriptor parameterDescriptor)
            {
                instance = parameterDescriptor.CreateInstance(this, (parameters, arguments) => ArgumentsBuilder(parameterDescriptor, parameters, arguments, callStack));
            }
            else
            {
                throw new UnresolvableDependency(descriptor.Type);
            }

            if (instance == null)
            {
                throw new NullDependencyInstanceException(descriptor.Type);
            }

            return instance;
        }

        private IDependencyInstance InternalGetInstance(IDependencyDescriptor descriptor, DependencyCallStack callStack)
        {
            if (callStack == null)
            {
                callStack = new DependencyCallStack(descriptor);
            }
            else
            {
                callStack = (DependencyCallStack)callStack.AddChild(descriptor);

                callStack.EnsureNoCircularDependencies();
            }

            try
            {
                IDependencyInstance instance;

                if (descriptor.Lifetime == Lifetime.Singleton)
                {
                    Monitor.Enter(_parentScope);

                    if (_parentScope.TryGetInstance(descriptor, out instance))
                    {
                        return instance;
                    }
                }

                if (descriptor.Lifetime == Lifetime.Scoped && TryGetInstance(descriptor, out instance))
                {
                    return instance;
                }

                return CreateInstance();
            }
            finally
            {
                if (descriptor.Lifetime == Lifetime.Singleton)
                {
                    Monitor.Exit(_parentScope);
                }
            }

            IDependencyInstance CreateInstance()
            {
                IDependencyInstance instance = InternalCreateInstance(descriptor, callStack);

                if (instance.Lifetime == Lifetime.Singleton)
                {
                    _parentScope.RegisterInstance(instance);
                }
                else
                {
                    if (instance.Lifetime == Lifetime.Scoped)
                    {
                        _scopedInstances.Add(instance.Type, instance);
                    }

                    if (instance.IsDisposable && instance.Binding == Binding.Bound)
                    {
                        _disposableInstances.Add(instance);
                    }
                }

                return instance;
            }
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

        /// <inheritdoc/>
        public object CreateInstance(Type type)
        {
            IDependencyDescriptor descriptor = new TypeDependencyDescriptor(type, type, Lifetime.Scoped);

            return InternalGetInstance(descriptor).Instance;
        }

        /// <inheritdoc/>
        public bool TryCreateInstance(Type type, out object instance)
        {
            instance = GetDependency(type);

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