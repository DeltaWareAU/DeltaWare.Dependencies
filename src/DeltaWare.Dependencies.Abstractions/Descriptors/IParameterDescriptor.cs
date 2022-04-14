using System;
using System.Reflection;

namespace DeltaWare.Dependencies.Abstractions.Descriptors
{
    public interface IParameterDescriptor : IDependencyDescriptor
    {
        IDependencyInstance CreateInstance(IDependencyProvider provider, Action<ParameterInfo[], object[]> argumentsBuilder);
    }
}
