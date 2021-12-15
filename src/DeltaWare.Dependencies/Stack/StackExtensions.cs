using DeltaWare.Dependencies.Abstractions;
using DeltaWare.Dependencies.Abstractions.Exceptions;
using DeltaWare.Dependencies.Abstractions.Stack;

namespace DeltaWare.Dependencies.Stack
{
    internal static class StackExtensions
    {
        public static void EnsureNoCircularDependencies(this IStack<IDependencyDescriptor> initialStack)
        {
            IStack<IDependencyDescriptor> dependencyStack = initialStack;

            while (dependencyStack.ParentStack != null)
            {
                dependencyStack = dependencyStack.ParentStack;

                if (dependencyStack.Value == initialStack.Value)
                {
                    throw new CircularDependencyException(initialStack);
                }
            }
        }
    }
}