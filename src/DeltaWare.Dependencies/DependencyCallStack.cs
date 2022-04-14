using DeltaWare.Dependencies.Abstractions.Descriptors;
using DeltaWare.Dependencies.Abstractions.Exceptions;
using DeltaWare.SDK.Core.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeltaWare.Dependencies
{
    internal class DependencyCallStack : ITreeNode<IDependencyDescriptor>
    {
        private readonly List<ITreeNode<IDependencyDescriptor>> _childNodes = new();

        /// <summary>
        /// Creates a new instance of <see cref="DependencyCallStack"/>
        /// </summary>
        /// <param name="value">Sets the value.</param>
        /// <remarks>The Parent Node will be set to <see langword="null"/>.</remarks>
        public DependencyCallStack(IDependencyDescriptor value)
        {
            Value = value;
        }

        private DependencyCallStack(ITreeNode<IDependencyDescriptor> parentNode, IDependencyDescriptor value) : this(value)
        {
            ParentNode = parentNode;
        }

        /// <inheritdoc/>
        public ITreeNode<IDependencyDescriptor> ParentNode { get; }

        /// <inheritdoc/>
        public IReadOnlyList<ITreeNode<IDependencyDescriptor>> ChildNodes => _childNodes;

        /// <inheritdoc/>
        public IDependencyDescriptor Value { get; }

        /// <inheritdoc/>
        public ITreeNode<IDependencyDescriptor> AddChild(IDependencyDescriptor value)
        {
            ITreeNode<IDependencyDescriptor> childNode = new DependencyCallStack(this, value);

            _childNodes.Add(childNode);

            return childNode;
        }

        /// <inheritdoc/>
        public bool RemoveChild(ITreeNode<IDependencyDescriptor> node)
        {
            return _childNodes.Remove(node);
        }

        /// <inheritdoc/>
        public void Traverse(Action<IDependencyDescriptor> action)
        {
            action(Value);

            foreach (ITreeNode<IDependencyDescriptor> childNode in _childNodes)
            {
                childNode.Traverse(action);
            }
        }

        /// <inheritdoc/>
        public IEnumerable<IDependencyDescriptor> Flatten()
        {
            return new[] { Value }.Concat(_childNodes.SelectMany(x => x.Flatten()));
        }

        public void EnsureNoCircularDependencies()
        {
            DependencyCallStack dependencyCallStack = this;

            while (dependencyCallStack.ParentNode != null)
            {
                dependencyCallStack = (DependencyCallStack)dependencyCallStack.ParentNode;

                if (dependencyCallStack.Value == Value)
                {
                    throw new CircularDependencyException(this);
                }
            }
        }
    }
}