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
        public TypeDependencyDescriptor(Type implementationType) : base(implementationType)
        {
            if (implementationType.IsAbstract || implementationType.IsInterface)
            {
                throw new ArgumentException();
            }
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

                    throw NullDependencyInstanceException.NullInstance(parameters[i].ParameterType);
                }

                arguments[i] = argument;
            }

            return Activator.CreateInstance(ImplementationType, arguments);
        }

        private ParameterInfo[] GetConstructorParameters()
        {
            ConstructorInfo[] constructors = ImplementationType.GetConstructors();

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
                        throw MultipleDependencyConstructorsException.MultipleInjectAttribute(ImplementationType);
                    }

                    injectionConstructor = constructor;
                }

                if (injectionConstructor == null)
                {
                    throw MultipleDependencyConstructorsException.MultipleConstructors(ImplementationType);
                }

                return injectionConstructor.GetParameters();
            }

            return constructors.First().GetParameters();
        }
    }
}
