using System;

namespace DeltaWare.Dependencies.Abstractions.Exceptions
{
    public class MultipleDependencyConstructorsException : Exception
    {
        public MultipleDependencyConstructorsException(Type type) : base($"Multiple constructs found for {type.Name}, only one may exist.")
        {
        }
    }
}