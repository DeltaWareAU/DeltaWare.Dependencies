using DeltaWare.Dependencies.Abstractions;
using DeltaWare.Dependencies.Abstractions.Attributes;
using DeltaWare.Dependencies.Abstractions.Exceptions;
using System;
using System.Linq;
using System.Reflection;

namespace DeltaWare.Dependencies.Descriptors
{
    internal sealed class TypeDependencyDescriptor : DependencyDescriptorBase
    {
        private readonly Type _implementationType;

        public TypeDependencyDescriptor(Type implementationType)
        {
            _implementationType = implementationType ?? throw new ArgumentNullException(nameof(implementationType));
        }

        protected override object InternalCreateInstance(IDependencyProvider provider)
        {
            ParameterInfo[] parameters = GetConstructorParameters();

            object[] arguments = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                object argument = provider.GetDependency(parameters[i].ParameterType);

                if (argument == null)
                {
                    if (parameters[i].HasDefaultValue)
                    {
                        continue;
                    }

                    NullDependencyInstanceException.NullInstance(parameters[i].ParameterType);
                }

                arguments[i] = argument;
            }

            return Activator.CreateInstance(_implementationType, arguments);
        }

        private ParameterInfo[] GetConstructorParameters()
        {
            ConstructorInfo[] constructors = _implementationType.GetConstructors();

            if (constructors.Length > 1)
            {
                ConstructorInfo injectionConstructor = null;

                foreach (ConstructorInfo constructor in constructors)
                {
                    if (!constructor.HasAttribute<InjectAttribute>())
                    {
                        continue;
                    }

                    if (injectionConstructor != null)
                    {
                        throw MultipleDependencyConstructorsException.MultipleInjectAttribute(_implementationType);
                    }

                    injectionConstructor = constructor;
                }

                if (injectionConstructor == null)
                {
                    throw MultipleDependencyConstructorsException.MultipleConstructors(_implementationType);
                }

                return injectionConstructor.GetParameters();
            }

            return constructors.First().GetParameters();
        }
    }
}
