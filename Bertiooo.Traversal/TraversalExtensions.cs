using Bertiooo.Traversal.Factories;
using Bertiooo.Traversal.Selectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bertiooo.Traversal
{
	public static class TraversalExtensions
	{
		#region Node Analysis

        /// <summary>
        /// Determines whether the specified node is the root node.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to evaluate.</param>
        /// <returns><see langword="true"/> if the node has no parent; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no traversal strategy can be derived from <typeparamref name="TNode"/>.
        /// </exception>
		public static bool IsRoot<TNode>(this TNode node)
		{
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy<TNode>();
            return node.IsRoot(strategy);
        }

        /// <summary>
        /// Determines whether the specified node is the root node by using the provided parent selector.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to evaluate.</param>
        /// <param name="getParentFunc">A function that returns the parent of a node.</param>
        /// <returns><see langword="true"/> if the node has no parent; otherwise, <see langword="false"/>.</returns>
        public static bool IsRoot<TNode>(this TNode node, Func<TNode, TNode?> getParentFunc)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy(getParentFunc);
            return node.IsRoot(strategy);
        }

        /// <summary>
        /// Determines whether the specified node is the root node by using the provided traversal strategy.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to evaluate.</param>
        /// <param name="strategy">The traversal strategy used to resolve the parent of the node.</param>
        /// <returns><see langword="true"/> if the node has no parent; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="strategy"/> is <see langword="null"/>.</exception>
        public static bool IsRoot<TNode>(this TNode node, ITraversalStrategy<TNode> strategy)
        {
            return node.HasParent(strategy) == false;
        }

        /// <summary>
        /// Determines whether the specified node is an inner node.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to evaluate.</param>
        /// <returns><see langword="true"/> if the node is not the root node and not a leaf node; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no traversal strategy can be derived from <typeparamref name="TNode"/>.
        /// </exception>
        public static bool IsInnerNode<TNode>(this TNode node)
		{
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy<TNode>();
            return node.IsInnerNode(strategy);
		}

        /// <summary>
        /// Determines whether the specified node is an inner node by using the provided parent and child selectors.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to evaluate.</param>
        /// <param name="getParentFunc">A function that returns the parent of a node.</param>
        /// <param name="getChildrenFunc">A function that returns the children of a node.</param>
        /// <returns><see langword="true"/> if the node is not the root node and not a leaf node; otherwise, <see langword="false"/>.</returns>
        public static bool IsInnerNode<TNode>(this TNode node, Func<TNode, TNode?> getParentFunc, Func<TNode, IEnumerable<TNode>> getChildrenFunc)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy(getParentFunc, getChildrenFunc);
            return node.IsInnerNode(strategy);
        }

        /// <summary>
        /// Determines whether the specified node is an inner node by using the provided traversal strategy.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to evaluate.</param>
        /// <param name="strategy">The traversal strategy used to resolve the parent and children of the node.</param>
        /// <returns><see langword="true"/> if the node is not the root node and not a leaf node; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="strategy"/> is <see langword="null"/>.</exception>
        public static bool IsInnerNode<TNode>(this TNode node, ITraversalStrategy<TNode> strategy)
        {
            return node.IsRoot(strategy) == false && node.IsLeaf(strategy) == false;
        }

        /// <summary>
        /// Determines whether the specified node is a leaf node.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to evaluate.</param>
        /// <returns><see langword="true"/> if the node has no children; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no traversal strategy can be derived from <typeparamref name="TNode"/>.
        /// </exception>
        public static bool IsLeaf<TNode>(this TNode node)
		{
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy<TNode>();
			return node.IsLeaf(strategy);
		}

        /// <summary>
        /// Determines whether the specified node is a leaf node by using the provided child selector.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to evaluate.</param>
        /// <param name="getChildrenFunc">A function that returns the children of a node.</param>
        /// <returns><see langword="true"/> if the node has no children; otherwise, <see langword="false"/>.</returns>
        public static bool IsLeaf<TNode>(this TNode node, Func<TNode, IEnumerable<TNode>> getChildrenFunc)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy(getChildrenFunc);
            return node.IsLeaf(strategy);
        }

        /// <summary>
        /// Determines whether the specified node is a leaf node by using the provided traversal strategy.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to evaluate.</param>
        /// <param name="strategy">The traversal strategy used to resolve the children of the node.</param>
        /// <returns><see langword="true"/> if the node has no children; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="strategy"/> is <see langword="null"/>.</exception>
        public static bool IsLeaf<TNode>(this TNode node, ITraversalStrategy<TNode> strategy)
        {
            return node.HasChildren(strategy) == false;
        }

        /// <summary>
        /// Determines whether the specified node has a parent.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to evaluate.</param>
        /// <returns><see langword="true"/> if the parent of the node is not the default value of <typeparamref name="TNode"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no traversal strategy can be derived from <typeparamref name="TNode"/>.
        /// </exception>
        public static bool HasParent<TNode>(this TNode node)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy<TNode>();
			return node.HasParent(strategy);
        }

        /// <summary>
        /// Determines whether the specified node has a parent by using the provided parent selector.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to evaluate.</param>
        /// <param name="getParentFunc">A function that returns the parent of a node.</param>
        /// <returns><see langword="true"/> if the parent of the node is not the default value of <typeparamref name="TNode"/>; otherwise, <see langword="false"/>.</returns>
        public static bool HasParent<TNode>(this TNode node, Func<TNode, TNode?> getParentFunc)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy(getParentFunc);
            return node.HasParent(strategy);
        }

        /// <summary>
        /// Determines whether the specified node has a parent by using the provided traversal strategy.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to evaluate.</param>
        /// <param name="strategy">The traversal strategy used to resolve the parent of the node.</param>
        /// <returns><see langword="true"/> if the parent of the node is not the default value of <typeparamref name="TNode"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="strategy"/> is <see langword="null"/>.</exception>
        public static bool HasParent<TNode>(this TNode node, ITraversalStrategy<TNode> strategy)
        {
            if (strategy == null)
                throw new ArgumentNullException(nameof(strategy));

            return Equals(strategy.GetParent(node), default) == false;
        }

        /// <summary>
        /// Determines whether the specified node has any children.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to evaluate.</param>
        /// <returns><see langword="true"/> if the node has at least one child; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no traversal strategy can be derived from <typeparamref name="TNode"/>.
        /// </exception>
        public static bool HasChildren<TNode>(this TNode node)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy<TNode>();
			return node.HasChildren(strategy);
        }

        /// <summary>
        /// Determines whether the specified node has any children by using the provided child selector.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to evaluate.</param>
        /// <param name="getChildrenFunc">A function that returns the children of a node.</param>
        /// <returns><see langword="true"/> if the node has at least one child; otherwise, <see langword="false"/>.</returns>
        public static bool HasChildren<TNode>(this TNode node, Func<TNode, IEnumerable<TNode>> getChildrenFunc)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy(getChildrenFunc);
            return node.HasChildren(strategy);
        }

        /// <summary>
        /// Determines whether the specified node has any children by using the provided traversal strategy.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to evaluate.</param>
        /// <param name="strategy">The traversal strategy used to resolve the children of the node.</param>
        /// <returns><see langword="true"/> if the node has at least one child; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="strategy"/> is <see langword="null"/>.</exception>
        public static bool HasChildren<TNode>(this TNode node, ITraversalStrategy<TNode> strategy)
		{
            if (strategy == null)
                throw new ArgumentNullException(nameof(strategy));

            var children = strategy.GetChildren(node);
			return children != null && children.Any();
		}

        /// <summary>
        /// Determines whether the specified node has any siblings.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to evaluate.</param>
        /// <returns><see langword="true"/> if the node has at least one sibling; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no traversal strategy can be derived from <typeparamref name="TNode"/>.
        /// </exception>
        public static bool HasSiblings<TNode>(this TNode node)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy<TNode>();
            return node.HasSiblings(strategy);
        }

        /// <summary>
        /// Determines whether the specified node has any siblings by using the provided parent and child selectors.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to evaluate.</param>
        /// <param name="getParentFunc">A function that returns the parent of a node.</param>
        /// <param name="getChildrenFunc">A function that returns the children of a node.</param>
        /// <returns><see langword="true"/> if the node has at least one sibling; otherwise, <see langword="false"/>.</returns>
        public static bool HasSiblings<TNode>(this TNode node, Func<TNode, TNode?> getParentFunc, Func<TNode, IEnumerable<TNode>> getChildrenFunc)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy(getParentFunc, getChildrenFunc);
            return node.HasSiblings(strategy);
        }

        /// <summary>
        /// Determines whether the specified node has any siblings by using the provided traversal strategy.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to evaluate.</param>
        /// <param name="strategy">The traversal strategy used to resolve the parent and siblings of the node.</param>
        /// <returns><see langword="true"/> if the node has at least one sibling; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="strategy"/> is <see langword="null"/>.</exception>
        public static bool HasSiblings<TNode>(this TNode node, ITraversalStrategy<TNode> strategy)
		{
            if (strategy == null)
                throw new ArgumentNullException(nameof(strategy));

            var parent = strategy.GetParent(node);

			if (parent == null)
				return false;

			var parentsChildren = strategy.GetChildren(parent);

			if (parentsChildren == null)
				return false;

			return parentsChildren.Any(x => Equals(x, node) == false);
		}

		/// <summary>
		/// Returns the zero-based level of the specified node.
		/// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node whose level is to be determined.</param>
        /// <returns>The zero-based level of the node, where the root node is level 0.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no traversal strategy can be derived from <typeparamref name="TNode"/>.
        /// </exception>
		public static int GetLevel<TNode>(this TNode node)
		{
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy<TNode>();
            return node.GetLevel(strategy);
        }

        /// <summary>
		/// Returns the zero-based level of the specified node by using the provided parent selector.
		/// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node whose level is to be determined.</param>
        /// <param name="getParentFunc">A function that returns the parent of a node.</param>
        /// <returns>The zero-based level of the node, where the root node is level 0.</returns>
		public static int GetLevel<TNode>(this TNode node, Func<TNode, TNode?> getParentFunc)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy(getParentFunc);
            return node.GetLevel(strategy);
        }

        /// <summary>
        /// Returns the zero-based level of the specified node by using the provided traversal strategy.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node whose level is to be determined.</param>
        /// <param name="strategy">The traversal strategy used to resolve the ancestors of the node.</param>
        /// <returns>The zero-based level of the node, where the root node is level 0.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="strategy"/> is <see langword="null"/>.</exception>
        public static int GetLevel<TNode>(this TNode node, ITraversalStrategy<TNode> strategy)
        {
            return node.Ancestors(strategy).Count();
        }

        /// <summary>
        /// Returns the maximum depth of the specified node.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node whose maximum depth is to be determined.</param>
        /// <returns>The maximum depth below the node. A leaf node has a depth of 0.</returns>
        /// <remarks>
        /// This operation may be computationally expensive because it traverses all descendants of the node.
        /// </remarks>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no traversal strategy can be derived from <typeparamref name="TNode"/>.
        /// </exception>
        public static int GetMaxDepth<TNode>(this TNode node)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy<TNode>();
            return node.GetMaxDepth(strategy);
        }

        /// <summary>
        /// Returns the maximum depth of the specified node by using the provided parent and child selectors.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node whose maximum depth is to be determined.</param>
        /// <param name="getParentFunc">A function that returns the parent of a node.</param>
        /// <param name="getChildrenFunc">A function that returns the children of a node.</param>
        /// <returns>The maximum depth below the node. A leaf node has a depth of 0.</returns>
        /// <remarks>
        /// This operation may be computationally expensive because it traverses all descendants of the node.
        /// </remarks>
        public static int GetMaxDepth<TNode>(this TNode node, Func<TNode, TNode?> getParentFunc, Func<TNode, IEnumerable<TNode>> getChildrenFunc)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy(getParentFunc, getChildrenFunc);
            return node.GetMaxDepth(strategy);
        }

        /// <summary>
        /// Returns the maximum depth of the specified node by using the provided traversal strategy.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node whose maximum depth is to be determined.</param>
        /// <param name="strategy">The traversal strategy used to traverse the descendants of the node.</param>
        /// <returns>The maximum depth below the node. A leaf node has a depth of 0.</returns>
        /// <remarks>
        /// This operation may be computationally expensive because it traverses all descendants of the node.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="strategy"/> is <see langword="null"/>.</exception>
        public static int GetMaxDepth<TNode>(this TNode node, ITraversalStrategy<TNode> strategy)
		{
			var maxLevel = 0;

			node.Traverse(
				x => maxLevel = Math.Max(maxLevel, x.GetLevel(strategy)),
                strategy);

			return maxLevel - node.GetLevel(strategy);
		}

        /// <summary>
        /// Determines whether the specified node is a direct child of another node.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to evaluate.</param>
        /// <param name="other">The potential parent node.</param>
        /// <returns><see langword="true"/> if <paramref name="other"/> is the direct parent of <paramref name="node"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no traversal strategy can be derived from <typeparamref name="TNode"/>.
        /// </exception>
        public static bool IsChildOf<TNode>(this TNode node, TNode other)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy<TNode>();
            return node.IsChildOf(other, strategy);
        }

        /// <summary>
        /// Determines whether the specified node is a direct child of another node by using the provided parent selector.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to evaluate.</param>
        /// <param name="other">The potential parent node.</param>
        /// <param name="getParentFunc">A function that returns the parent of a node.</param>
        /// <returns><see langword="true"/> if <paramref name="other"/> is the direct parent of <paramref name="node"/>; otherwise, <see langword="false"/>.</returns>
        public static bool IsChildOf<TNode>(this TNode node, TNode other, Func<TNode, TNode?> getParentFunc)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy(getParentFunc);
            return node.IsChildOf(other, strategy);
        }

        /// <summary>
        /// Determines whether the specified node is a direct child of another node by using the provided traversal strategy.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to evaluate.</param>
        /// <param name="other">The potential parent node.</param>
        /// <param name="strategy">The traversal strategy used to resolve the parent of the node.</param>
        /// <returns><see langword="true"/> if <paramref name="other"/> is the direct parent of <paramref name="node"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="strategy"/> is <see langword="null"/>.</exception>
        public static bool IsChildOf<TNode>(this TNode node, TNode other, ITraversalStrategy<TNode> strategy)
		{
            if (strategy == null)
                throw new ArgumentNullException(nameof(strategy));

            var parent = strategy.GetParent(node);

			if (parent == null)
				return false;

			return parent.Equals(other);
		}

        /// <summary>
        /// Determines whether the specified node is a direct parent of another node.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The potential parent node.</param>
        /// <param name="other">The node to evaluate.</param>
        /// <returns><see langword="true"/> if <paramref name="node"/> is the direct parent of <paramref name="other"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no traversal strategy can be derived from <typeparamref name="TNode"/>.
        /// </exception>
        public static bool IsParentOf<TNode>(this TNode node, TNode other)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy<TNode>();
            return node.IsParentOf(other, strategy);
        }

        /// <summary>
        /// Determines whether the specified node is a direct parent of another node by using the provided parent selector.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The potential parent node.</param>
        /// <param name="other">The node to evaluate.</param>
        /// <param name="getParentFunc">A function that returns the parent of a node.</param>
        /// <returns><see langword="true"/> if <paramref name="node"/> is the direct parent of <paramref name="other"/>; otherwise, <see langword="false"/>.</returns>
        public static bool IsParentOf<TNode>(this TNode node, TNode other, Func<TNode, TNode?> getParentFunc)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy(getParentFunc);
            return node.IsParentOf(other, strategy);
        }

        /// <summary>
        /// Determines whether the specified node is a direct parent of another node by using the provided traversal strategy.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The potential parent node.</param>
        /// <param name="other">The node to evaluate.</param>
        /// <param name="strategy">The traversal strategy used to resolve the parent of <paramref name="other"/>.</param>
        /// <returns><see langword="true"/> if <paramref name="node"/> is the direct parent of <paramref name="other"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> or <paramref name="strategy"/> is <see langword="null"/>.</exception>
        public static bool IsParentOf<TNode>(this TNode node, TNode other, ITraversalStrategy<TNode> strategy)
		{
			if (other == null)
				throw new ArgumentNullException(nameof(other));

            if (strategy == null)
                throw new ArgumentNullException(nameof(strategy));

            var parent = strategy.GetParent(other);

			if (parent == null)
				return false;

            return parent.Equals(node);
		}

        /// <summary>
        /// Determines whether the specified node is a sibling of another node.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to evaluate.</param>
        /// <param name="other">The other node to compare with.</param>
        /// <returns><see langword="true"/> if both nodes share the same parent; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no traversal strategy can be derived from <typeparamref name="TNode"/>.
        /// </exception>
        public static bool IsSiblingOf<TNode>(this TNode node, TNode other)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy<TNode>();
            return node.IsSiblingOf(other, strategy);
        }

        /// <summary>
        /// Determines whether the specified node is a sibling of another node by using the provided parent selector.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to evaluate.</param>
        /// <param name="other">The other node to compare with.</param>
        /// <param name="getParentFunc">A function that returns the parent of a node.</param>
        /// <returns><see langword="true"/> if both nodes share the same parent; otherwise, <see langword="false"/>.</returns>
        public static bool IsSiblingOf<TNode>(this TNode node, TNode other, Func<TNode, TNode?> getParentFunc)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy(getParentFunc);
            return node.IsSiblingOf(other, strategy);
        }

        /// <summary>
        /// Determines whether the specified node is a sibling of another node by using the provided traversal strategy.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to evaluate.</param>
        /// <param name="other">The other node to compare with.</param>
        /// <param name="strategy">The traversal strategy used to resolve the parents of both nodes.</param>
        /// <returns><see langword="true"/> if both nodes share the same parent; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> or <paramref name="strategy"/> is <see langword="null"/>.</exception>
        public static bool IsSiblingOf<TNode>(this TNode node, TNode other, ITraversalStrategy<TNode> strategy)
		{
			if (other == null)
				throw new ArgumentNullException(nameof(other));

            if (strategy == null)
                throw new ArgumentNullException(nameof(strategy));

            var nodeParent = strategy.GetParent(node);

			if (nodeParent == null)
				return false;

			var otherParent = strategy.GetParent(other);

			if(otherParent == null) 
				return false;

			return nodeParent.Equals(otherParent);
		}

        /// <summary>
        /// Determines whether the specified node is a descendant of another node.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to evaluate.</param>
        /// <param name="other">The potential ancestor node.</param>
        /// <returns><see langword="true"/> if <paramref name="other"/> is an ancestor of <paramref name="node"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no traversal strategy can be derived from <typeparamref name="TNode"/>.
        /// </exception>
        public static bool IsDescendantOf<TNode>(this TNode node, TNode other)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy<TNode>();
            return node.IsDescendantOf(other, strategy);
        }

        /// <summary>
        /// Determines whether the specified node is a descendant of another node by using the provided parent selector.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to evaluate.</param>
        /// <param name="other">The potential ancestor node.</param>
        /// <param name="getParentFunc">A function that returns the parent of a node.</param>
        /// <returns><see langword="true"/> if <paramref name="other"/> is an ancestor of <paramref name="node"/>; otherwise, <see langword="false"/>.</returns>
        public static bool IsDescendantOf<TNode>(this TNode node, TNode other, Func<TNode, TNode?> getParentFunc)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy(getParentFunc);
            return node.IsDescendantOf(other, strategy);
        }

        /// <summary>
        /// Determines whether the specified node is a descendant of another node by using the provided traversal strategy.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to evaluate.</param>
        /// <param name="other">The potential ancestor node.</param>
        /// <param name="strategy">The traversal strategy used to resolve the ancestors of the node.</param>
        /// <returns><see langword="true"/> if <paramref name="other"/> is an ancestor of <paramref name="node"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="strategy"/> is <see langword="null"/>.</exception>
        public static bool IsDescendantOf<TNode>(this TNode node, TNode other, ITraversalStrategy<TNode> strategy)
        {
            return node.Ancestors(strategy).Any(x => Equals(other, x));
        }

        /// <summary>
        /// Determines whether the specified node is an ancestor of another node.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The potential ancestor node.</param>
        /// <param name="other">The node to evaluate.</param>
        /// <returns><see langword="true"/> if <paramref name="node"/> is an ancestor of <paramref name="other"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no traversal strategy can be derived from <typeparamref name="TNode"/>.
        /// </exception>
        public static bool IsAncestorOf<TNode>(this TNode node, TNode other)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy<TNode>();
            return node.IsAncestorOf(other, strategy);
        }

        /// <summary>
        /// Determines whether the specified node is an ancestor of another node by using the provided parent selector.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The potential ancestor node.</param>
        /// <param name="other">The node to evaluate.</param>
        /// <param name="getParentFunc">A function that returns the parent of a node.</param>
        /// <returns><see langword="true"/> if <paramref name="node"/> is an ancestor of <paramref name="other"/>; otherwise, <see langword="false"/>.</returns>
        public static bool IsAncestorOf<TNode>(this TNode node, TNode other, Func<TNode, TNode?> getParentFunc)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy(getParentFunc);
            return node.IsAncestorOf(other, strategy);
        }

        /// <summary>
        /// Determines whether the specified node is an ancestor of another node by using the provided traversal strategy.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The potential ancestor node.</param>
        /// <param name="other">The node to evaluate.</param>
        /// <param name="strategy">The traversal strategy used to resolve the ancestors of <paramref name="other"/>.</param>
        /// <returns><see langword="true"/> if <paramref name="node"/> is an ancestor of <paramref name="other"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
        public static bool IsAncestorOf<TNode>(this TNode node, TNode other, ITraversalStrategy<TNode> strategy)
		{
			if (other == null)
				throw new ArgumentNullException(nameof(other));

			return other.Ancestors(strategy).Any(x => Equals(node, x));
		}

        #endregion

        #region Related Nodes Retrieval

        /// <summary>
        /// Returns the root node of the hierarchy that contains the specified node.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node whose root is to be returned.</param>
        /// <returns>The root node of the hierarchy.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no traversal strategy can be derived from <typeparamref name="TNode"/>.
        /// </exception>
        public static TNode Root<TNode>(this TNode node)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy<TNode>();
            return node.Root(strategy);
        }

        /// <summary>
        /// Returns the root node of the hierarchy that contains the specified node by using the provided parent selector.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node whose root is to be returned.</param>
        /// <param name="getParentFunc">A function that returns the parent of a node.</param>
        /// <returns>The root node of the hierarchy.</returns>
        public static TNode Root<TNode>(this TNode node, Func<TNode, TNode?> getParentFunc)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy(getParentFunc);
            return node.Root(strategy);
        }

        /// <summary>
        /// Returns the root node of the hierarchy that contains the specified node by using the provided traversal strategy.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node whose root is to be returned.</param>
        /// <param name="strategy">The traversal strategy used to resolve the parents of the node.</param>
        /// <returns>The root node of the hierarchy.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="strategy"/> is <see langword="null"/>.</exception>
        public static TNode Root<TNode>(this TNode node, ITraversalStrategy<TNode> strategy)
		{
            if (strategy == null)
                throw new ArgumentNullException(nameof(strategy));

            var parent = strategy.GetParent(node);

            while (parent != null)
            {
                node = parent;
                parent = strategy.GetParent(node);
            }

            return node;
        }

        /// <summary>
        /// Returns the specified node together with its direct parent, if one exists.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to return together with its parent.</param>
        /// <returns>An enumerable containing the node and, if available, its direct parent.</returns>
        /// <remarks>
        /// If the node has no parent, the returned sequence contains only the node itself.
        /// </remarks>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no traversal strategy can be derived from <typeparamref name="TNode"/>.
        /// </exception>
        public static IEnumerable<TNode> WithParent<TNode>(this TNode node)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy<TNode>();
            return node.WithParent(strategy);
        }

        /// <summary>
        /// Returns the specified node together with its direct parent, if one exists, by using the provided parent selector.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to return together with its parent.</param>
        /// <param name="getParentFunc">A function that returns the parent of a node.</param>
        /// <returns>An enumerable containing the node and, if available, its direct parent.</returns>
        /// <remarks>
        /// If the node has no parent, the returned sequence contains only the node itself.
        /// </remarks>
        public static IEnumerable<TNode> WithParent<TNode>(this TNode node, Func<TNode, TNode?> getParentFunc)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy(getParentFunc);
            return node.WithParent(strategy);
        }

        /// <summary>
        /// Returns the specified node together with its direct parent, if one exists, by using the provided traversal strategy.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to return together with its parent.</param>
        /// <param name="strategy">The traversal strategy used to resolve the parent of the node.</param>
        /// <returns>An enumerable containing the node and, if available, its direct parent.</returns>
        /// <remarks>
        /// If the node has no parent, the returned sequence contains only the node itself.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="strategy"/> is <see langword="null"/>.</exception>
        public static IEnumerable<TNode> WithParent<TNode>(this TNode node, ITraversalStrategy<TNode> strategy)
		{
            if (strategy == null)
                throw new ArgumentNullException(nameof(strategy));

            return Iterator();

            IEnumerable<TNode> Iterator()
            {
                yield return node;

                var parent = strategy.GetParent(node);

                if (parent != null)
                    yield return parent;
            }
		}

        /// <summary>
        /// Returns the specified node together with its direct children.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to return together with its children.</param>
        /// <returns>An enumerable containing the node and all of its direct children.</returns>
        /// <remarks>
        /// If the node has no children, the returned sequence contains only the node itself.
        /// </remarks>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no traversal strategy can be derived from <typeparamref name="TNode"/>.
        /// </exception>
        public static IEnumerable<TNode> WithChildren<TNode>(this TNode node)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy<TNode>();
            return node.WithChildren(strategy);
        }

        /// <summary>
        /// Returns the specified node together with its direct children by using the provided child selector.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to return together with its children.</param>
        /// <param name="getChildrenFunc">A function that returns the children of a node.</param>
        /// <returns>An enumerable containing the node and all of its direct children.</returns>
        /// <remarks>
        /// If the node has no children, the returned sequence contains only the node itself.
        /// </remarks>
        public static IEnumerable<TNode> WithChildren<TNode>(this TNode node, Func<TNode, IEnumerable<TNode>> getChildrenFunc)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy(getChildrenFunc);
            return node.WithChildren(strategy);
        }

        /// <summary>
        /// Returns the specified node together with its direct children by using the provided traversal strategy.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to return together with its children.</param>
        /// <param name="strategy">The traversal strategy used to resolve the children of the node.</param>
        /// <returns>An enumerable containing the node and all of its direct children.</returns>
        /// <remarks>
        /// If the node has no children, the returned sequence contains only the node itself.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="strategy"/> is <see langword="null"/>.</exception>
        public static IEnumerable<TNode> WithChildren<TNode>(this TNode node, ITraversalStrategy<TNode> strategy)
		{
            if (strategy == null)
                throw new ArgumentNullException(nameof(strategy));

            return Iterator();

            IEnumerable<TNode> Iterator()
            {
                yield return node;

                var children = strategy.GetChildren(node);

                if (children == null)
                    yield break;

                foreach (var child in children)
                    yield return child;
            }
		}

        /// <summary>
        /// Returns the siblings of the specified node.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node whose siblings are to be returned.</param>
        /// <returns>An enumerable containing all siblings of the node, excluding the node itself.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no traversal strategy can be derived from <typeparamref name="TNode"/>.
        /// </exception>
        public static IEnumerable<TNode> Siblings<TNode>(this TNode node)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy<TNode>();
            return node.Siblings(strategy);
        }

        /// <summary>
        /// Returns the siblings of the specified node by using the provided parent and child selectors.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node whose siblings are to be returned.</param>
        /// <param name="getParentFunc">A function that returns the parent of a node.</param>
        /// <param name="getChildrenFunc">A function that returns the children of a node.</param>
        /// <returns>An enumerable containing all siblings of the node, excluding the node itself.</returns>
        public static IEnumerable<TNode> Siblings<TNode>(this TNode node, Func<TNode, TNode?> getParentFunc, Func<TNode, IEnumerable<TNode>> getChildrenFunc)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy(getParentFunc, getChildrenFunc);
            return node.Siblings(strategy);
        }

        /// <summary>
        /// Returns the siblings of the specified node by using the provided traversal strategy.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node whose siblings are to be returned.</param>
        /// <param name="strategy">The traversal strategy used to resolve the parent and siblings of the node.</param>
        /// <returns>An enumerable containing all siblings of the node, excluding the node itself.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="strategy"/> is <see langword="null"/>.</exception>
        public static IEnumerable<TNode> Siblings<TNode>(this TNode node, ITraversalStrategy<TNode> strategy)
        {
            if (strategy == null)
                throw new ArgumentNullException(nameof(strategy));

			var parent = strategy.GetParent(node);

			if (parent == null)
				return Enumerable.Empty<TNode>();

			var children = strategy.GetChildren(parent);

            if (children == null)
                return Enumerable.Empty<TNode>();

            return children.Where(x => Equals(x, node) == false);
        }

        /// <summary>
        /// Returns the specified node together with its siblings.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to return together with its siblings.</param>
        /// <returns>An enumerable containing the node followed by its siblings.</returns>
        /// <remarks>
        /// If the node has no siblings, the returned sequence contains only the node itself.
        /// The node itself is always returned as the first element.
        /// </remarks>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no traversal strategy can be derived from <typeparamref name="TNode"/>.
        /// </exception>
        public static IEnumerable<TNode> WithSiblings<TNode>(this TNode node)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy<TNode>();
            return node.WithSiblings(strategy);
        }

        /// <summary>
        /// Returns the specified node together with its siblings by using the provided parent and child selectors.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to return together with its siblings.</param>
        /// <param name="getParentFunc">A function that returns the parent of a node.</param>
        /// <param name="getChildrenFunc">A function that returns the children of a node.</param>
        /// <returns>An enumerable containing the node followed by its siblings.</returns>
        /// <remarks>
        /// If the node has no siblings, the returned sequence contains only the node itself.
        /// The node itself is always returned as the first element.
        /// </remarks>
        public static IEnumerable<TNode> WithSiblings<TNode>(this TNode node, Func<TNode, TNode?> getParentFunc, Func<TNode, IEnumerable<TNode>> getChildrenFunc)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy(getParentFunc, getChildrenFunc);
            return node.WithSiblings(strategy);
        }
        
        /// <summary>
        /// Returns the specified node together with its siblings by using the provided traversal strategy.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to return together with its siblings.</param>
        /// <param name="strategy">The traversal strategy used to resolve the parent and siblings of the node.</param>
        /// <returns>An enumerable containing the node followed by its siblings.</returns>
        /// <remarks>
        /// If the node has no siblings, the returned sequence contains only the node itself.
        /// The node itself is always returned as the first element.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="strategy"/> is <see langword="null"/>.</exception>
        public static IEnumerable<TNode> WithSiblings<TNode>(this TNode node, ITraversalStrategy<TNode> strategy)
		{
            if (strategy == null)
                throw new ArgumentNullException(nameof(strategy));

            return Iterator();

            IEnumerable<TNode> Iterator()
            {
                yield return node;

                foreach (var sibling in node.Siblings(strategy))
                    yield return sibling;
            }
        }

        #region Descendants

        /// <summary>
        /// Returns all descendants of the specified node.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node whose descendants are to be returned.</param>
        /// <returns>An enumerable containing all descendants of the node.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no traversal strategy can be derived from <typeparamref name="TNode"/>.
        /// </exception>
        public static IEnumerable<TNode> Descendants<TNode>(this TNode node)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy<TNode>();
            return node.Descendants(strategy);
        }

        /// <summary>
        /// Returns all descendants of the specified node using the given traversal mode.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node whose descendants are to be returned.</param>
        /// <param name="mode">The traversal mode to use.</param>
        /// <returns>An enumerable containing all descendants of the node in the specified traversal order.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no traversal strategy can be derived from <typeparamref name="TNode"/>.
        /// </exception>
        public static IEnumerable<TNode> Descendants<TNode>(this TNode node, TraversalMode mode)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy<TNode>();
            return node.Descendants(strategy, mode);
        }

        /// <summary>
        /// Returns all descendants of the specified node, sorting nodes using the given comparer.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node whose descendants are to be returned.</param>
        /// <param name="comparer">The comparer to use for sorting nodes.</param>
        /// <returns>An enumerable containing all descendants of the node, sorted according to the comparer.</returns>
        public static IEnumerable<TNode> Descendants<TNode>(this TNode node, IComparer<TNode> comparer)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy<TNode>();
            return node.Descendants(strategy, comparer);
        }

        /// <summary>
        /// Returns all descendants of the specified node by using the provided child selector.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node whose descendants are to be returned.</param>
        /// <param name="getChildrenFunc">A function that returns the children of a node.</param>
        /// <returns>An enumerable containing all descendants of the node.</returns>
        public static IEnumerable<TNode> Descendants<TNode>(
            this TNode node, 
            Func<TNode, IEnumerable<TNode>> getChildrenFunc)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy(getChildrenFunc);
            return node.Descendants(strategy);
        }

        /// <summary>
        /// Returns all descendants of the specified node by using the provided child selector and traversal mode.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node whose descendants are to be returned.</param>
        /// <param name="getChildrenFunc">A function that returns the children of a node.</param>
        /// <param name="mode">The traversal mode to use.</param>
        /// <returns>An enumerable containing all descendants of the node in the specified traversal order.</returns>
        public static IEnumerable<TNode> Descendants<TNode>(
            this TNode node, 
            Func<TNode, IEnumerable<TNode>> getChildrenFunc,
            TraversalMode mode)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy(getChildrenFunc);
            return node.Descendants(strategy, mode);
        }

        /// <summary>
        /// Returns all descendants of the specified node by using the provided child selector and comparer.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node whose descendants are to be returned.</param>
        /// <param name="getChildrenFunc">A function that returns the children of a node.</param>
        /// <param name="comparer">The comparer to use for sorting nodes.</param>
        /// <returns>An enumerable containing all descendants of the node, sorted according to the comparer.</returns>
        public static IEnumerable<TNode> Descendants<TNode>(
            this TNode node,
            Func<TNode, IEnumerable<TNode>> getChildrenFunc, 
            IComparer<TNode> comparer)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy(getChildrenFunc);
            return node.Descendants(strategy, comparer);
        }

        /// <summary>
        /// Returns all descendants of the specified node by using the provided traversal strategy.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node whose descendants are to be returned.</param>
        /// <param name="strategy">The traversal strategy used to resolve the children of the node.</param>
        /// <returns>An enumerable containing all descendants of the node.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="strategy"/> is <see langword="null"/>.</exception>
        public static IEnumerable<TNode> Descendants<TNode>(
            this TNode node,
            ITraversalStrategy<TNode> strategy)
        {
            return node.Descendants(strategy, TraversalMode.DepthFirst);
        }

        /// <summary>
        /// Returns all descendants of the specified node by using the provided traversal strategy and mode.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node whose descendants are to be returned.</param>
        /// <param name="strategy">The traversal strategy used to resolve the children of the node.</param>
        /// <param name="mode">The traversal mode to use.</param>
        /// <returns>An enumerable containing all descendants of the node in the specified traversal order.</returns>
        public static IEnumerable<TNode> Descendants<TNode>(
            this TNode node,
            ITraversalStrategy<TNode> strategy,
            TraversalMode mode)
        {
            var selector = CandidateSelectorFactory.Instance.CreateSelector<TNode>(mode);
            return node.Descendants(strategy, selector);
        }

        /// <summary>
        /// Returns all descendants of the specified node by using the provided traversal strategy and comparer.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node whose descendants are to be returned.</param>
        /// <param name="strategy">The traversal strategy used to resolve the children of the node.</param>
        /// <param name="comparer">The comparer to use for sorting nodes.</param>
        /// <returns>An enumerable containing all descendants of the node, sorted according to the comparer.</returns>
        public static IEnumerable<TNode> Descendants<TNode>(
            this TNode node,
            ITraversalStrategy<TNode> strategy,
            IComparer<TNode> comparer)
        {
            var selector = CandidateSelectorFactory.Instance.CreateSelector(comparer);
            return node.Descendants(strategy, selector);
        }

        private static IEnumerable<TNode> Descendants<TNode>(
            this TNode node, 
            ITraversalStrategy<TNode> strategy, 
            ICandidateSelector<TNode> selector)
		{
            if (strategy == null)
                throw new ArgumentNullException(nameof(strategy));

            return strategy.GetChildren(node).Traverse(strategy, selector);
        }

        #endregion

        #region WithDescendants

        /// <summary>
        /// Returns the specified node together with all of its descendants.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to return together with its descendants.</param>
        /// <returns>An enumerable containing the node followed by all of its descendants.</returns>
        /// <remarks>
        /// If the node has no children, the returned sequence contains only the node itself.
        /// </remarks>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no traversal strategy can be derived from <typeparamref name="TNode"/>.
        /// </exception>
        public static IEnumerable<TNode> WithDescendants<TNode>(this TNode node)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy<TNode>();
            return node.WithDescendants(strategy);
        }

        /// <summary>
        /// Returns the specified node together with all of its descendants using the given traversal mode.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to return together with its descendants.</param>
        /// <param name="mode">The traversal mode to use.</param>
        /// <returns>An enumerable containing the node followed by all of its descendants in the specified traversal order.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no traversal strategy can be derived from <typeparamref name="TNode"/>.
        /// </exception>
        public static IEnumerable<TNode> WithDescendants<TNode>(this TNode node, TraversalMode mode)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy<TNode>();
            return node.WithDescendants(strategy, mode);
        }

        /// <summary>
        /// Returns the specified node together with all of its descendants, sorting nodes using the given comparer.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to return together with its descendants.</param>
        /// <param name="comparer">The comparer to use for sorting nodes.</param>
        /// <returns>An enumerable containing the node followed by all of its descendants, sorted according to the comparer.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no traversal strategy can be derived from <typeparamref name="TNode"/>.
        /// </exception>
        public static IEnumerable<TNode> WithDescendants<TNode>(this TNode node, IComparer<TNode> comparer)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy<TNode>();
            return node.WithDescendants(strategy, comparer);
        }

        /// <summary>
        /// Returns the specified node together with all of its descendants by using the provided child selector.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to return together with its descendants.</param>
        /// <param name="getChildrenFunc">A function that returns the children of a node.</param>
        /// <returns>An enumerable containing the node followed by all of its descendants.</returns>
        public static IEnumerable<TNode> WithDescendants<TNode>(
            this TNode node,
            Func<TNode, IEnumerable<TNode>> getChildrenFunc)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy(getChildrenFunc);
            return node.WithDescendants(strategy);
        }

        /// <summary>
        /// Returns the specified node together with all of its descendants by using the provided child selector and traversal mode.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to return together with its descendants.</param>
        /// <param name="getChildrenFunc">A function that returns the children of a node.</param>
        /// <param name="mode">The traversal mode to use.</param>
        /// <returns>An enumerable containing the node followed by all of its descendants in the specified traversal order.</returns>
        public static IEnumerable<TNode> WithDescendants<TNode>(
            this TNode node,
            Func<TNode, IEnumerable<TNode>> getChildrenFunc,
            TraversalMode mode)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy(getChildrenFunc);
            return node.WithDescendants(strategy, mode);
        }

        /// <summary>
        /// Returns the specified node together with all of its descendants by using the provided child selector and comparer.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to return together with its descendants.</param>
        /// <param name="getChildrenFunc">A function that returns the children of a node.</param>
        /// <param name="comparer">The comparer to use for sorting nodes.</param>
        /// <returns>An enumerable containing the node followed by all of its descendants, sorted according to the comparer.</returns>
        public static IEnumerable<TNode> WithDescendants<TNode>(
            this TNode node,
            Func<TNode, IEnumerable<TNode>> getChildrenFunc,
            IComparer<TNode> comparer)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy(getChildrenFunc);
            return node.WithDescendants(strategy, comparer);
        }

        /// <summary>
        /// Returns the specified node together with all of its descendants by using the provided traversal strategy.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to return together with its descendants.</param>
        /// <param name="strategy">The traversal strategy used to resolve the children of the node.</param>
        /// <returns>An enumerable containing the node followed by all of its descendants.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="strategy"/> is <see langword="null"/>.</exception>
        public static IEnumerable<TNode> WithDescendants<TNode>(
            this TNode node,
            ITraversalStrategy<TNode> strategy)
        {
            return node.WithDescendants(strategy, TraversalMode.DepthFirst);
        }

        /// <summary>
        /// Returns the specified node together with all of its descendants by using the provided traversal strategy and mode.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to return together with its descendants.</param>
        /// <param name="strategy">The traversal strategy used to resolve the children of the node.</param>
        /// <param name="mode">The traversal mode to use.</param>
        /// <returns>An enumerable containing the node followed by all of its descendants in the specified traversal order.</returns>
        public static IEnumerable<TNode> WithDescendants<TNode>(
            this TNode node,
            ITraversalStrategy<TNode> strategy,
            TraversalMode mode)
        {
            var selector = CandidateSelectorFactory.Instance.CreateSelector<TNode>(mode);
            return node.WithDescendants(strategy, selector);
        }

        /// <summary>
        /// Returns the specified node together with all of its descendants by using the provided traversal strategy and comparer.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to return together with its descendants.</param>
        /// <param name="strategy">The traversal strategy used to resolve the children of the node.</param>
        /// <param name="comparer">The comparer to use for sorting nodes.</param>
        /// <returns>An enumerable containing the node followed by all of its descendants, sorted according to the comparer.</returns>
        public static IEnumerable<TNode> WithDescendants<TNode>(
            this TNode node,
            ITraversalStrategy<TNode> strategy,
            IComparer<TNode> comparer)
        {
            var selector = CandidateSelectorFactory.Instance.CreateSelector(comparer);
            return node.WithDescendants(strategy, selector);
        }

        private static IEnumerable<TNode> WithDescendants<TNode>(
            this TNode node,
            ITraversalStrategy<TNode> strategy,
            ICandidateSelector<TNode> selector)
        {
            return node.Traverse(strategy, selector);
        }

        #endregion

        #region Ancestors

        /// <summary>
        /// Returns the ancestors of the specified node.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node whose ancestors are to be returned.</param>
        /// <returns>An enumerable containing all ancestors of the node.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no traversal strategy can be derived from <typeparamref name="TNode"/>.
        /// </exception>
		public static IEnumerable<TNode> Ancestors<TNode>(this TNode node)
		{
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy<TNode>();
            return node.Ancestors(strategy);
        }

        /// <summary>
        /// Returns the ancestors of the specified node by using the provided parent selector.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node whose ancestors are to be returned.</param>
        /// <param name="getParentFunc">A function that returns the parent of a node.</param>
        /// <returns>An enumerable containing all ancestors of the node.</returns>
        /// <remarks>
        /// The enumerable contains the node's parent first, followed by its parent, and so on.
        /// </remarks>
        public static IEnumerable<TNode> Ancestors<TNode>(this TNode node, Func<TNode, TNode?> getParentFunc)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy(getParentFunc);
            return node.Ancestors(strategy);
        }

        /// <summary>
        /// Returns the ancestors of the specified node by using the provided traversal strategy.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node whose ancestors are to be returned.</param>
        /// <param name="strategy">The traversal strategy used to resolve the parent of the node.</param>
        /// <returns>An enumerable containing all ancestors of the node.</returns>
        /// <remarks>
        /// The enumerable contains the node's parent first, followed by its parent, and so on.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="strategy"/> is <see langword="null"/>.</exception>
        public static IEnumerable<TNode> Ancestors<TNode>(this TNode node, ITraversalStrategy<TNode> strategy)
        {
            if (strategy == null)
                throw new ArgumentNullException(nameof(strategy));

            return Iterator();

            IEnumerable<TNode> Iterator()
            {
                var tmpNode = strategy.GetParent(node);

                while (tmpNode != null)
                {
                    yield return tmpNode;
                    tmpNode = strategy.GetParent(tmpNode);
                }
            }
        }

        #endregion

        #region WithAncestors

        /// <summary>
        /// Returns the specified node together with all of its ancestors.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to return together with its ancestors.</param>
        /// <returns>An enumerable containing the node followed by all of its ancestors.</returns>
        /// <remarks>
        /// If the node has no parent, the returned sequence contains only the node itself.
        /// The node itself is always returned as the first element.
        /// </remarks>
        public static IEnumerable<TNode> WithAncestors<TNode>(this TNode node)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy<TNode>();
            return node.WithAncestors(strategy);
        }

        /// <summary>
        /// Returns the specified node together with all of its ancestors by using the provided parent selector.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to return together with its ancestors.</param>
        /// <param name="getParentFunc">A function that returns the parent of a node.</param>
        /// <returns>An enumerable containing the node followed by all of its ancestors.</returns>
        /// <remarks>
        /// If the node has no parent, the returned sequence contains only the node itself.
        /// The node itself is always returned as the first element.
        /// </remarks>
        public static IEnumerable<TNode> WithAncestors<TNode>(this TNode node, Func<TNode, TNode?> getParentFunc)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy(getParentFunc);
            return node.WithAncestors(strategy);
        }

        /// <summary>
        /// Returns the specified node together with all of its ancestors by using the provided traversal strategy.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to return together with its ancestors.</param>
        /// <param name="strategy">The traversal strategy used to resolve the parent of the node.</param>
        /// <returns>An enumerable containing the node followed by all of its ancestors.</returns>
        /// <remarks>
        /// If the node has no parent, the returned sequence contains only the node itself.
        /// The node itself is always returned as the first element.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="strategy"/> is <see langword="null"/>.</exception>
        public static IEnumerable<TNode> WithAncestors<TNode>(this TNode node, ITraversalStrategy<TNode> strategy)
		{
			yield return node;

			foreach (var ancestor in node.Ancestors(strategy))
				yield return ancestor;
		}

        #endregion

        #region Leaves

        /// <summary>
        /// Returns all leaf nodes in the subtree rooted at the specified node.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The root node of the subtree to inspect.</param>
        /// <returns>An enumerable containing all leaf nodes in the subtree. If the specified node is a leaf, the sequence contains that node.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no traversal strategy can be derived from <typeparamref name="TNode"/>.
        /// </exception>
        public static IEnumerable<TNode> Leaves<TNode>(this TNode node)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy<TNode>();
            return node.Leaves(strategy);
        }

        /// <summary>
        /// Returns all leaf nodes in the subtree rooted at the specified node using the given traversal mode.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The root node of the subtree to inspect.</param>
        /// <param name="mode">The traversal mode to use.</param>
        /// <returns>An enumerable containing all leaf nodes in the subtree. If the specified node is a leaf, the sequence contains that node.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no traversal strategy can be derived from <typeparamref name="TNode"/>.
        /// </exception>
        public static IEnumerable<TNode> Leaves<TNode>(this TNode node, TraversalMode mode)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy<TNode>();
            return node.Leaves(strategy, mode);
        }

        /// <summary>
        /// Returns all leaf nodes in the subtree rooted at the specified node, ordering traversal candidates with the given comparer.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The root node of the subtree to inspect.</param>
        /// <param name="comparer">Defines which node to prefer over another during traversal.</param>
        /// <returns>An enumerable containing all leaf nodes in the subtree. If the specified node is a leaf, the sequence contains that node.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no traversal strategy can be derived from <typeparamref name="TNode"/>.
        /// </exception>
        public static IEnumerable<TNode> Leaves<TNode>(this TNode node, IComparer<TNode> comparer)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy<TNode>();
            return node.Leaves(strategy, comparer);
        }

        /// <summary>
        /// Returns all leaf nodes in the subtree rooted at the specified node by using the provided child selector.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The root node of the subtree to inspect.</param>
        /// <param name="getChildrenFunc">A function that returns the children of a node.</param>
        /// <returns>An enumerable containing all leaf nodes in the subtree. If the specified node is a leaf, the sequence contains that node.</returns>
        public static IEnumerable<TNode> Leaves<TNode>(
            this TNode node,
            Func<TNode, IEnumerable<TNode>> getChildrenFunc)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy(getChildrenFunc);
            return node.Leaves(strategy);
        }

        /// <summary>
        /// Returns all leaf nodes in the subtree rooted at the specified node by using the provided child selector and traversal mode.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The root node of the subtree to inspect.</param>
        /// <param name="getChildrenFunc">A function that returns the children of a node.</param>
        /// <param name="mode">The traversal mode to use.</param>
        /// <returns>An enumerable containing all leaf nodes in the subtree. If the specified node is a leaf, the sequence contains that node.</returns>
        public static IEnumerable<TNode> Leaves<TNode>(
            this TNode node,
            Func<TNode, IEnumerable<TNode>> getChildrenFunc,
            TraversalMode mode)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy(getChildrenFunc);
            return node.Leaves(strategy, mode);
        }

        /// <summary>
        /// Returns all leaf nodes in the subtree rooted at the specified node by using the provided child selector and comparer.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The root node of the subtree to inspect.</param>
        /// <param name="getChildrenFunc">A function that returns the children of a node.</param>
        /// <param name="comparer">Defines which node to prefer over another during traversal.</param>
        /// <returns>An enumerable containing all leaf nodes in the subtree. If the specified node is a leaf, the sequence contains that node.</returns>
        public static IEnumerable<TNode> Leaves<TNode>(
            this TNode node,
            Func<TNode, IEnumerable<TNode>> getChildrenFunc,
            IComparer<TNode> comparer)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy(getChildrenFunc);
            return node.Leaves(strategy, comparer);
        }

        /// <summary>
        /// Returns all leaf nodes in the subtree rooted at the specified node by using the provided traversal strategy.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The root node of the subtree to inspect.</param>
        /// <param name="strategy">The traversal strategy used to resolve the children of the nodes.</param>
        /// <returns>An enumerable containing all leaf nodes in the subtree. If the specified node is a leaf, the sequence contains that node.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="strategy"/> is <see langword="null"/>.</exception>
        public static IEnumerable<TNode> Leaves<TNode>(
            this TNode node,
            ITraversalStrategy<TNode> strategy)
        {
            return node.Leaves(strategy, TraversalMode.DepthFirst);
        }

        /// <summary>
        /// Returns all leaf nodes in the subtree rooted at the specified node by using the provided traversal strategy and mode.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The root node of the subtree to inspect.</param>
        /// <param name="strategy">The traversal strategy used to resolve the children of the nodes.</param>
        /// <param name="mode">The traversal mode to use.</param>
        /// <returns>An enumerable containing all leaf nodes in the subtree. If the specified node is a leaf, the sequence contains that node.</returns>
        public static IEnumerable<TNode> Leaves<TNode>(
            this TNode node,
            ITraversalStrategy<TNode> strategy,
            TraversalMode mode)
        {
            var selector = CandidateSelectorFactory.Instance.CreateSelector<TNode>(mode);
            return node.Leaves(strategy, selector);
        }

        /// <summary>
        /// Returns all leaf nodes in the subtree rooted at the specified node by using the provided traversal strategy and comparer.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The root node of the subtree to inspect.</param>
        /// <param name="strategy">The traversal strategy used to resolve the children of the nodes.</param>
        /// <param name="comparer">Defines which node to prefer over another during traversal.</param>
        /// <returns>An enumerable containing all leaf nodes in the subtree. If the specified node is a leaf, the sequence contains that node.</returns>
        public static IEnumerable<TNode> Leaves<TNode>(
            this TNode node,
            ITraversalStrategy<TNode> strategy,
            IComparer<TNode> comparer)
        {
            var selector = CandidateSelectorFactory.Instance.CreateSelector(comparer);
            return node.Leaves(strategy, selector);
        }

        /// <summary>
        /// Returns all leaf nodes in the subtree rooted at the specified node by using the provided traversal strategy and candidate selector.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The root node of the subtree to inspect.</param>
        /// <param name="strategy">The traversal strategy used to resolve the children of the nodes.</param>
        /// <param name="selector">The candidate selector that controls traversal order.</param>
        /// <returns>An enumerable containing all leaf nodes in the subtree. If the specified node is a leaf, the sequence contains that node.</returns>
        private static IEnumerable<TNode> Leaves<TNode>(
            this TNode node,
            ITraversalStrategy<TNode> strategy,
            ICandidateSelector<TNode> selector)
        {
            return node.WithDescendants(strategy, selector).Where(x => x.IsLeaf(strategy));
        }

        #endregion

        #region InnerNodes

        /// <summary>
        /// Returns all inner nodes in the subtree rooted at the specified node, excluding the node itself.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The root node of the subtree to inspect.</param>
        /// <returns>An enumerable containing all inner descendant nodes.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no traversal strategy can be derived from <typeparamref name="TNode"/>.
        /// </exception>
        public static IEnumerable<TNode> InnerNodes<TNode>(this TNode node)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy<TNode>();
            return node.InnerNodes(strategy);
        }

        /// <summary>
        /// Returns all inner nodes in the subtree rooted at the specified node, excluding the node itself, using the given traversal mode.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The root node of the subtree to inspect.</param>
        /// <param name="mode">The traversal mode to use.</param>
        /// <returns>An enumerable containing all inner descendant nodes.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no traversal strategy can be derived from <typeparamref name="TNode"/>.
        /// </exception>
        public static IEnumerable<TNode> InnerNodes<TNode>(this TNode node, TraversalMode mode)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy<TNode>();
            return node.InnerNodes(strategy, mode);
        }

        /// <summary>
        /// Returns all inner nodes in the subtree rooted at the specified node, excluding the node itself, ordering traversal candidates with the given comparer.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The root node of the subtree to inspect.</param>
        /// <param name="comparer">Defines which node to prefer over another during traversal.</param>
        /// <returns>An enumerable containing all inner descendant nodes.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no traversal strategy can be derived from <typeparamref name="TNode"/>.
        /// </exception>
        public static IEnumerable<TNode> InnerNodes<TNode>(this TNode node, IComparer<TNode> comparer)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy<TNode>();
            return node.InnerNodes(strategy, comparer);
        }

        /// <summary>
        /// Returns all inner nodes in the subtree rooted at the specified node, excluding the node itself, by using the provided parent and child selectors.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The root node of the subtree to inspect.</param>
        /// <param name="getParentFunc">A function that returns the parent of a node.</param>
        /// <param name="getChildrenFunc">A function that returns the children of a node.</param>
        /// <returns>An enumerable containing all inner descendant nodes.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no traversal strategy can be derived from <typeparamref name="TNode"/>.
        /// </exception>
        public static IEnumerable<TNode> InnerNodes<TNode>(
            this TNode node,
            Func<TNode, TNode?> getParentFunc,
            Func<TNode, IEnumerable<TNode>> getChildrenFunc)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy(getParentFunc, getChildrenFunc);
            return node.InnerNodes(strategy);
        }

        /// <summary>
        /// Returns all inner nodes in the subtree rooted at the specified node, excluding the node itself, by using the provided parent and child selectors and traversal mode.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The root node of the subtree to inspect.</param>
        /// <param name="getParentFunc">A function that returns the parent of a node.</param>
        /// <param name="getChildrenFunc">A function that returns the children of a node.</param>
        /// <param name="mode">The traversal mode to use.</param>
        /// <returns>An enumerable containing all inner descendant nodes.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no traversal strategy can be derived from <typeparamref name="TNode"/>.
        /// </exception>
        public static IEnumerable<TNode> InnerNodes<TNode>(
            this TNode node,
            Func<TNode, TNode?> getParentFunc,
            Func<TNode, IEnumerable<TNode>> getChildrenFunc,
            TraversalMode mode)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy(getParentFunc, getChildrenFunc);
            return node.InnerNodes(strategy, mode);
        }

        /// <summary>
        /// Returns all inner nodes in the subtree rooted at the specified node, excluding the node itself, by using the provided parent and child selectors and comparer.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The root node of the subtree to inspect.</param>
        /// <param name="getParentFunc">A function that returns the parent of a node.</param>
        /// <param name="getChildrenFunc">A function that returns the children of a node.</param>
        /// <param name="comparer">Defines which node to prefer over another during traversal.</param>
        /// <returns>An enumerable containing all inner descendant nodes.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no traversal strategy can be derived from <typeparamref name="TNode"/>.
        /// </exception>
        public static IEnumerable<TNode> InnerNodes<TNode>(
            this TNode node,
            Func<TNode, TNode?> getParentFunc,
            Func<TNode, IEnumerable<TNode>> getChildrenFunc,
            IComparer<TNode> comparer)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy(getParentFunc, getChildrenFunc);
            return node.InnerNodes(strategy, comparer);
        }

        /// <summary>
        /// Returns all inner nodes in the subtree rooted at the specified node, excluding the node itself, by using the provided traversal strategy.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The root node of the subtree to inspect.</param>
        /// <param name="strategy">The traversal strategy used to resolve the parent and children of the node.</param>
        /// <returns>An enumerable containing all inner descendant nodes.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="strategy"/> is <see langword="null"/>.</exception>
        public static IEnumerable<TNode> InnerNodes<TNode>(
            this TNode node,
            ITraversalStrategy<TNode> strategy)
        {
            return node.InnerNodes(strategy, TraversalMode.DepthFirst);
        }

        /// <summary>
        /// Returns all inner nodes in the subtree rooted at the specified node, excluding the node itself, by using the provided traversal strategy and mode.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The root node of the subtree to inspect.</param>
        /// <param name="strategy">The traversal strategy used to resolve the parent and children of the node.</param>
        /// <param name="mode">The traversal mode to use.</param>
        /// <returns>An enumerable containing all inner descendant nodes.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="strategy"/> is <see langword="null"/>.</exception>
        public static IEnumerable<TNode> InnerNodes<TNode>(
            this TNode node,
            ITraversalStrategy<TNode> strategy,
            TraversalMode mode)
        {
            var selector = CandidateSelectorFactory.Instance.CreateSelector<TNode>(mode);
            return node.InnerNodes(strategy, selector);
        }

        /// <summary>
        /// Returns all inner nodes in the subtree rooted at the specified node, excluding the node itself, by using the provided traversal strategy and comparer.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The root node of the subtree to inspect.</param>
        /// <param name="strategy">The traversal strategy used to resolve the parent and children of the node.</param>
        /// <param name="comparer">Defines which node to prefer over another during traversal.</param>
        /// <returns>An enumerable containing all inner descendant nodes.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="strategy"/> is <see langword="null"/>.</exception>
        public static IEnumerable<TNode> InnerNodes<TNode>(
            this TNode node,
            ITraversalStrategy<TNode> strategy,
            IComparer<TNode> comparer)
        {
            var selector = CandidateSelectorFactory.Instance.CreateSelector(comparer);
            return node.InnerNodes(strategy, selector);
        }

        /// <summary>
        /// Returns all inner nodes in the subtree rooted at the specified node, excluding the node itself, by using the provided traversal strategy and candidate selector.
        /// </summary>
        private static IEnumerable<TNode> InnerNodes<TNode>(
            this TNode node,
            ITraversalStrategy<TNode> strategy,
            ICandidateSelector<TNode> selector)
        {
            return node.Descendants(strategy, selector).Where(x => x.IsInnerNode(strategy));
        }

        #endregion

        #region WithInnerNodes

        /// <summary>
        /// Returns the specified node together with all inner nodes in its subtree.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The root node of the subtree to inspect.</param>
        /// <returns>An enumerable containing the node and all inner nodes in its subtree.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no traversal strategy can be derived from <typeparamref name="TNode"/>.
        /// </exception>
        public static IEnumerable<TNode> WithInnerNodes<TNode>(this TNode node)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy<TNode>();
            return node.WithInnerNodes(strategy);
        }

        /// <summary>
        /// Returns the specified node together with all inner nodes in its subtree using the given traversal mode.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The root node of the subtree to inspect.</param>
        /// <param name="mode">The traversal mode to use.</param>
        /// <returns>An enumerable containing the node and all inner nodes in its subtree.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no traversal strategy can be derived from <typeparamref name="TNode"/>.
        /// </exception>
        public static IEnumerable<TNode> WithInnerNodes<TNode>(this TNode node, TraversalMode mode)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy<TNode>();
            return node.WithInnerNodes(strategy, mode);
        }

        /// <summary>
        /// Returns the specified node together with all inner nodes in its subtree, ordering traversal candidates with the given comparer.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The root node of the subtree to inspect.</param>
        /// <param name="comparer">Defines which node to prefer over another during traversal.</param>
        /// <returns>An enumerable containing the node and all inner nodes in its subtree..</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no traversal strategy can be derived from <typeparamref name="TNode"/>.
        /// </exception>
        public static IEnumerable<TNode> WithInnerNodes<TNode>(this TNode node, IComparer<TNode> comparer)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy<TNode>();
            return node.WithInnerNodes(strategy, comparer);
        }

        /// <summary>
        /// Returns the specified node together with all inner nodes in its subtree by using the provided parent and child selectors.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The root node of the subtree to inspect.</param>
        /// <param name="getParentFunc">A function that returns the parent of a node.</param>
        /// <param name="getChildrenFunc">A function that returns the children of a node.</param>
        /// <returns>An enumerable containing the node and all inner nodes in its subtree.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no traversal strategy can be derived from <typeparamref name="TNode"/>.
        /// </exception>
        public static IEnumerable<TNode> WithInnerNodes<TNode>(
            this TNode node,
            Func<TNode, TNode?> getParentFunc,
            Func<TNode, IEnumerable<TNode>> getChildrenFunc)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy(getParentFunc, getChildrenFunc);
            return node.WithInnerNodes(strategy);
        }

        /// <summary>
        /// Returns the specified node together with all inner nodes in its subtree by using the provided parent and child selectors and traversal mode.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The root node of the subtree to inspect.</param>
        /// <param name="getParentFunc">A function that returns the parent of a node.</param>
        /// <param name="getChildrenFunc">A function that returns the children of a node.</param>
        /// <param name="mode">The traversal mode to use.</param>
        /// <returns>An enumerable containing the node and all inner nodes in its subtree.</returns>
        public static IEnumerable<TNode> WithInnerNodes<TNode>(
            this TNode node,
            Func<TNode, TNode?> getParentFunc,
            Func<TNode, IEnumerable<TNode>> getChildrenFunc,
            TraversalMode mode)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy(getParentFunc, getChildrenFunc);
            return node.WithInnerNodes(strategy, mode);
        }

        /// <summary>
        /// Returns the specified node together with all inner nodes in its subtree by using the provided parent and child selectors and comparer.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The root node of the subtree to inspect.</param>
        /// <param name="getParentFunc">A function that returns the parent of a node.</param>
        /// <param name="getChildrenFunc">A function that returns the children of a node.</param>
        /// <param name="comparer">Defines which node to prefer over another during traversal.</param>
        /// <returns>An enumerable containing the node and all inner nodes in its subtree.</returns>
        public static IEnumerable<TNode> WithInnerNodes<TNode>(
            this TNode node,
            Func<TNode, TNode?> getParentFunc,
            Func<TNode, IEnumerable<TNode>> getChildrenFunc,
            IComparer<TNode> comparer)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy(getParentFunc, getChildrenFunc);
            return node.WithInnerNodes(strategy, comparer);
        }

        /// <summary>
        /// Returns the specified node together with all inner nodes in its subtree by using the provided traversal strategy.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The root node of the subtree to inspect.</param>
        /// <param name="strategy">The traversal strategy used to resolve the parent and children of the node.</param>
        /// <returns>An enumerable containing the node and all inner nodes in its subtree.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="strategy"/> is <see langword="null"/>.</exception>
        public static IEnumerable<TNode> WithInnerNodes<TNode>(
            this TNode node,
            ITraversalStrategy<TNode> strategy)
        {
            return node.WithInnerNodes(strategy, TraversalMode.DepthFirst);
        }

        /// <summary>
        /// Returns the specified node together with all inner nodes in its subtree by using the provided traversal strategy and mode.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The root node of the subtree to inspect.</param>
        /// <param name="strategy">The traversal strategy used to resolve the parent and children of the node.</param>
        /// <param name="mode">The traversal mode to use.</param>
        /// <returns>An enumerable containing the node and all inner nodes in its subtree.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="strategy"/> is <see langword="null"/>.</exception>
        public static IEnumerable<TNode> WithInnerNodes<TNode>(
            this TNode node,
            ITraversalStrategy<TNode> strategy,
            TraversalMode mode)
        {
            var selector = CandidateSelectorFactory.Instance.CreateSelector<TNode>(mode);
            return node.WithInnerNodes(strategy, selector);
        }

        /// <summary>
        /// Returns the specified node together with all inner nodes in its subtree by using the provided traversal strategy and comparer.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The root node of the subtree to inspect.</param>
        /// <param name="strategy">The traversal strategy used to resolve the parent and children of the node.</param>
        /// <param name="comparer">Defines which node to prefer over another during traversal.</param>
        /// <returns>An enumerable containing the node and all inner nodes in its subtree.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="strategy"/> is <see langword="null"/>.</exception>
        public static IEnumerable<TNode> WithInnerNodes<TNode>(
            this TNode node,
            ITraversalStrategy<TNode> strategy,
            IComparer<TNode> comparer)
		{
            var selector = CandidateSelectorFactory.Instance.CreateSelector(comparer);
            return node.WithInnerNodes(strategy, selector);
        }

        /// <summary>
        /// Returns the specified node together with all inner nodes in its subtree by using the provided traversal strategy and candidate selector.
        /// </summary>
        private static IEnumerable<TNode> WithInnerNodes<TNode>(
            this TNode node,
            ITraversalStrategy<TNode> strategy,
            ICandidateSelector<TNode> selector)
        {
            yield return node;

            foreach (var innerNode in node.InnerNodes(strategy, selector))
                yield return innerNode;
		}

        #endregion

        #endregion

        #region Traverse Methods

        #region Returning Enumerables

        #region Starting from Single Node

        /// <summary>
        /// Traverses the hierarchy starting from the specified node.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to start traversal from.</param>
        /// <param name="cancellationToken">A token that can be used to cancel traversal.</param>
        /// <returns>An enumerable sequence containing the traversed nodes.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no traversal strategy can be derived from <typeparamref name="TNode"/>.
        /// </exception>
        public static IEnumerable<TNode> Traverse<TNode>(
            this TNode node,
            CancellationToken cancellationToken = default)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy<TNode>();
            return node.Traverse(strategy, cancellationToken);
        }

        /// <summary>
        /// Traverses the hierarchy starting from the specified node using the given traversal mode.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to start the traversal from.</param>
        /// <param name="mode">The traversal mode to use.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the traversal.</param>
        /// <returns>
        /// An enumerable sequence of nodes of type <typeparamref name="TNode}"/> in the specified traversal order.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no traversal strategy can be derived from <typeparamref name="TNode"/>.
        /// </exception>
        public static IEnumerable<TNode> Traverse<TNode>(
            this TNode node,
            TraversalMode mode,
            CancellationToken cancellationToken = default)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy<TNode>();
            return node.Traverse(strategy, mode, cancellationToken);
        }

        /// <summary>
        /// Traverses the hierarchy starting from the specified node, ordering traversal candidates with the given comparer.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to start the traversal from.</param>
        /// <param name="comparer">The comparer to use for sorting nodes.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the traversal.</param>
        /// <returns>
        /// An enumerable sequence of nodes of type <typeparamref name="TNode"/>,
        /// sorted according to the specified comparer.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no traversal strategy can be derived from <typeparamref name="TNode"/>.
        /// </exception>
        public static IEnumerable<TNode> Traverse<TNode>(
            this TNode node,
            IComparer<TNode> comparer,
            CancellationToken cancellationToken = default)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy<TNode>();
            return node.Traverse(strategy, comparer, cancellationToken);
        }

        /// <summary>
        /// Traverses the hierarchy starting from the specified node using the given candidate selector.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to start the traversal from.</param>
        /// <param name="selector">The selector to use for identifying candidate nodes.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the traversal.</param>
        /// <returns>
        /// An enumerable sequence of nodes of type <typeparamref name="TNode"/>.
        /// </returns>
        public static IEnumerable<TNode> Traverse<TNode>(
            this TNode node,
            ICandidateSelector<TNode> selector,
            CancellationToken cancellationToken = default)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy<TNode>();
            return node.Traverse(strategy, selector, cancellationToken);
        }

        /// <summary>
        /// Traverses the hierarchy starting from the specified node by using the provided child selector.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to start the traversal from.</param>
        /// <param name="getChildrenFunc">A function that returns the children of a node.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the traversal.</param>
        /// <returns>
        /// An enumerable sequence of nodes of type <typeparamref name="TNode"/>.
        /// </returns>
        public static IEnumerable<TNode> Traverse<TNode>(
            this TNode node,
            Func<TNode, IEnumerable<TNode>> getChildrenFunc,
            CancellationToken cancellationToken = default)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy(getChildrenFunc);
            return node.Traverse(strategy, cancellationToken);
        }

        /// <summary>
        /// Traverses the hierarchy starting from the specified node by using the provided child selector and traversal mode.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to start the traversal from.</param>
        /// <param name="getChildrenFunc">A function that returns the children of a node.</param>
        /// <param name="mode">The traversal mode to use.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the traversal.</param>
        /// <returns>
        /// An enumerable sequence of nodes of type <typeparamref name="TNode"/> in the specified traversal order.
        /// </returns>
        public static IEnumerable<TNode> Traverse<TNode>(
            this TNode node,
            Func<TNode, IEnumerable<TNode>> getChildrenFunc,
            TraversalMode mode,
            CancellationToken cancellationToken = default)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy(getChildrenFunc);
            return node.Traverse(strategy, mode, cancellationToken);
        }

        /// <summary>
        /// Traverses the hierarchy starting from the specified node by using the provided child selector and comparer.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to start the traversal from.</param>
        /// <param name="getChildrenFunc">A function that returns the children of a node.</param>
        /// <param name="comparer">The comparer to use for sorting nodes.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the traversal.</param>
        /// <returns>
        /// An enumerable sequence of nodes of type <typeparamref name="TNode"/>,
        /// sorted according to the specified comparer.
        /// </returns>
        public static IEnumerable<TNode> Traverse<TNode>(
            this TNode node,
            Func<TNode, IEnumerable<TNode>> getChildrenFunc,
            IComparer<TNode> comparer,
            CancellationToken cancellationToken = default)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy(getChildrenFunc);
            return node.Traverse(strategy, comparer, cancellationToken);
        }

        /// <summary>
        /// Traverses the hierarchy starting from the specified node by using the provided child selector and candidate selector.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to start the traversal from.</param>
        /// <param name="getChildrenFunc">A function that returns the children of a node.</param>
        /// <param name="selector">The selector to use for identifying candidate nodes.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the traversal.</param>
        /// <returns>
        /// An enumerable sequence of nodes of type <typeparamref name="TNode"/>.
        /// </returns>
        public static IEnumerable<TNode> Traverse<TNode>(
            this TNode node,
            Func<TNode, IEnumerable<TNode>> getChildrenFunc,
            ICandidateSelector<TNode> selector,
            CancellationToken cancellationToken = default)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy(getChildrenFunc);
            return node.Traverse(strategy, selector, cancellationToken);
        }

        /// <summary>
        /// Traverses the hierarchy starting from the specified node by using the provided traversal strategy.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to start the traversal from.</param>
        /// <param name="strategy">The traversal strategy used to resolve the parent and children of the node.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the traversal.</param>
        /// <returns>
        /// An enumerable sequence of nodes of type <typeparamref name="TNode"/> in the depth-first order.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="strategy"/> is <see langword="null"/>.</exception>
        public static IEnumerable<TNode> Traverse<TNode>(
            this TNode node,
            ITraversalStrategy<TNode> strategy,
            CancellationToken cancellationToken = default)
        {
            return node.Traverse(strategy, TraversalMode.DepthFirst, cancellationToken);
        }

        /// <summary>
        /// Traverses the hierarchy starting from the specified node by using the provided traversal strategy and traversal mode.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to start the traversal from.</param>
        /// <param name="strategy">The traversal strategy used to resolve the parent and children of the node.</param>
        /// <param name="mode">The traversal mode to use.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the traversal.</param>
        /// <returns>
        /// An enumerable sequence of nodes of type <typeparamref name="TNode"/> in the specified traversal order.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="strategy"/> is <see langword="null"/>.</exception>
        public static IEnumerable<TNode> Traverse<TNode>(
            this TNode node,
            ITraversalStrategy<TNode> strategy,
            TraversalMode mode,
            CancellationToken cancellationToken = default)
        {
            var selector = CandidateSelectorFactory.Instance.CreateSelector<TNode>(mode);
            return node.Traverse(strategy, selector, cancellationToken);
        }

        /// <summary>
        /// Traverses the hierarchy starting from the specified node by using the provided traversal strategy and comparer.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to start the traversal from.</param>
        /// <param name="strategy">The traversal strategy used to resolve the parent and children of the node.</param>
        /// <param name="comparer">The comparer to use for sorting nodes.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the traversal.</param>
        /// <returns>
        /// An enumerable sequence of nodes of type <typeparamref name="TNode"/>,
        /// sorted according to the specified comparer.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="strategy"/> is <see langword="null"/>.</exception>
        public static IEnumerable<TNode> Traverse<TNode>(
            this TNode node,
            ITraversalStrategy<TNode> strategy,
            IComparer<TNode> comparer,
            CancellationToken cancellationToken = default)
        {
            var selector = CandidateSelectorFactory.Instance.CreateSelector(comparer);
            return node.Traverse(strategy, selector, cancellationToken);
        }

        /// <summary>
        /// Traverses the hierarchy starting from the specified node by using the provided traversal strategy and candidate selector.
        /// When invoked, the node will be added to the candidate selector. The selector is then used for traversal.
        /// The candidate selector is reset on each iteration in order to prevent misaligned situations.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to start the traversal from.</param>
        /// <param name="strategy">The traversal strategy used to resolve the parent and children of the node.</param>
        /// <param name="selector">The selector to use for identifying candidate nodes.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the traversal.</param>
        /// <returns>
        /// An enumerable sequence of nodes of type <typeparamref name="TNode"/>.
        /// </returns>
        public static IEnumerable<TNode> Traverse<TNode>(
            this TNode node,
            ITraversalStrategy<TNode> strategy,
            ICandidateSelector<TNode> selector,
            CancellationToken cancellationToken = default)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Iterator();

            IEnumerable<TNode> Iterator()
            {
                selector.Clear();
                selector.Add(node);

                foreach(var item in Traverse(strategy, selector, cancellationToken))
                {
                    yield return item;
                }
            }
        }

        #endregion

        #region Starting from Multiple Nodes

        /// <summary>
        /// Traverses the hierarchy starting from the specified nodes.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="nodes">The nodes to start traversal from.</param>
        /// <param name="cancellationToken">A token that can be used to cancel traversal.</param>
        /// <returns>An enumerable sequence containing the traversed nodes.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no traversal strategy can be derived from <typeparamref name="TNode"/>.
        /// </exception>
        public static IEnumerable<TNode> Traverse<TNode>(
            this IEnumerable<TNode> nodes,
            CancellationToken cancellationToken = default)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy<TNode>();
            return nodes.Traverse(strategy, cancellationToken);
        }

        /// <summary>
        /// Traverses the hierarchy starting from the specified nodes using the given traversal mode.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="nodes">The nodes to start traversal from.</param>
        /// <param name="mode">The traversal mode to use.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the traversal.</param>
        /// <returns>
        /// An enumerable sequence of nodes of type <typeparamref name="TNode}"/> in the specified traversal order.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no traversal strategy can be derived from <typeparamref name="TNode"/>.
        /// </exception>
        public static IEnumerable<TNode> Traverse<TNode>(
            this IEnumerable<TNode> nodes,
            TraversalMode mode,
            CancellationToken cancellationToken = default)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy<TNode>();
            return nodes.Traverse(strategy, mode, cancellationToken);
        }

        /// <summary>
        /// Traverses the hierarchy starting from the specified nodes, ordering traversal candidates with the given comparer.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="nodes">The nodes to start traversal from.</param>
        /// <param name="comparer">The comparer to use for sorting nodes.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the traversal.</param>
        /// <returns>An enumerable sequence containing the traversed nodes.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no traversal strategy can be derived from <typeparamref name="TNode"/>.
        /// </exception>
        public static IEnumerable<TNode> Traverse<TNode>(
            this IEnumerable<TNode> nodes,
            IComparer<TNode> comparer,
            CancellationToken cancellationToken = default)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy<TNode>();
            return nodes.Traverse(strategy, comparer, cancellationToken);
        }

        /// <summary>
        /// Traverses the hierarchy starting from the specified nodes using the given candidate selector.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="nodes">The nodes to start traversal from.</param>
        /// <param name="selector">The selector to use for identifying candidate nodes.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the traversal.</param>
        /// <returns>An enumerable sequence containing the traversed nodes.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no traversal strategy can be derived from <typeparamref name="TNode"/>.
        /// </exception>
        public static IEnumerable<TNode> Traverse<TNode>(
            this IEnumerable<TNode> nodes,
            ICandidateSelector<TNode> selector,
            CancellationToken cancellationToken = default)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy<TNode>();
            return nodes.Traverse(strategy, selector, cancellationToken);
        }

        /// <summary>
        /// Traverses the hierarchy starting from the specified nodes by using the provided child selector.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="nodes">The nodes to start traversal from.</param>
        /// <param name="getChildrenFunc">A function that returns the children of a node.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the traversal.</param>
        /// <returns>An enumerable sequence containing the traversed nodes.</returns>
        public static IEnumerable<TNode> Traverse<TNode>(
            this IEnumerable<TNode> nodes,
            Func<TNode, IEnumerable<TNode>> getChildrenFunc,
            CancellationToken cancellationToken = default)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy(getChildrenFunc);
            return nodes.Traverse(strategy, cancellationToken);
        }

        /// <summary>
        /// Traverses the hierarchy starting from the specified nodes by using the provided child selector and traversal mode.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="nodes">The nodes to start traversal from.</param>
        /// <param name="getChildrenFunc">A function that returns the children of a node.</param>
        /// <param name="mode">The traversal mode to use.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the traversal.</param>
        /// <returns>An enumerable sequence containing the traversed nodes.</returns>
        public static IEnumerable<TNode> Traverse<TNode>(
            this IEnumerable<TNode> nodes,
            Func<TNode, IEnumerable<TNode>> getChildrenFunc,
            TraversalMode mode,
            CancellationToken cancellationToken = default)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy(getChildrenFunc);
            return nodes.Traverse(strategy, mode, cancellationToken);
        }

        /// <summary>
        /// Traverses the hierarchy starting from the specified nodes by using the provided child selector and comparer.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="nodes">The nodes to start traversal from.</param>
        /// <param name="getChildrenFunc">A function that returns the children of a node.</param>
        /// <param name="comparer">The comparer to use for sorting nodes.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the traversal.</param>
        /// <returns>An enumerable sequence containing the traversed nodes.</returns>
        public static IEnumerable<TNode> Traverse<TNode>(
            this IEnumerable<TNode> nodes,
            Func<TNode, IEnumerable<TNode>> getChildrenFunc,
            IComparer<TNode> comparer,
            CancellationToken cancellationToken = default)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy(getChildrenFunc);
            return nodes.Traverse(strategy, comparer, cancellationToken);
        }

        /// <summary>
        /// Traverses the hierarchy starting from the specified nodes by using the provided child selector and candidate selector.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="nodes">The nodes to start traversal from.</param>
        /// <param name="getChildrenFunc">A function that returns the children of a node.</param>
        /// <param name="selector">The selector to use for identifying candidate nodes.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the traversal.</param>
        /// <returns>An enumerable sequence containing the traversed nodes.</returns>
        public static IEnumerable<TNode> Traverse<TNode>(
            this IEnumerable<TNode> nodes,
            Func<TNode, IEnumerable<TNode>> getChildrenFunc,
            ICandidateSelector<TNode> selector,
            CancellationToken cancellationToken = default)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy(getChildrenFunc);
            return nodes.Traverse(strategy, selector, cancellationToken);
        }

        /// <summary>
        /// Traverses the hierarchy starting from the specified nodes by using the provided traversal strategy.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="nodes">The nodes to start traversal from.</param>
        /// <param name="strategy">The traversal strategy used to resolve child nodes.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the traversal.</param>
        /// <returns>An enumerable sequence containing the traversed nodes in depth-first order.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="strategy"/> is <see langword="null"/>.</exception>
        public static IEnumerable<TNode> Traverse<TNode>(
            this IEnumerable<TNode> nodes,
            ITraversalStrategy<TNode> strategy,
            CancellationToken cancellationToken = default)
        {
            return nodes.Traverse(strategy, TraversalMode.DepthFirst, cancellationToken);
        }

        /// <summary>
        /// Traverses the hierarchy starting from the specified nodes by using the provided traversal strategy and traversal mode.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="nodes">The nodes to start traversal from.</param>
        /// <param name="strategy">The traversal strategy used to resolve child nodes.</param>
        /// <param name="mode">The traversal mode to use.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the traversal.</param>
        /// <returns>An enumerable sequence containing the traversed nodes.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="strategy"/> is <see langword="null"/>.</exception>
        public static IEnumerable<TNode> Traverse<TNode>(
            this IEnumerable<TNode> nodes,
            ITraversalStrategy<TNode> strategy,
            TraversalMode mode,
            CancellationToken cancellationToken = default)
        {
            var selector = CandidateSelectorFactory.Instance.CreateSelector<TNode>(mode);
            return nodes.Traverse(strategy, selector, cancellationToken);
        }

        /// <summary>
        /// Traverses the hierarchy starting from the specified nodes by using the provided traversal strategy and comparer.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="nodes">The nodes to start traversal from.</param>
        /// <param name="strategy">The traversal strategy used to resolve child nodes.</param>
        /// <param name="comparer">The comparer to use for sorting nodes.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the traversal.</param>
        /// <returns>An enumerable sequence containing the traversed nodes.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="strategy"/> is <see langword="null"/>.</exception>
        public static IEnumerable<TNode> Traverse<TNode>(
            this IEnumerable<TNode> nodes,
            ITraversalStrategy<TNode> strategy,
            IComparer<TNode> comparer,
            CancellationToken cancellationToken = default)
        {
            var selector = CandidateSelectorFactory.Instance.CreateSelector(comparer);
            return nodes.Traverse(strategy, selector, cancellationToken);
        }

        /// <summary>
        /// Traverses the hierarchy starting from the specified nodes by using the provided traversal strategy and candidate selector.
        /// When invoked, the nodes will be added to the candidate selector. The selector is then used for traversal.
        /// The candidate selector is reset on each iteration in order to prevent misaligned situations.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="nodes">The nodes to start traversal from.</param>
        /// <param name="strategy">The traversal strategy used to resolve child nodes.</param>
        /// <param name="selector">The selector to use for identifying candidate nodes.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the traversal.</param>
        /// <returns>An enumerable sequence containing the traversed nodes.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="strategy"/> or <paramref name="selector"/> is <see langword="null"/>.</exception>
        public static IEnumerable<TNode> Traverse<TNode>(
            this IEnumerable<TNode> nodes,
            ITraversalStrategy<TNode> strategy,
            ICandidateSelector<TNode> selector,
            CancellationToken cancellationToken = default)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Iterator();

            IEnumerable<TNode> Iterator()
            {
                selector.Clear();

                foreach (var node in nodes)
                {
                    selector.Add(node);
                }

                foreach (var item in Traverse(strategy, selector, cancellationToken))
                {
                    yield return item;
                }
            }
        }

        #endregion

        #endregion

        #region Invoking Callbacks

        #region Sync Execution

        /// <summary>
        /// Traverses the hierarchy starting from the specified node and invokes the callback for each visited node.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to start the traversal from.</param>
        /// <param name="callback">The callback to invoke for each visited node.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the traversal.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no traversal strategy can be derived from <typeparamref name="TNode"/>.
        /// </exception>
        public static void Traverse<TNode>(
            this TNode node,
            Action<TNode> callback,
            CancellationToken cancellationToken = default)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy<TNode>();
            node.Traverse(callback, strategy, cancellationToken);
        }

        /// <summary>
        /// Traverses the hierarchy starting from the specified node using the given traversal mode and invokes the callback for each visited node.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to start the traversal from.</param>
        /// <param name="callback">The callback to invoke for each visited node.</param>
        /// <param name="mode">The traversal mode to use.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the traversal.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no traversal strategy can be derived from <typeparamref name="TNode"/>.
        /// </exception>
        public static void Traverse<TNode>(
			this TNode node,
			Action<TNode> callback,
			TraversalMode mode,
            CancellationToken cancellationToken = default)
		{
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy<TNode>();
            node.Traverse(callback, strategy, mode, cancellationToken);
        }

		/// <summary>
        /// Traverses the hierarchy starting from the specified node, ordering traversal candidates with the given comparer, and invokes the callback for each visited node.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to start the traversal from.</param>
        /// <param name="callback">The callback to invoke for each visited node.</param>
        /// <param name="comparer">The comparer to use for sorting nodes.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the traversal.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no traversal strategy can be derived from <typeparamref name="TNode"/>.
        /// </exception>
		public static void Traverse<TNode>(
			this TNode node,
			Action<TNode> callback,
			IComparer<TNode> comparer,
            CancellationToken cancellationToken = default)
		{
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy<TNode>();
            node.Traverse(callback, strategy, comparer, cancellationToken);
        }

		/// <summary>
        /// Traverses the hierarchy starting from the specified node using the given candidate selector and invokes the callback for each visited node.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to start the traversal from.</param>
        /// <param name="callback">The callback to invoke for each visited node.</param>
        /// <param name="selector">The selector to use for identifying candidate nodes.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the traversal.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no traversal strategy can be derived from <typeparamref name="TNode"/>.
        /// </exception>
		public static void Traverse<TNode>(
			this TNode node,
			Action<TNode> callback,
			ICandidateSelector<TNode> selector,
            CancellationToken cancellationToken = default)
		{
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy<TNode>();
            node.Traverse(callback, strategy, selector, cancellationToken);
		}

        /// <summary>
        /// Traverses the hierarchy starting from the specified node by using the provided child selector and invokes the callback for each visited node.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to start the traversal from.</param>
        /// <param name="getChildrenFunc">A function that returns the children of a node.</param>
        /// <param name="callback">The callback to invoke for each visited node.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the traversal.</param>
        public static void Traverse<TNode>(
            this TNode node,
            Func<TNode, IEnumerable<TNode>> getChildrenFunc,
            Action<TNode> callback,
            CancellationToken cancellationToken = default)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy(getChildrenFunc);
            node.Traverse(callback, strategy, cancellationToken);
        }

        /// <summary>
        /// Traverses the hierarchy starting from the specified node by using the provided child selector and traversal mode, and invokes the callback for each visited node.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to start the traversal from.</param>
        /// <param name="getChildrenFunc">A function that returns the children of a node.</param>
        /// <param name="callback">The callback to invoke for each visited node.</param>
        /// <param name="mode">The traversal mode to use.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the traversal.</param>
        public static void Traverse<TNode>(
            this TNode node,
            Func<TNode, IEnumerable<TNode>> getChildrenFunc,
            Action<TNode> callback,
            TraversalMode mode,
            CancellationToken cancellationToken = default)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy(getChildrenFunc);
            node.Traverse(callback, strategy, mode, cancellationToken);
        }

        /// <summary>
        /// Traverses the hierarchy starting from the specified node by using the provided child selector and comparer, and invokes the callback for each visited node.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to start the traversal from.</param>
        /// <param name="getChildrenFunc">A function that returns the children of a node.</param>
        /// <param name="callback">The callback to invoke for each visited node.</param>
        /// <param name="comparer">The comparer to use for sorting nodes.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the traversal.</param>
        public static void Traverse<TNode>(
            this TNode node,
            Func<TNode, IEnumerable<TNode>> getChildrenFunc,
            Action<TNode> callback,
            IComparer<TNode> comparer,
            CancellationToken cancellationToken = default)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy(getChildrenFunc);
            node.Traverse(callback, strategy, comparer, cancellationToken);
        }

        /// <summary>
        /// Traverses the hierarchy starting from the specified node by using the provided child selector and candidate selector, and invokes the callback for each visited node.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to start the traversal from.</param>
        /// <param name="getChildrenFunc">A function that returns the children of a node.</param>
        /// <param name="callback">The callback to invoke for each visited node.</param>
        /// <param name="selector">The selector to use for identifying candidate nodes.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the traversal.</param>
        public static void Traverse<TNode>(
            this TNode node,
            Func<TNode, IEnumerable<TNode>> getChildrenFunc,
            Action<TNode> callback,
            ICandidateSelector<TNode> selector,
            CancellationToken cancellationToken = default)
        {
            var strategy = TraversalStrategyFactory.Instance.CreateStrategy(getChildrenFunc);
            node.Traverse(callback, strategy, selector, cancellationToken);
        }

        /// <summary>
        /// Traverses the hierarchy starting from the specified node by using the provided traversal strategy and invokes the callback for each visited node.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to start the traversal from.</param>
        /// <param name="callback">The callback to invoke for each visited node.</param>
        /// <param name="strategy">The traversal strategy used to resolve the parent and children of the node.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the traversal.</param>
        /// <exception cref="ArgumentNullException"><paramref name="strategy"/> is <see langword="null"/>.</exception>
        public static void Traverse<TNode>(
            this TNode node,
            Action<TNode> callback,
            ITraversalStrategy<TNode> strategy,
            CancellationToken cancellationToken = default)
        {
            node.Traverse(callback, strategy, TraversalMode.DepthFirst, cancellationToken);
        }

        /// <summary>
        /// Traverses the hierarchy starting from the specified node by using the provided traversal strategy and traversal mode, and invokes the callback for each visited node.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to start the traversal from.</param>
        /// <param name="callback">The callback to invoke for each visited node.</param>
        /// <param name="strategy">The traversal strategy used to resolve the parent and children of the node.</param>
        /// <param name="mode">The traversal mode to use.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the traversal.</param>
        /// <exception cref="ArgumentNullException"><paramref name="strategy"/> is <see langword="null"/>.</exception>
        public static void Traverse<TNode>(
            this TNode node,
            Action<TNode> callback,
            ITraversalStrategy<TNode> strategy,
            TraversalMode mode,
            CancellationToken cancellationToken = default)
        {
            var selector = CandidateSelectorFactory.Instance.CreateSelector<TNode>(mode);
            node.Traverse(callback, strategy, selector, cancellationToken);
        }

        /// <summary>
        /// Traverses the hierarchy starting from the specified node by using the provided traversal strategy and comparer, and invokes the callback for each visited node.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to start the traversal from.</param>
        /// <param name="callback">The callback to invoke for each visited node.</param>
        /// <param name="strategy">The traversal strategy used to resolve the parent and children of the node.</param>
        /// <param name="comparer">The comparer to use for sorting nodes.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the traversal.</param>
        /// <exception cref="ArgumentNullException"><paramref name="strategy"/> is <see langword="null"/>.</exception>
        public static void Traverse<TNode>(
            this TNode node,
            Action<TNode> callback,
            ITraversalStrategy<TNode> strategy,
            IComparer<TNode> comparer,
            CancellationToken cancellationToken = default)
        {
            var selector = CandidateSelectorFactory.Instance.CreateSelector(comparer);
            node.Traverse(callback, strategy, selector, cancellationToken);
        }

        /// <summary>
        /// Traverses the hierarchy starting from the specified node by using the provided traversal strategy and candidate selector, and invokes the callback for each visited node.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to start traversal from.</param>
        /// <param name="callback">The callback to invoke for each visited node.</param>
        /// <param name="strategy">The traversal strategy used to resolve child nodes.</param>
        /// <param name="selector">The selector to use for identifying candidate nodes.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the traversal.</param>
        /// <returns>An enumerable sequence containing the traversed nodes.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="strategy"/> or <paramref name="selector"/> is <see langword="null"/>.</exception>
        public static void Traverse<TNode>(
            this TNode node,
            Action<TNode> callback,
            ITraversalStrategy<TNode> strategy,
            ICandidateSelector<TNode> selector,
            CancellationToken cancellationToken = default)
        {
            foreach(var tmpNode in node.Traverse(strategy, selector, cancellationToken))
            {
                callback.Invoke(tmpNode);
            }
        }

        #endregion

        #region Async Execution

        /// <summary>
        /// Asynchronously traverses the hierarchy starting from the specified node and invokes the callback for each visited node.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to start the traversal from.</param>
        /// <param name="callback">The callback to invoke for each visited node.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the traversal.</param>
        /// <returns>A task that represents the asynchronous traversal operation.</returns>
        public static Task TraverseAsync<TNode>(
            this TNode node,
            Action<TNode> callback,
            CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() => node.Traverse(callback, cancellationToken), cancellationToken);
        }

        /// <summary>
        /// Asynchronously traverses the hierarchy starting from the specified node using the given traversal mode and invokes the callback for each visited node.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to start the traversal from.</param>
        /// <param name="callback">The callback to invoke for each visited node.</param>
        /// <param name="mode">The traversal mode to use.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the traversal.</param>
        /// <returns>A task that represents the asynchronous traversal operation.</returns>
        public static Task TraverseAsync<TNode>(
            this TNode node,
            Action<TNode> callback,
            TraversalMode mode,
            CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() => node.Traverse(callback, mode, cancellationToken), cancellationToken);
        }

        /// <summary>
        /// Asynchronously traverses the hierarchy starting from the specified node, ordering traversal candidates with the given comparer, and invokes the callback for each visited node.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to start the traversal from.</param>
        /// <param name="callback">The callback to invoke for each visited node.</param>
        /// <param name="comparer">The comparer to use for sorting nodes.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the traversal.</param>
        /// <returns>A task that represents the asynchronous traversal operation.</returns>
        public static Task TraverseAsync<TNode>(
            this TNode node,
            Action<TNode> callback,
            IComparer<TNode> comparer,
            CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() => node.Traverse(callback, comparer, cancellationToken), cancellationToken);
        }

        /// <summary>
        /// Asynchronously traverses the hierarchy starting from the specified node using the given candidate selector and invokes the callback for each visited node.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to start the traversal from.</param>
        /// <param name="callback">The callback to invoke for each visited node.</param>
        /// <param name="selector">The selector to use for identifying candidate nodes.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the traversal.</param>
        /// <returns>A task that represents the asynchronous traversal operation.</returns>
        public static Task TraverseAsync<TNode>(
            this TNode node,
            Action<TNode> callback,
            ICandidateSelector<TNode> selector,
            CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() => node.Traverse(callback, selector, cancellationToken), cancellationToken);
        }

        /// <summary>
        /// Asynchronously traverses the hierarchy starting from the specified node by using the provided child selector and invokes the callback for each visited node.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to start the traversal from.</param>
        /// <param name="getChildrenFunc">A function that returns the children of a node.</param>
        /// <param name="callback">The callback to invoke for each visited node.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the traversal.</param>
        /// <returns>A task that represents the asynchronous traversal operation.</returns>
        public static Task TraverseAsync<TNode>(
            this TNode node,
            Func<TNode, IEnumerable<TNode>> getChildrenFunc,
            Action<TNode> callback,
            CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() => node.Traverse(getChildrenFunc, callback, cancellationToken), cancellationToken);
        }

        /// <summary>
        /// Asynchronously traverses the hierarchy starting from the specified node by using the provided child selector and traversal mode, and invokes the callback for each visited node.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to start the traversal from.</param>
        /// <param name="getChildrenFunc">A function that returns the children of a node.</param>
        /// <param name="callback">The callback to invoke for each visited node.</param>
        /// <param name="mode">The traversal mode to use.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the traversal.</param>
        /// <returns>A task that represents the asynchronous traversal operation.</returns>
        public static Task TraverseAsync<TNode>(
            this TNode node,
            Func<TNode, IEnumerable<TNode>> getChildrenFunc,
            Action<TNode> callback,
            TraversalMode mode,
            CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() => node.Traverse(getChildrenFunc, callback, mode, cancellationToken), cancellationToken);
        }

        /// <summary>
        /// Asynchronously traverses the hierarchy starting from the specified node by using the provided child selector and comparer, and invokes the callback for each visited node.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to start the traversal from.</param>
        /// <param name="getChildrenFunc">A function that returns the children of a node.</param>
        /// <param name="callback">The callback to invoke for each visited node.</param>
        /// <param name="comparer">The comparer to use for sorting nodes.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the traversal.</param>
        /// <returns>A task that represents the asynchronous traversal operation.</returns>
        public static Task TraverseAsync<TNode>(
            this TNode node,
            Func<TNode, IEnumerable<TNode>> getChildrenFunc,
            Action<TNode> callback,
            IComparer<TNode> comparer,
            CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() => node.Traverse(getChildrenFunc, callback, comparer, cancellationToken), cancellationToken);
        }

        /// <summary>
        /// Asynchronously traverses the hierarchy starting from the specified node by using the provided child selector and candidate selector, and invokes the callback for each visited node.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to start the traversal from.</param>
        /// <param name="getChildrenFunc">A function that returns the children of a node.</param>
        /// <param name="callback">The callback to invoke for each visited node.</param>
        /// <param name="selector">The selector to use for identifying candidate nodes.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the traversal.</param>
        /// <returns>A task that represents the asynchronous traversal operation.</returns>
        public static Task TraverseAsync<TNode>(
            this TNode node,
            Func<TNode, IEnumerable<TNode>> getChildrenFunc,
            Action<TNode> callback,
            ICandidateSelector<TNode> selector,
            CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() => node.Traverse(getChildrenFunc, callback, selector, cancellationToken), cancellationToken);
        }

        /// <summary>
        /// Asynchronously traverses the hierarchy starting from the specified node by using the provided traversal strategy and invokes the callback for each visited node.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to start the traversal from.</param>
        /// <param name="callback">The callback to invoke for each visited node.</param>
        /// <param name="strategy">The traversal strategy used to resolve the parent and children of the node.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the traversal.</param>
        /// <returns>A task that represents the asynchronous traversal operation.</returns>
        public static Task TraverseAsync<TNode>(
            this TNode node,
            Action<TNode> callback,
            ITraversalStrategy<TNode> strategy,
            CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() => node.Traverse(callback, strategy, cancellationToken), cancellationToken);
        }

        /// <summary>
        /// Asynchronously traverses the hierarchy starting from the specified node by using the provided traversal strategy and traversal mode, and invokes the callback for each visited node.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to start the traversal from.</param>
        /// <param name="callback">The callback to invoke for each visited node.</param>
        /// <param name="strategy">The traversal strategy used to resolve the parent and children of the node.</param>
        /// <param name="mode">The traversal mode to use.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the traversal.</param>
        /// <returns>A task that represents the asynchronous traversal operation.</returns>
        public static Task TraverseAsync<TNode>(
            this TNode node,
            Action<TNode> callback,
            ITraversalStrategy<TNode> strategy,
            TraversalMode mode,
            CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() => node.Traverse(callback, strategy, mode, cancellationToken), cancellationToken);
        }

        /// <summary>
        /// Asynchronously traverses the hierarchy starting from the specified node by using the provided traversal strategy and comparer, and invokes the callback for each visited node.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to start the traversal from.</param>
        /// <param name="callback">The callback to invoke for each visited node.</param>
        /// <param name="strategy">The traversal strategy used to resolve the parent and children of the node.</param>
        /// <param name="comparer">The comparer to use for sorting nodes.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the traversal.</param>
        /// <returns>A task that represents the asynchronous traversal operation.</returns>
        public static Task TraverseAsync<TNode>(
            this TNode node,
            Action<TNode> callback,
            ITraversalStrategy<TNode> strategy,
            IComparer<TNode> comparer,
            CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() => node.Traverse(callback, strategy, comparer, cancellationToken), cancellationToken);
        }

        /// <summary>
        /// Asynchronously traverses the hierarchy starting from the specified node by using the provided traversal strategy and candidate selector, and invokes the callback for each visited node.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node to start the traversal from.</param>
        /// <param name="callback">The callback to invoke for each visited node.</param>
        /// <param name="strategy">The traversal strategy used to resolve the parent and children of the node.</param>
        /// <param name="selector">The selector to use for identifying candidate nodes.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the traversal.</param>
        /// <returns>A task that represents the asynchronous traversal operation.</returns>
        public static Task TraverseAsync<TNode>(
            this TNode node,
            Action<TNode> callback,
            ITraversalStrategy<TNode> strategy,
            ICandidateSelector<TNode> selector,
            CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() => node.Traverse(callback, strategy, selector, cancellationToken), cancellationToken);
        }

        #endregion

        #endregion

        /// <summary>
        /// Traverses nodes by using the provided traversal strategy and candidate selector.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="strategy">The traversal strategy used to resolve child nodes.</param>
        /// <param name="selector">The candidate selector that controls traversal order.</param>
        /// <param name="cancellationToken">A token that can be used to cancel traversal.</param>
        /// <returns>An enumerable sequence containing the traversed nodes.</returns>
        private static IEnumerable<TNode> Traverse<TNode>(
            ITraversalStrategy<TNode> strategy,
            ICandidateSelector<TNode> selector,
            CancellationToken cancellationToken = default)
        {
            var traversalService = TraversalServiceFactory.Instance.CreateService<TNode>();
            return traversalService.Traverse(strategy, selector, cancellationToken);
        }

        #endregion
    }
}
