﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;
using Bertiooo.Traversal.Selectors;
using Bertiooo.Traversal.Traverser;

namespace Bertiooo.Traversal
{
    public static class TraversalConvertibleExtensions
    {
        #region Node Analysis

        public static bool IsRoot<TNode>(this TNode node, Func<TNode, TNode> selectParent)
            where TNode : ITraversalConvertible
        {
            return node.AsParentProvider(selectParent).IsRoot();
        }

        public static bool IsInnerNode<TNode>(this TNode node, Func<TNode, IEnumerable<TNode>> selectChildren)
            where TNode : class, ITraversalConvertible
        {
            return node.AsChildrenProvider(selectChildren).IsInnerNode();
        }

        public static bool IsLeaf<TNode>(this TNode node, Func<TNode, IEnumerable<TNode>> selectChildren)
            where TNode : class, ITraversalConvertible
        {
            return node.AsChildrenProvider(selectChildren).IsLeaf();
        }

        public static bool HasParent<TNode>(this TNode node, Func<TNode, TNode> selectParent)
            where TNode : ITraversalConvertible
        {
            return node.AsParentProvider(selectParent).HasParent();
        }

        public static bool HasChildren<TNode>(this TNode node, Func<TNode, IEnumerable<TNode>> selectChildren)
            where TNode : class, ITraversalConvertible
        {
            return node.AsChildrenProvider(selectChildren).HasChildren();
        }

        public static bool HasSiblings<TNode>(this TNode node, Func<TNode, TNode> selectParent, Func<TNode, IEnumerable<TNode>> selectChildren)
            where TNode : ITraversalConvertible
        {
            return node.AsTraversable(selectParent, selectChildren).HasSiblings();
        }

        /// <inheritdoc cref="TraversableExtensions.GetLevel{TNode}(TNode)"/>
        public static int GetLevel<TNode>(this TNode node, Func<TNode, TNode> selectParent)
            where TNode : ITraversalConvertible
        {
            return node.AsParentProvider(selectParent).GetLevel();
        }

        /// <inheritdoc cref="TraversableExtensions.GetMaxDepth{TNode}(TNode)"/>
        public static int GetMaxDepth<TNode>(this TNode node, Func<TNode, TNode> selectParent, Func<TNode, IEnumerable<TNode>> selectChildren)
            where TNode : ITraversalConvertible
        {
            return node.AsTraversable(selectParent, selectChildren).GetMaxDepth();
        }

        public static bool IsChildOf<TNode>(this TNode node, TNode other, Func<TNode, TNode> selectParent)
            where TNode : ITraversalConvertible
        {
			if (other == null)
				throw new ArgumentNullException(nameof(other));

			var otherAdapter = other.AsParentProvider(selectParent);
            return node.AsParentProvider(selectParent).IsChildOf(otherAdapter);
        }

        public static bool IsParentOf<TNode>(this TNode node, TNode other, Func<TNode, TNode> selectParent)
            where TNode : ITraversalConvertible
        {
			if (other == null)
				throw new ArgumentNullException(nameof(other));

			var otherAdapter = other.AsParentProvider(selectParent);
            return node.AsParentProvider(selectParent).IsParentOf(otherAdapter);
        }

        public static bool IsSiblingOf<TNode>(this TNode node, TNode other, Func<TNode, TNode> selectParent)
            where TNode : ITraversalConvertible
        {
			if (other == null)
				throw new ArgumentNullException(nameof(other));

			var otherAdapter = other.AsParentProvider(selectParent);
            return node.AsParentProvider(selectParent).IsSiblingOf(otherAdapter);
        }

        public static bool IsDescendantOf<TNode>(this TNode node, TNode other, Func<TNode, TNode> selectParent)
            where TNode : ITraversalConvertible
        {
			if (other == null)
				throw new ArgumentNullException(nameof(other));

			var otherAdapter = other.AsParentProvider(selectParent);
            return node.AsParentProvider(selectParent).IsDescendantOf(otherAdapter);
        }

        public static bool IsAncestorOf<TNode>(this TNode node, TNode other, Func<TNode, TNode> selectParent)
            where TNode : ITraversalConvertible
        {
			if (other == null)
				throw new ArgumentNullException(nameof(other));

			var otherAdapter = other.AsParentProvider(selectParent);
            return node.AsParentProvider(selectParent).IsAncestorOf(otherAdapter);
        }

        #endregion

        #region Related Nodes Retrieval

        public static TNode Root<TNode>(this TNode node, Func<TNode, TNode> selectParent)
            where TNode : ITraversalConvertible
        {
            return node.AsParentProvider(selectParent).Root().Instance;
        }

		public static TNode Parent<TNode>(this TNode node, Func<TNode, TNode> selectParent)
			where TNode : class, ITraversalConvertible
		{
			return node.AsParentProvider(selectParent).Parent?.Instance;
		}

		public static IEnumerable<TNode> Children<TNode>(this TNode node, Func<TNode, IEnumerable<TNode>> selectChildren)
			where TNode : class, ITraversalConvertible
		{
			return node.AsChildrenProvider(selectChildren).Children.Select(x => x.Instance);
		}

		/// <inheritdoc cref="TraversableExtensions.WithParent{TNode}(TNode)"/>
		public static IEnumerable<TNode> WithParent<TNode>(this TNode node, Func<TNode, TNode> selectParent)
            where TNode : ITraversalConvertible
        {
            return node.AsParentProvider(selectParent).WithParent().Select(x => x.Instance);
        }

        /// <inheritdoc cref="TraversableExtensions.WithChildren{TNode}(TNode)"/>
        public static IEnumerable<TNode> WithChildren<TNode>(this TNode node, Func<TNode, IEnumerable<TNode>> selectChildren)
            where TNode : class, ITraversalConvertible
        {
            return node.AsChildrenProvider(selectChildren).WithChildren().Select(x => x.Instance);
        }

        public static IEnumerable<TNode> Siblings<TNode>(this TNode node, Func<TNode, TNode> selectParent, Func<TNode, IEnumerable<TNode>> selectChildren)
            where TNode : ITraversalConvertible
        {
            return node.AsTraversable(selectParent, selectChildren).Siblings().Select(x => x.Instance);
        }

        /// <inheritdoc cref="TraversableExtensions.WithSiblings{TNode}(TNode)"/>
        public static IEnumerable<TNode> WithSiblings<TNode>(this TNode node, Func<TNode, TNode> selectParent, Func<TNode, IEnumerable<TNode>> selectChildren)
            where TNode : ITraversalConvertible
        {
            return node.AsTraversable(selectParent, selectChildren).WithSiblings().Select(x => x.Instance);
        }

        public static IEnumerable<TNode> Descendants<TNode>(this TNode node, Func<TNode, IEnumerable<TNode>> selectChildren, TraversalMode traversalMode = TraversalMode.DepthFirst)
            where TNode : class, ITraversalConvertible
        {
			return node.Children(selectChildren).Traverse(selectChildren).Use(traversalMode).GetNodes();
		}

		public static IEnumerable<TNode> Descendants<TNode>(this TNode node, Func<TNode, IEnumerable<TNode>> selectChildren, IComparer<TNode> comparer, bool ascending = false)
			where TNode : class, ITraversalConvertible
		{
			return node.Children(selectChildren).Traverse(selectChildren).Use(comparer, ascending).GetNodes();
		}

		public static IEnumerable<TNode> Descendants<TNode>(this TNode node, Func<TNode, IEnumerable<TNode>> selectChildren, ICandidateSelector<TNode> candidateSelector)
            where TNode : class, ITraversalConvertible
        {
			return node.Children(selectChildren).Traverse(selectChildren).Use(candidateSelector).GetNodes();
		}

        public static IEnumerable<TNode> WithDescendants<TNode>(this TNode node, Func<TNode, IEnumerable<TNode>> selectChildren, TraversalMode traversalMode = TraversalMode.DepthFirst)
            where TNode : class, ITraversalConvertible
        {
			return node.Traverse(selectChildren).Use(traversalMode).GetNodes();
		}

		public static IEnumerable<TNode> WithDescendants<TNode>(this TNode node, Func<TNode, IEnumerable<TNode>> selectChildren, IComparer<TNode> comparer, bool ascending = false)
			where TNode : class, ITraversalConvertible
		{
			return node.Traverse(selectChildren).Use(comparer, ascending).GetNodes();
		}

		public static IEnumerable<TNode> WithDescendants<TNode>(this TNode node, Func<TNode, IEnumerable<TNode>> selectChildren, ICandidateSelector<TNode> candidateSelector)
            where TNode : class, ITraversalConvertible
        {
			return node.Traverse(selectChildren).Use(candidateSelector).GetNodes();
		}

        public static IEnumerable<TNode> Ancestors<TNode>(this TNode node, Func<TNode, TNode> selectParent)
            where TNode : ITraversalConvertible
        {
			return node.AsParentProvider(selectParent).Ancestors().Select(x => x.Instance);
		}

        public static IEnumerable<TNode> WithAncestors<TNode>(this TNode node, Func<TNode, TNode> selectParent)
            where TNode : ITraversalConvertible
        {
            return node.AsParentProvider(selectParent).WithAncestors().Select(x => x.Instance);
        }

		/// <inheritdoc cref="TraversableExtensions.Leaves{TNode}(TNode)"/>
		public static IEnumerable<TNode> Leaves<TNode>(this TNode node, Func<TNode, IEnumerable<TNode>> selectChildren)
			where TNode : class, ITraversalConvertible
		{
			return node.AsChildrenProvider(selectChildren).Leaves().Select(x => x.Instance);
		}

		/// <inheritdoc cref="TraversableExtensions.InnerNodes{TNode}(TNode)"/>
		public static IEnumerable<TNode> InnerNodes<TNode>(this TNode node, Func<TNode, IEnumerable<TNode>> selectChildren)
			where TNode : class, ITraversalConvertible
		{
			return node.AsChildrenProvider(selectChildren).InnerNodes().Select(x => x.Instance);
		}

		public static IEnumerable<TNode> WithInnerNodes<TNode>(this TNode node, Func<TNode, IEnumerable<TNode>> selectChildren)
			where TNode : class, ITraversalConvertible
		{
			return node.AsChildrenProvider(selectChildren).WithInnerNodes().Select(x => x.Instance);
		}

		#endregion

		#region Traverse Methods
        
		/// <inheritdoc cref="TraversableExtensions.Traverse{TNode}(TNode)"/>
		public static ITraverser<TNode> Traverse<TNode>(
            this TNode node,
            Func<TNode, IEnumerable<TNode>> selectChildren)
            where TNode : class, ITraversalConvertible
        {
            return new TraversalConvertibleTraverser<TNode>(node, selectChildren);
        }

		/// <inheritdoc cref="TraversableExtensions.Traverse{TNode}(IEnumerable{TNode})"/>
		public static ITraverser<TNode> Traverse<TNode>(
			this IEnumerable<TNode> nodes,
			Func<TNode, IEnumerable<TNode>> selectChildren)
			where TNode : class, ITraversalConvertible
		{
			return new TraversalConvertibleTraverser<TNode>(nodes, selectChildren);
		}

		public static void Traverse<TNode>(
            this TNode node,
            Action<TNode> callback,
            Func<TNode, IEnumerable<TNode>> selectChildren,
            TraversalMode traversalMode = TraversalMode.DepthFirst)
            where TNode : class, ITraversalConvertible
        {
			node.Traverse(selectChildren)
				.Use(traversalMode)
				.WithAction(callback)
				.Execute();
		}

		public static void Traverse<TNode>(
			this TNode node,
			Action<TNode> callback,
			Func<TNode, IEnumerable<TNode>> selectChildren,
			IComparer<TNode> comparer,
            bool ascending = false)
			where TNode : class, ITraversalConvertible
		{
			node.Traverse(selectChildren)
				.Use(comparer, ascending)
				.WithAction(callback)
				.Execute();
		}

		public static void Traverse<TNode>(
            this TNode node,
            Action<TNode> callback,
            Func<TNode, IEnumerable<TNode>> selectChildren,
            ICandidateSelector<TNode> candidateSelector)
            where TNode : class, ITraversalConvertible
        {
			node.Traverse(selectChildren)
				.Use(candidateSelector)
				.WithAction(callback)
				.Execute();
		}

        public static Task TraverseAsync<TNode>(
            this TNode node,
            Action<TNode> callback,
            Func<TNode, IEnumerable<TNode>> selectChildren,
            TraversalMode traversalMode = TraversalMode.DepthFirst,
            CancellationToken cancellationToken = default)
            where TNode : class, ITraversalConvertible
        {
			return Task.Factory.StartNew(() => node.Traverse(callback, selectChildren, traversalMode), cancellationToken);
		}

		public static Task TraverseAsync<TNode>(
			this TNode node,
			Action<TNode> callback,
			Func<TNode, IEnumerable<TNode>> selectChildren,
			IComparer<TNode> comparer,
            bool ascending = false,
			CancellationToken cancellationToken = default)
			where TNode : class, ITraversalConvertible
		{
			return Task.Factory.StartNew(() => node.Traverse(callback, selectChildren, comparer, ascending), cancellationToken);
		}

		public static Task TraverseAsync<TNode>(
            this TNode node,
            Action<TNode> callback,
            Func<TNode, IEnumerable<TNode>> selectChildren,
            ICandidateSelector<TNode> candidateSelector,
            CancellationToken cancellationToken = default)
            where TNode : class, ITraversalConvertible
        {
			return Task.Factory.StartNew(() => node.Traverse(callback, selectChildren, candidateSelector), cancellationToken);
		}

        #endregion

        #region Conversion Methods

        public static AbstractTraversableAdapter<TConvertible> AsTraversable<TConvertible>(
            this TConvertible convertible,
            Func<TConvertible, TConvertible> getParentFunc,
            Func<TConvertible, IEnumerable<TConvertible>> getChildrenFunc)
            where TConvertible : ITraversalConvertible
		{
            return new DefaultTraversableAdapter<TConvertible>(convertible, getParentFunc, getChildrenFunc);
        }

		public static AbstractTraversableAdapter<TConvertible> AsParentProvider<TConvertible>(
			this TConvertible convertible,
			Func<TConvertible, TConvertible> getParentFunc)
			where TConvertible : ITraversalConvertible
		{
			return new DefaultTraversableAdapter<TConvertible>(convertible, getParentFunc, x => Enumerable.Empty<TConvertible>());
		}

		public static AbstractTraversableAdapter<TConvertible> AsChildrenProvider<TConvertible>(
			this TConvertible convertible,
			Func<TConvertible, IEnumerable<TConvertible>> selectChildren)
			where TConvertible : class, ITraversalConvertible
		{
			return new DefaultTraversableAdapter<TConvertible>(convertible, x => null, selectChildren);
		}

		#endregion
	}
}
