using DeltaWare.Dependencies.Abstractions;
using DeltaWare.Dependencies.Abstractions.Descriptors;
using DeltaWare.Dependencies.Abstractions.Enums;
using DeltaWare.Dependencies.Abstractions.Exceptions;
using System;
using System.Linq;
using System.Reflection;

namespace DeltaWare.Dependencies.Descriptors
{
    internal class TypeDependencyDescriptor : DependencyDescriptorBase, IParameterDescriptor
    {
        private readonly Type _implementationType;

        public TypeDependencyDescriptor(Type type, Type implementationType, Lifetime lifetime,
            Binding binding = Binding.Bound) : base(type, lifetime, binding)
        {
            _implementationType = implementationType ?? throw new ArgumentNullException(nameof(implementationType));
        }

        public IDependencyInstance CreateInstance(IDependencyProvider provider, Action<ParameterInfo[], object[]> argumentsBuilder)
        {
            ConstructorInfo[] constructs = _implementationType.GetConstructors();

            if (constructs.Length > 1)
            {
                throw new MultipleDependencyConstructorsException(_implementationType);
            }

            ConstructorInfo constructor = constructs.First();

            ParameterInfo[] parameters = constructor.GetParameters();

            object[] arguments = new object[parameters.Length];

            argumentsBuilder.Invoke(parameters, arguments);

            object instance = Activator.CreateInstance(_implementationType, arguments);

            return ToDependencyInstance(provider, instance);
        }
    }
}