﻿using Bertiooo.Traversal.Selectors;
using Bertiooo.Traversal.Traverser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bertiooo.Traversal
{
	public static class GenericTraversalConvertibleExtensions
	{
		#region Node Analysis

		public static bool IsRoot<TAdapter, TConvertible>(this ITraversalConvertible<TAdapter, TConvertible> convertible)
			where TAdapter : ITraversable<TAdapter>, IInstanceProvider<TConvertible>
		{
			return convertible.AsTraversable().IsRoot();
		}

		public static bool IsInnerNode<TAdapter, TConvertible>(this ITraversalConvertible<TAdapter, TConvertible> convertible)
			where TAdapter : ITraversable<TAdapter>, IInstanceProvider<TConvertible>
		{
			return convertible.AsTraversable().IsInnerNode();
		}

		public static bool IsLeaf<TAdapter, TConvertible>(this ITraversalConvertible<TAdapter, TConvertible> convertible)
			where TAdapter : ITraversable<TAdapter>, IInstanceProvider<TConvertible>
		{
			return convertible.AsTraversable().IsLeaf();
		}

		public static bool HasParent<TAdapter, TConvertible>(this ITraversalConvertible<TAdapter, TConvertible> convertible)
			where TAdapter : ITraversable<TAdapter>, IInstanceProvider<TConvertible>
		{
			return convertible.AsTraversable().HasParent();
		}

		public static bool HasChildren<TAdapter, TConvertible>(this ITraversalConvertible<TAdapter, TConvertible> convertible)
			where TAdapter : ITraversable<TAdapter>, IInstanceProvider<TConvertible>
		{
			return convertible.AsTraversable().HasChildren();
		}

		public static bool HasSiblings<TAdapter, TConvertible>(this ITraversalConvertible<TAdapter, TConvertible> convertible)
			where TAdapter : ITraversable<TAdapter>, IInstanceProvider<TConvertible>
		{
			return convertible.AsTraversable().HasSiblings();
		}

		/// <inheritdoc cref="TraversableExtensions.GetLevel{TNode}(TNode)"/>
		public static int GetLevel<TAdapter, TConvertible>(this ITraversalConvertible<TAdapter, TConvertible> convertible)
			where TAdapter : ITraversable<TAdapter>, IInstanceProvider<TConvertible>
		{
			return convertible.AsTraversable().GetLevel();
		}

		/// <inheritdoc cref="TraversableExtensions.GetMaxDepth{TNode}(TNode)"/>
		public static int GetMaxDepth<TAdapter, TConvertible>(this ITraversalConvertible<TAdapter, TConvertible> convertible)
			where TAdapter : ITraversable<TAdapter>, IInstanceProvider<TConvertible>
		{
			return convertible.AsTraversable().GetMaxDepth();
		}

		public static bool IsChildOf<TAdapter, TConvertible>(this ITraversalConvertible<TAdapter, TConvertible> convertible, ITraversalConvertible<TAdapter, TConvertible> other)
			where TAdapter : ITraversable<TAdapter>, IInstanceProvider<TConvertible>
		{
			if (other == null)
				throw new ArgumentNullException(nameof(other));

			return convertible.AsTraversable().IsChildOf(other.AsTraversable());
		}

		public static bool IsParentOf<TAdapter, TConvertible>(this ITraversalConvertible<TAdapter, TConvertible> convertible, ITraversalConvertible<TAdapter, TConvertible> other)
			where TAdapter : ITraversable<TAdapter>, IInstanceProvider<TConvertible>
		{
			if (other == null)
				throw new ArgumentNullException(nameof(other));

			return convertible.AsTraversable().IsParentOf(other.AsTraversable());
		}

		public static bool IsSiblingOf<TAdapter, TConvertible>(this ITraversalConvertible<TAdapter, TConvertible> convertible, ITraversalConvertible<TAdapter, TConvertible> other)
			where TAdapter : ITraversable<TAdapter>, IInstanceProvider<TConvertible>
		{
			if (other == null)
				throw new ArgumentNullException(nameof(other));

			return convertible.AsTraversable().IsSiblingOf(other.AsTraversable());
		}

		public static bool IsDescendantOf<TAdapter, TConvertible>(this ITraversalConvertible<TAdapter, TConvertible> convertible, ITraversalConvertible<TAdapter, TConvertible> other)
			where TAdapter : ITraversable<TAdapter>, IInstanceProvider<TConvertible>
		{
			if (other == null)
				throw new ArgumentNullException(nameof(other));

			return convertible.AsTraversable().IsDescendantOf(other.AsTraversable());
		}

		public static bool IsAncestorOf<TAdapter, TConvertible>(this ITraversalConvertible<TAdapter, TConvertible> convertible, ITraversalConvertible<TAdapter, TConvertible> other)
			where TAdapter : ITraversable<TAdapter>, IInstanceProvider<TConvertible>
		{
			if (other == null)
				throw new ArgumentNullException(nameof(other));

			return convertible.AsTraversable().IsAncestorOf(other.AsTraversable());
		}

		#endregion

		#region Related Nodes Retrieval

		public static TConvertible Root<TAdapter, TConvertible>(this ITraversalConvertible<TAdapter, TConvertible> convertible)
			where TAdapter : ITraversable<TAdapter>, IInstanceProvider<TConvertible>
		{
			return convertible.AsTraversable().Root().Instance;
		}

		public static TConvertible Parent<TAdapter, TConvertible>(this ITraversalConvertible<TAdapter, TConvertible> convertible)
			where TAdapter : ITraversable<TAdapter>, IInstanceProvider<TConvertible>
			where TConvertible : class
		{
			return convertible.AsTraversable().Parent?.Instance;
		}

		public static IEnumerable<TConvertible> Children<TAdapter, TConvertible>(this ITraversalConvertible<TAdapter, TConvertible> convertible)
			where TAdapter : ITraversable<TAdapter>, IInstanceProvider<TConvertible>
		{
			return convertible.AsTraversable().Children.Select(x => x.Instance);
		}

		/// <inheritdoc cref="TraversableExtensions.WithParent{TNode}(TNode)"/>
		public static IEnumerable<TConvertible> WithParent<TAdapter, TConvertible>(this ITraversalConvertible<TAdapter, TConvertible> convertible)
			where TAdapter : ITraversable<TAdapter>, IInstanceProvider<TConvertible>
		{
			return convertible.AsTraversable().WithParent().Select(x => x.Instance);
		}

		/// <inheritdoc cref="TraversableExtensions.WithChildren{TNode}(TNode)"/>
		public static IEnumerable<TConvertible> WithChildren<TAdapter, TConvertible>(this ITraversalConvertible<TAdapter, TConvertible> convertible)
			where TAdapter : ITraversable<TAdapter>, IInstanceProvider<TConvertible>
		{
			return convertible.AsTraversable().WithChildren().Select(x => x.Instance);
		}

		public static IEnumerable<TConvertible> Siblings<TAdapter, TConvertible>(this ITraversalConvertible<TAdapter, TConvertible> convertible)
			where TAdapter : ITraversable<TAdapter>, IInstanceProvider<TConvertible>
		{
			return convertible.AsTraversable().Siblings().Select(x => x.Instance);
		}

		/// <inheritdoc cref="TraversableExtensions.WithSiblings{TNode}(TNode)"/>
		public static IEnumerable<TConvertible> WithSiblings<TAdapter, TConvertible>(this ITraversalConvertible<TAdapter, TConvertible> convertible)
			where TAdapter : ITraversable<TAdapter>, IInstanceProvider<TConvertible>
		{
			return convertible.AsTraversable().WithSiblings().Select(x => x.Instance);
		}

		public static IEnumerable<TConvertible> Descendants<TAdapter, TConvertible>(this ITraversalConvertible<TAdapter, TConvertible> convertible, TraversalMode traversalMode = TraversalMode.DepthFirst)
			where TAdapter : ITraversable<TAdapter>, IInstanceProvider<TConvertible>
			where TConvertible : ITraversalConvertible<TAdapter, TConvertible>
		{
			return convertible.Children().Traverse<TAdapter, TConvertible>().Use(traversalMode).GetNodes();
		}

		public static IEnumerable<TConvertible> Descendants<TAdapter, TConvertible>(this ITraversalConvertible<TAdapter, TConvertible> convertible, IComparer<TConvertible> comparer, bool ascending = false)
			where TAdapter : ITraversable<TAdapter>, IInstanceProvider<TConvertible>
			where TConvertible : ITraversalConvertible<TAdapter, TConvertible>
		{
			return convertible.Children().Traverse<TAdapter, TConvertible>().Use(comparer, ascending).GetNodes();
		}

		public static IEnumerable<TConvertible> Descendants<TAdapter, TConvertible>(this ITraversalConvertible<TAdapter, TConvertible> convertible, ICandidateSelector<TConvertible> candidateSelector)
			where TAdapter : ITraversable<TAdapter>, IInstanceProvider<TConvertible>
			where TConvertible : ITraversalConvertible<TAdapter, TConvertible>
		{
			return convertible.Children().Traverse<TAdapter, TConvertible>().Use(candidateSelector).GetNodes();
		}

		public static IEnumerable<TConvertible> WithDescendants<TAdapter, TConvertible>(this ITraversalConvertible<TAdapter, TConvertible> convertible, TraversalMode traversalMode = TraversalMode.DepthFirst)
			where TAdapter : ITraversable<TAdapter>, IInstanceProvider<TConvertible>
			where TConvertible : ITraversalConvertible<TAdapter, TConvertible>
		{
			return convertible.Traverse().Use(traversalMode).GetNodes();
		}

		public static IEnumerable<TConvertible> WithDescendants<TAdapter, TConvertible>(this ITraversalConvertible<TAdapter, TConvertible> convertible, IComparer<TConvertible> comparer, bool ascending = false)
			where TAdapter : ITraversable<TAdapter>, IInstanceProvider<TConvertible>
			where TConvertible : ITraversalConvertible<TAdapter, TConvertible>
		{
			return convertible.Traverse().Use(comparer, ascending).GetNodes();
		}

		public static IEnumerable<TConvertible> WithDescendants<TAdapter, TConvertible>(this ITraversalConvertible<TAdapter, TConvertible> convertible, ICandidateSelector<TConvertible> candidateSelector)
			where TAdapter : ITraversable<TAdapter>, IInstanceProvider<TConvertible>
			where TConvertible : ITraversalConvertible<TAdapter, TConvertible>
		{
			return convertible.Traverse().Use(candidateSelector).GetNodes();
		}

		public static IEnumerable<TConvertible> Ancestors<TAdapter, TConvertible>(this ITraversalConvertible<TAdapter, TConvertible> convertible)
			where TAdapter : ITraversable<TAdapter>, IInstanceProvider<TConvertible>
		{
			return convertible.AsTraversable().Ancestors().Select(x => x.Instance);
		}

		public static IEnumerable<TConvertible> WithAncestors<TAdapter, TConvertible>(this ITraversalConvertible<TAdapter, TConvertible> convertible)
			where TAdapter : ITraversable<TAdapter>, IInstanceProvider<TConvertible>
		{
			return convertible.AsTraversable().WithAncestors().Select(x => x.Instance);
		}

		/// <inheritdoc cref="TraversableExtensions.Leaves{TNode}(TNode)"/>
		public static IEnumerable<TConvertible> Leaves<TAdapter, TConvertible>(this ITraversalConvertible<TAdapter, TConvertible> convertible)
			where TAdapter : ITraversable<TAdapter>, IInstanceProvider<TConvertible>
		{
			return convertible.AsTraversable().Leaves().Select(x => x.Instance);
		}

		/// <inheritdoc cref="TraversableExtensions.InnerNodes{TNode}(TNode)"/>
		public static IEnumerable<TConvertible> InnerNodes<TAdapter, TConvertible>(this ITraversalConvertible<TAdapter, TConvertible> convertible)
			where TAdapter : ITraversable<TAdapter>, IInstanceProvider<TConvertible>
		{
			return convertible.AsTraversable().InnerNodes().Select(x => x.Instance);
		}

		public static IEnumerable<TConvertible> WithInnerNodes<TAdapter, TConvertible>(this ITraversalConvertible<TAdapter, TConvertible> convertible)
			where TAdapter : ITraversable<TAdapter>, IInstanceProvider<TConvertible>
		{
			return convertible.AsTraversable().WithInnerNodes().Select(x => x.Instance);
		}

		#endregion

		#region Traverse Methods

		/// <inheritdoc cref="TraversableExtensions.Traverse{TNode}(TNode)"/>
		public static ITraverser<TConvertible> Traverse<TAdapter, TConvertible>(
			this ITraversalConvertible<TAdapter, TConvertible> convertible)
			where TAdapter : ITraversable<TAdapter>, IInstanceProvider<TConvertible>
			where TConvertible : ITraversalConvertible<TAdapter, TConvertible>
		{
			var adapter = convertible.AsTraversable();
			return new GenericTraversalConvertibleTraverser<TAdapter, TConvertible>(adapter);
		}

		/// <inheritdoc cref="TraversableExtensions.Traverse{TNode}(IEnumerable{TNode})"/>
		public static ITraverser<TConvertible> Traverse<TAdapter, TConvertible>(
			this IEnumerable<TConvertible> convertibles)
			where TAdapter : ITraversable<TAdapter>, IInstanceProvider<TConvertible>
			where TConvertible : ITraversalConvertible<TAdapter, TConvertible>
		{
			var adapters = convertibles.Select(x => x.AsTraversable());
			return new GenericTraversalConvertibleTraverser<TAdapter, TConvertible>(adapters);
		}

		public static void Traverse<TAdapter, TConvertible>(
			this ITraversalConvertible<TAdapter, TConvertible> convertible,
			Action<TConvertible> callback,
			TraversalMode traversalMode = TraversalMode.DepthFirst)
			where TAdapter : ITraversable<TAdapter>, IInstanceProvider<TConvertible>
			where TConvertible : ITraversalConvertible<TAdapter, TConvertible>
		{
			convertible.Traverse()
				.Use(traversalMode)
				.WithAction(callback)
				.Execute();
		}

		public static void Traverse<TAdapter, TConvertible>(
			this ITraversalConvertible<TAdapter, TConvertible> convertible,
			Action<TConvertible> callback,
			IComparer<TConvertible> comparer,
			bool ascending = false)
			where TAdapter : ITraversable<TAdapter>, IInstanceProvider<TConvertible>
			where TConvertible : ITraversalConvertible<TAdapter, TConvertible>
		{
			convertible.Traverse()
				.Use(comparer, ascending)
				.WithAction(callback)
				.Execute();
		}

		public static void Traverse<TAdapter, TConvertible>(
			this ITraversalConvertible<TAdapter, TConvertible> convertible,
			Action<TConvertible> callback,
			ICandidateSelector<TConvertible> candidateSelector)
			where TAdapter : ITraversable<TAdapter>, IInstanceProvider<TConvertible>
			where TConvertible : ITraversalConvertible<TAdapter, TConvertible>
		{
			convertible.Traverse()
				.Use(candidateSelector)
				.WithAction(callback)
				.Execute();
		}

		public static Task TraverseAsync<TAdapter, TConvertible>(
			this ITraversalConvertible<TAdapter, TConvertible> convertible,
			Action<TConvertible> callback,
			TraversalMode traversalMode = TraversalMode.DepthFirst,
			CancellationToken cancellationToken = default)
			where TAdapter : ITraversable<TAdapter>, IInstanceProvider<TConvertible>
			where TConvertible : ITraversalConvertible<TAdapter, TConvertible>
		{
			return Task.Factory.StartNew(() => convertible.Traverse(callback, traversalMode), cancellationToken);
		}

		public static Task TraverseAsync<TAdapter, TConvertible>(
			this ITraversalConvertible<TAdapter, TConvertible> convertible,
			Action<TConvertible> callback,
			IComparer<TConvertible> comparer,
			bool ascending = false,
			CancellationToken cancellationToken = default)
			where TAdapter : ITraversable<TAdapter>, IInstanceProvider<TConvertible>
			where TConvertible : ITraversalConvertible<TAdapter, TConvertible>
		{
			return Task.Factory.StartNew(() => convertible.Traverse(callback, comparer, ascending), cancellationToken);
		}

		public static Task TraverseAsync<TAdapter, TConvertible>(
			this ITraversalConvertible<TAdapter, TConvertible> convertible,
			Action<TConvertible> callback,
			ICandidateSelector<TConvertible> candidateSelector,
			CancellationToken cancellationToken = default)
			where TAdapter : ITraversable<TAdapter>, IInstanceProvider<TConvertible>
			where TConvertible : ITraversalConvertible<TAdapter, TConvertible>
		{
			return Task.Factory.StartNew(() => convertible.Traverse(callback, candidateSelector), cancellationToken);
		}

		#endregion
	}
}
