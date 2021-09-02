﻿using DeltaWare.Dependencies.Abstractions;
using DeltaWare.Dependencies.Abstractions.Exceptions;
using DeltaWare.Dependencies.Interfaces;
using System;
using System.Linq;
using System.Reflection;

namespace DeltaWare.Dependencies.Types
{
    public class DependencyDescriptor<TDependency> : IDependencyDescriptor, ICloneable
    {
        /// <inheritdoc cref="IDependencyDescriptor.Binding"/>
        public Binding Binding { get; }

        /// <inheritdoc cref="IDependencyDescriptor.Lifetime"/>
        public Lifetime Lifetime { get; }

        /// <inheritdoc cref="IDependencyDescriptor.Type"/>
        public Type Type { get; } = typeof(TDependency);

        /// <summary>
        /// Creates a new instance of <see cref="ImplementationDescriptor{TDependency,TImplementation}"/>.
        /// </summary>
        /// <param name="lifetime">Specifies the lifetime of the dependency.</param>
        /// <param name="binding">Specifies the binding of a dependency.</param>
        /// <exception cref="ArgumentNullException">Thrown when a null value is provided.</exception>
        public DependencyDescriptor(Lifetime lifetime, Binding binding = Binding.Bound)
        {
            Binding = binding;
            Lifetime = lifetime;
        }

        public object Clone()
        {
            return new DependencyDescriptor<TDependency>(Lifetime, Binding);
        }

        public IDependencyInstance CreateInstance(IDependencyProvider provider)
        {
            Type implementationType = typeof(TDependency);

            ConstructorInfo[] constructs = implementationType.GetConstructors();

            if (constructs.Length > 1)
            {
                throw new ArgumentException($"Multiple constructs found for {implementationType.Name}, only one may exist.");
            }

            ConstructorInfo constructor = constructs.First();

            ParameterInfo[] parameters = constructor.GetParameters();

            object[] arguments = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                if (provider.TryGetDependency(parameters[i].ParameterType, out object instance))
                {
                    arguments[i] = instance;
                }
                else if (!parameters[i].HasDefaultValue)
                {
                    throw new DependencyNotFoundException(parameters[i].ParameterType);
                }
            }

            object dependencyInstance = (TDependency)Activator.CreateInstance(typeof(TDependency), arguments);

            return new DependencyInstance(dependencyInstance, Type, Lifetime, Binding);
        }
    }
}