#nullable enable
using System;
using System.Reflection;

namespace DeltaWare.Dependencies.Abstractions.Exceptions
{
    public class NullDependencyInstanceException : Exception
    {
        private NullDependencyInstanceException(string message, Exception? innerException = null) : base(message, innerException)
        {
        }

        public static NullDependencyInstanceException NullInjectablePropertyException(PropertyInfo property)
        {
            return new NullDependencyInstanceException($"The injectable property {property.Name} has a null dependency", IllegalInjectionAttributeUsageException.NullInjectablePropertyException());
        }

        public static Exception NullInstance(Type type)
        {
            return new NullDependencyInstanceException($"No instance could be created for dependency {type.Name}");
        }
    }
}
