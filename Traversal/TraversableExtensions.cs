using Bertiooo.Traversal.Selectors;
using Bertiooo.Traversal.Traverser;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bertiooo.Traversal
{
	public static class TraversableExtensions
	{
		#region Node Analysis

		public static bool IsRoot<TNode>(this TNode node)
			where TNode : IParentProvider<TNode>
		{
			return node.HasParent() == false;
		}

		public static bool IsInnerNode<TNode>(this TNode node)
			where TNode : IChildrenProvider<TNode>
		{
			return node.HasChildren();
		}

		public static bool IsLeaf<TNode>(this TNode node)
			where TNode : IChildrenProvider<TNode>
		{
			return node.HasChildren() == false;
		}

		public static bool HasParent<TNode>(this TNode node)
			where TNode : IParentProvider<TNode>
		{
			return node.Parent != null;
		}

		public static bool HasChildren<TNode>(this TNode node)
			where TNode : IChildrenProvider<TNode>
		{
			return node.Children != null && node.Children.Any();
		}

		public static bool HasSiblings<TNode>(this TNode node)
			where TNode : ITraversable<TNode>
		{
			if (node.Parent == null)
				return false;

			return node.Parent.Children.Where(x => Equals(x, node) == false).Any();
		}

		/// <summary>
		/// Returns the zero-based level number. 
		/// E.g. the root node level would be 0, its children would have level 1, its grandchildren level 2, etc.
		/// </summary>
		public static int GetLevel<TNode>(this TNode node)
			where TNode : IParentProvider<TNode>
		{
			return node.Ancestors().Count();
		}

		/// <summary>
		/// Returns the maximum depth of the node. If the node has no children at all, the maximum depth would be 0.
		/// With children it would be 1, with grandchildren 2 and so on.
		/// </summary>
		/// <remarks>
		/// Might be computational expensive, since it traverses all descendants of the node
		/// in order to figure out the maximum depth.
		/// </remarks>
		public static int GetMaxDepth<TNode>(this TNode node)
			where TNode : class, ITraversable<TNode>
		{
			var maxLevel = 0;
			node.Traverse(x => maxLevel = Math.Max(maxLevel, x.GetLevel()));

			return maxLevel - node.GetLevel();
		}

		public static bool IsChildOf<TNode>(this TNode node, TNode other)
			where TNode : IParentProvider<TNode>
		{
			if (node.Parent == null)
				return false;

			return node.Parent.Equals(other);
		}

		public static bool IsParentOf<TNode>(this TNode node, TNode other)
			where TNode : IParentProvider<TNode>
		{
			if (other == null)
				throw new ArgumentNullException(nameof(other));

			if (other.Parent == null)
				return false;

			return other.Parent.Equals(node);
		}

		public static bool IsSiblingOf<TNode>(this TNode node, TNode other)
			where TNode : IParentProvider<TNode>
		{
			if (other == null)
				throw new ArgumentNullException(nameof(other));

			if (node.Parent == null || other.Parent == null)
				return false;

			return node.Parent.Equals(other.Parent);
		}

		public static bool IsDescendantOf<TNode>(this TNode node, TNode other)
			where TNode : IParentProvider<TNode>
		{
			return node.Ancestors().Any(x => Equals(other, x));
		}

		public static bool IsAncestorOf<TNode>(this TNode node, TNode other)
			where TNode : IParentProvider<TNode>
		{
			return other.Ancestors().Any(x => Equals(node, x));
		}

		#endregion

		#region Related Nodes Retrieval

		public static TNode GetRoot<TNode>(this TNode node)
			where TNode : IParentProvider<TNode>
		{
			var tmpNode = node;

			while (tmpNode.Parent != null)
			{
				tmpNode = tmpNode.Parent;
			}

			return tmpNode;
		}

		/// <remarks>
		/// If the node itself has no parent, thus is the root node,
		/// the method only returns an enumerable with the node itself.
		/// </remarks>
		public static IEnumerable<TNode> WithParent<TNode>(this TNode node)
			where TNode : IParentProvider<TNode>
		{
			yield return node;

			if (node.Parent != null)
				yield return node.Parent;
		}

		/// <remarks>
		/// If the node has no children,
		/// the method only returns an enumerable with the node itself.
		/// </remarks>
		public static IEnumerable<TNode> WithChildren<TNode>(this TNode node)
			where TNode : IChildrenProvider<TNode>
		{
			yield return node;

			foreach(var child in node.Children)
				yield return child;
		}

		public static IEnumerable<TNode> Siblings<TNode>(this TNode node)
			where TNode : ITraversable<TNode>
		{
			if (node.Parent == null)
				return Enumerable.Empty<TNode>();

			return node.Parent.Children.Where(x => Equals(x, node) == false);
		}

		/// <remarks>
		/// If the node has no siblings,
		/// the method only returns an enumerable with the node itself.
		/// In contrast of calling the Children property of the node's parent,
		/// by calling this method the order will vary, i.e. the node itself is the first element
		/// followed by the siblings.
		/// </remarks>
		public static IEnumerable<TNode> WithSiblings<TNode>(this TNode node)
			where TNode : ITraversable<TNode>
		{
			yield return node;				
			
			foreach (var sibling in node.Siblings())
				yield return sibling;
		}

		public static IEnumerable<TNode> Descendants<TNode>(this TNode node, TraversalMode traversalMode = TraversalMode.DepthFirst)
			where TNode : class, IChildrenProvider<TNode>
		{
			return node.Traverse().Use(traversalMode).Exclude(node).GetNodes();
		}

		/// <param name="comparer">Define which node to prefer over the other in the order of traversal.</param>
		public static IEnumerable<TNode> Descendants<TNode>(this TNode node, IComparer<TNode> comparer, bool ascending = false)
			where TNode : class, IChildrenProvider<TNode>
		{
			return node.Traverse().Use(comparer, ascending).Exclude(node).GetNodes();
		}

		public static IEnumerable<TNode> Descendants<TNode>(this TNode node, ICandidateSelector<TNode> candidateSelector)
			where TNode : class, IChildrenProvider<TNode>
		{
			return node.Traverse().Use(candidateSelector).Exclude(node).GetNodes();
		}

		public static IEnumerable<TNode> WithDescendants<TNode>(this TNode node, TraversalMode traversalMode = TraversalMode.DepthFirst)
			where TNode : class, IChildrenProvider<TNode>
		{
			return node.Traverse().Use(traversalMode).GetNodes();
		}

		/// <param name="comparer">Define which node to prefer over the other in the order of traversal.</param>
		public static IEnumerable<TNode> WithDescendants<TNode>(this TNode node, IComparer<TNode> comparer, bool ascending = false)
			where TNode : class, IChildrenProvider<TNode>
		{
			return node.Traverse().Use(comparer, ascending).GetNodes();
		}

		public static IEnumerable<TNode> WithDescendants<TNode>(this TNode node, ICandidateSelector<TNode> candidateSelector)
			where TNode : class, IChildrenProvider<TNode>
		{
			return node.Traverse().Use(candidateSelector).GetNodes();
		}

		public static IEnumerable<TNode> Ancestors<TNode>(this TNode node)
			where TNode : IParentProvider<TNode>
		{
			var tmpNode = node.Parent;

			while (tmpNode != null)
			{
				yield return tmpNode;
				tmpNode = tmpNode.Parent;
			}
		}

		public static IEnumerable<TNode> WithAncestors<TNode>(this TNode node)
			where TNode : IParentProvider<TNode>
		{
			yield return node;

			foreach (var ancestor in node.Ancestors())
				yield return ancestor;
		}

		/// <summary>
		/// Returns all descendants that are a leaf.
		/// If the node itself is a leaf, than the node is returned.
		/// </summary>
		public static IEnumerable<TNode> Leaves<TNode>(this TNode node)
			where TNode : class, IChildrenProvider<TNode>
		{
			return node.WithDescendants().Where(x => x.IsLeaf());
		}

		/// <summary>
		/// Returns all descendants that are inner nodes.
		/// The node itself is not included.
		/// </summary>
		public static IEnumerable<TNode> InnerNodes<TNode>(this TNode node)
			where TNode : class, IChildrenProvider<TNode>
		{
			return node.Descendants().Where(x => x.IsInnerNode());
		}

		public static IEnumerable<TNode> WithInnerNodes<TNode>(this TNode node)
			where TNode : class, IChildrenProvider<TNode>
		{
			return node.WithDescendants().Where(x => x.IsInnerNode());
		}

		#endregion

		#region Traverse Methods

		/// <summary>
		/// Initiates a new traversal process that can be configured via fluent API.
		/// </summary>
		public static ITraverser<TNode> Traverse<TNode>(
			this TNode node)
			where TNode : class, IChildrenProvider<TNode>
		{
			return new TraversableTraverser<TNode>(node);
		}

		public static void Traverse<TNode>(
			this TNode node,
			Action<TNode> callback,
			TraversalMode traversalMode = TraversalMode.DepthFirst)
			where TNode : class, IChildrenProvider<TNode>
		{
			node.Traverse()
				.Use(traversalMode)
				.WithAction(callback)
				.Execute();
		}

		public static void Traverse<TNode>(
			this TNode node,
			Action<TNode> callback,
			IComparer<TNode> comparer,
			bool ascending = false)
			where TNode : class, IChildrenProvider<TNode>
		{
			node.Traverse()
				.Use(comparer, ascending)
				.WithAction(callback)
				.Execute();
		}

		public static void Traverse<TNode>(
			this TNode node,
			Action<TNode> callback,
			ICandidateSelector<TNode> candidateSelector)
			where TNode : class, IChildrenProvider<TNode>
		{
			node.Traverse()
				.Use(candidateSelector)
				.WithAction(callback)
				.Execute();
		}

		public static Task TraverseAsync<TNode>(
			this TNode node,
			Action<TNode> callback,
			TraversalMode traversalMode = TraversalMode.DepthFirst,
			CancellationToken cancellationToken = default)
			where TNode : class, IChildrenProvider<TNode>
		{
			return Task.Factory.StartNew(() => node.Traverse(callback, traversalMode), cancellationToken);
		}

		public static Task TraverseAsync<TNode>(
			this TNode node,
			Action<TNode> callback,
			IComparer<TNode> comparer,
			bool ascending = false,
			CancellationToken cancellationToken = default)
			where TNode : class, IChildrenProvider<TNode>
		{
			return Task.Factory.StartNew(() => node.Traverse(callback, comparer, ascending), cancellationToken);
		}

		public static Task TraverseAsync<TNode>(
			this TNode node,
			Action<TNode> callback,
			ICandidateSelector<TNode> candidateSelector,
			CancellationToken cancellationToken = default)
			where TNode : class, IChildrenProvider<TNode>
		{
			return Task.Factory.StartNew(() => node.Traverse(callback, candidateSelector), cancellationToken);
		}

		#endregion
	}
}
