using DeltaWare.Dependencies.Abstractions.Attributes;
using System;

namespace DeltaWare.Dependencies.Abstractions.Exceptions
{
    public class IllegalInjectionAttributeUsageException : Exception
    {
        private IllegalInjectionAttributeUsageException(string message) : base(message)
        {

        }

        public static IllegalInjectionAttributeUsageException MultipleConstructorsWithInjectAttribute(Type type)
        {
            return new IllegalInjectionAttributeUsageException($"Multiple constructors found with the {nameof(InjectAttribute)} for {type.Name}, only one constructor may use this attribute.");
        }

        public static IllegalInjectionAttributeUsageException NullInjectablePropertyException()
        {
            return new IllegalInjectionAttributeUsageException($"A property with the {nameof(InjectAttribute)} must either have a non null default value or a registered type.");
        }
    }
}
