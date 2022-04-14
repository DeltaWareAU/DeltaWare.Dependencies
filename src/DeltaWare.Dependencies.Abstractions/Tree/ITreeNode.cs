using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace DeltaWare.SDK.Core.Collections
{
    /// <summary>
    /// Stores data represented as a tree.
    /// </summary>
    /// <typeparam name="T">The value of the node.</typeparam>
    public interface ITreeNode<T>
    {
        /// <summary>
        /// The current nodes parent.
        /// </summary>
        /// <remarks><see langword="null"/> if the current node has no parent.</remarks>
        ITreeNode<T> ParentNode { get; }

        /// <summary>
        /// The current nodes children.
        /// </summary>
        IReadOnlyList<ITreeNode<T>> ChildNodes { get; }

        /// <summary>
        /// The value of the current node.
        /// </summary>
        T Value { get; }

        /// <summary>
        /// Adds a child node.
        /// </summary>
        /// <param name="value">The value of the child node.</param>
        /// <returns>Returns an instance of the child node.</returns>
        ITreeNode<T> AddChild(T value);

        /// <summary>
        /// Removes the specified child node.
        /// </summary>
        /// <param name="node">The node to be removed.</param>
        /// <returns><see langword="true"/> if child node was removed or <see langword="false"/> if it was not.</returns>
        bool RemoveChild(ITreeNode<T> node);

        /// <summary>
        /// Traverses the tree invoking the specified action at each node.
        /// </summary>
        /// <param name="action">The action to be invoked for each node.</param>
        void Traverse(Action<T> action);

        /// <summary>
        /// Flattens the tree.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> contained the values from the tree.</returns>
        IEnumerable<T> Flatten();
    }
}