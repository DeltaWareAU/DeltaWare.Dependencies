#nullable enable
using DeltaWare.Dependencies.Abstractions.Attributes;
using System;

namespace DeltaWare.Dependencies.Abstractions.Exceptions
{
    public class MultipleDependencyConstructorsException : Exception
    {
        private MultipleDependencyConstructorsException(Type type, Exception? innerException = null) : base($"Multiple constructors found for {type.Name}, only one may exist or use the {nameof(InjectAttribute)} on the constructor to be used.", innerException)
        {
        }

        public static MultipleDependencyConstructorsException MultipleInjectAttribute(Type type)
        {
            return new MultipleDependencyConstructorsException(type, IllegalInjectionAttributeUsageException.MultipleConstructorsWithInjectAttribute(type));
        }

        public static MultipleDependencyConstructorsException MultipleConstructors(Type type)
        {
            return new MultipleDependencyConstructorsException(type);
        }
    }
}
