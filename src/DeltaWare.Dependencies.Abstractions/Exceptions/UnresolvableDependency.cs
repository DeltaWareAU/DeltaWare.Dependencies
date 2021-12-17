using System;

namespace DeltaWare.Dependencies.Abstractions.Exceptions
{
    public class UnresolvableDependency : Exception
    {
        public UnresolvableDependency(Type type) : base($"The dependency {type.Name} could not be resolved")
        {
        }
    }
}