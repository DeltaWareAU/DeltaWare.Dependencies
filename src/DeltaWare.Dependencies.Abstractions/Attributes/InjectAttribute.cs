using System;

namespace DeltaWare.Dependencies.Abstractions.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Constructor)]
    public class InjectAttribute : Attribute
    {
    }
}
