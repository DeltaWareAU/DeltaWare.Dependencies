using System.Collections.Generic;

namespace DeltaWare.Dependencies.Abstractions.Stack
{
    public interface IStack<T>
    {
        List<IStack<T>> Children { get; }
        IStack<T> ParentStack { get; }
        T Value { get; }

        IStack<T> CreateChild(T value);
    }
}