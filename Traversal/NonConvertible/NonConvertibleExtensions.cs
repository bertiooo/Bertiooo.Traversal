using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;
using Bertiooo.Traversal.Selectors;
using Bertiooo.Traversal.Traverser;

namespace Bertiooo.Traversal.NonConvertible
{
    /// <remarks>
    /// Since these are extensions for any class, the code editor will suggest these extensions on any class,
    /// which can be sort of annoying. Putting them into a separate namespace will only make them available
    /// if they are really needed.
    /// </remarks>
    public static class NonConvertibleExtensions
    {
        #region Node Analysis

        public static bool IsRoot<TNode>(this TNode node, Func<TNode, TNode> selectParent)
            where TNode : class
        {
            return node.AsTraversable(selectParent, x => Enumerable.Empty<TNode>()).IsRoot();
        }

        public static bool IsInnerNode<TNode>(this TNode node, Func<TNode, IEnumerable<TNode>> selectChildren)
            where TNode : class
        {
            return node.AsTraversable(x => null, selectChildren).IsInnerNode();
        }

        public static bool IsLeaf<TNode>(this TNode node, Func<TNode, IEnumerable<TNode>> selectChildren)
            where TNode : class
        {
            return node.AsTraversable(x => null, selectChildren).IsLeaf();
        }

        public static bool HasParent<TNode>(this TNode node, Func<TNode, TNode> selectParent)
            where TNode : class
        {
            return node.AsTraversable(selectParent, x => Enumerable.Empty<TNode>()).HasParent();
        }

        public static bool HasChildren<TNode>(this TNode node, Func<TNode, IEnumerable<TNode>> selectChildren)
            where TNode : class
        {
            return node.AsTraversable(x => null, selectChildren).HasChildren();
        }

        public static bool HasSiblings<TNode>(this TNode node, Func<TNode, TNode> selectParent, Func<TNode, IEnumerable<TNode>> selectChildren)
            where TNode : class
        {
            return node.AsTraversable(selectParent, selectChildren).HasSiblings();
        }

        /// <inheritdoc cref="TraversableExtensions.GetLevel{TNode}(TNode)"/>
        public static int GetLevel<TNode>(this TNode node, Func<TNode, TNode> selectParent)
            where TNode : class
        {
            return node.AsTraversable(selectParent, x => Enumerable.Empty<TNode>()).GetLevel();
        }

        /// <inheritdoc cref="TraversableExtensions.GetMaxDepth{TNode}(TNode)"/>
        public static int GetMaxDepth<TNode>(this TNode node, Func<TNode, TNode> selectParent, Func<TNode, IEnumerable<TNode>> selectChildren)
            where TNode : class
        {
            return node.AsTraversable(selectParent, selectChildren).GetMaxDepth();
        }

        public static bool IsChildOf<TNode>(this TNode node, TNode other, Func<TNode, TNode> selectParent)
            where TNode : class
        {
            var otherAdapter = other.AsTraversable(selectParent, x => Enumerable.Empty<TNode>());
            return node.AsTraversable(selectParent, x => Enumerable.Empty<TNode>()).IsChildOf(otherAdapter);
        }

        public static bool IsParentOf<TNode>(this TNode node, TNode other, Func<TNode, TNode> selectParent)
            where TNode : class
        {
            var otherAdapter = other.AsTraversable(selectParent, x => Enumerable.Empty<TNode>());
            return node.AsTraversable(selectParent, x => Enumerable.Empty<TNode>()).IsParentOf(otherAdapter);
        }

        public static bool IsSiblingOf<TNode>(this TNode node, TNode other, Func<TNode, TNode> selectParent)
            where TNode : class
        {
            var otherAdapter = other.AsTraversable(selectParent, x => Enumerable.Empty<TNode>());
            return node.AsTraversable(selectParent, x => Enumerable.Empty<TNode>()).IsSiblingOf(otherAdapter);
        }

        public static bool IsDescendantOf<TNode>(this TNode node, TNode other, Func<TNode, TNode> selectParent)
            where TNode : class
        {
            var otherAdapter = other.AsTraversable(selectParent, x => Enumerable.Empty<TNode>());
            return node.AsTraversable(selectParent, x => Enumerable.Empty<TNode>()).IsDescendantOf(otherAdapter);
        }

        public static bool IsAncestorOf<TNode>(this TNode node, TNode other, Func<TNode, TNode> selectParent)
            where TNode : class
        {
            var otherAdapter = other.AsTraversable(selectParent, x => Enumerable.Empty<TNode>());
            return node.AsTraversable(selectParent, x => Enumerable.Empty<TNode>()).IsAncestorOf(otherAdapter);
        }

        #endregion

        #region Related Nodes Retrieval

        public static TNode GetRoot<TNode>(this TNode node, Func<TNode, TNode> selectParent)
            where TNode : class
        {
            return node.AsTraversable(selectParent, x => Enumerable.Empty<TNode>()).GetRoot().Instance;
        }

        /// <inheritdoc cref="TraversableExtensions.WithParent{TNode}(TNode)"/>
        public static IEnumerable<TNode> WithParent<TNode>(this TNode node, Func<TNode, TNode> selectParent)
            where TNode : class
        {
            return node.AsTraversable(selectParent, x => Enumerable.Empty<TNode>()).WithParent().Select(x => x.Instance);
        }

        /// <inheritdoc cref="TraversableExtensions.WithChildren{TNode}(TNode)"/>
        public static IEnumerable<TNode> WithChildren<TNode>(this TNode node, Func<TNode, IEnumerable<TNode>> selectChildren)
            where TNode : class
        {
            return node.AsTraversable(x => null, selectChildren).WithChildren().Select(x => x.Instance);
        }

        public static IEnumerable<TNode> Siblings<TNode>(this TNode node, Func<TNode, TNode> selectParent, Func<TNode, IEnumerable<TNode>> selectChildren)
            where TNode : class
        {
            return node.AsTraversable(selectParent, selectChildren).Siblings().Select(x => x.Instance);
        }

        /// <inheritdoc cref="TraversableExtensions.WithSiblings{TNode}(TNode)"/>
        public static IEnumerable<TNode> WithSiblings<TNode>(this TNode node, Func<TNode, TNode> selectParent, Func<TNode, IEnumerable<TNode>> selectChildren)
            where TNode : class
        {
            return node.AsTraversable(selectParent, selectChildren).WithSiblings().Select(x => x.Instance);
        }

        public static IEnumerable<TNode> Descendants<TNode>(this TNode node, Func<TNode, IEnumerable<TNode>> selectChildren, TraversalMode traversalMode = TraversalMode.DepthFirst)
            where TNode : class
        {
            return node.AsTraversable(x => null, selectChildren).Descendants(traversalMode).Select(x => x.Instance);
        }

        public static IEnumerable<TNode> Descendants<TNode>(this TNode node, Func<TNode, IEnumerable<TNode>> selectChildren, ICandidateSelector<DefaultTraversableAdapter<TNode>> candidateSelector)
            where TNode : class
        {
            return node.AsTraversable(x => null, selectChildren).Descendants(candidateSelector).Select(x => x.Instance);
        }

        public static IEnumerable<TNode> WithDescendants<TNode>(this TNode node, Func<TNode, IEnumerable<TNode>> selectChildren, TraversalMode traversalMode = TraversalMode.DepthFirst)
            where TNode : class
        {
            return node.AsTraversable(x => null, selectChildren).WithDescendants(traversalMode).Select(x => x.Instance);
        }

        public static IEnumerable<TNode> WithDescendants<TNode>(this TNode node, Func<TNode, IEnumerable<TNode>> selectChildren, ICandidateSelector<DefaultTraversableAdapter<TNode>> candidateSelector)
            where TNode : class
        {
            return node.AsTraversable(x => null, selectChildren).WithDescendants(candidateSelector).Select(x => x.Instance);
        }

        public static IEnumerable<TNode> Ancestors<TNode>(this TNode node, Func<TNode, TNode> selectParent)
            where TNode : class
        {
            return node.AsTraversable(selectParent, x => Enumerable.Empty<TNode>()).Ancestors().Select(x => x.Instance);
        }

        public static IEnumerable<TNode> WithAncestors<TNode>(this TNode node, Func<TNode, TNode> selectParent)
            where TNode : class
        {
            return node.AsTraversable(selectParent, x => Enumerable.Empty<TNode>()).WithAncestors().Select(x => x.Instance);
        }

        #endregion

        #region Traverse Methods
        public static IAdapterTraverser<DefaultTraversableAdapter<TNode>, TNode> Traverse<TNode>(
            this TNode node,
            Func<TNode, IEnumerable<TNode>> selectChildren)
            where TNode : class
        {
            var adapter = node.AsTraversable(x => null, selectChildren);
            return new NonConvertibleTraverser<TNode>(adapter, selectChildren);
        }

        public static void Traverse<TNode>(
        this TNode node,
            Action<TNode> callback,
            Func<TNode, IEnumerable<TNode>> selectChildren,
            TraversalMode traversalMode = TraversalMode.DepthFirst)
            where TNode : class
        {
            node.AsTraversable(x => null, selectChildren)
                .Traverse(x => callback.Invoke(x.Instance), traversalMode);
        }

        public static void Traverse<TNode>(
            this TNode node,
            Action<TNode> callback,
            Func<TNode, IEnumerable<TNode>> selectChildren,
            ICandidateSelector<DefaultTraversableAdapter<TNode>> candidateSelector)
            where TNode : class
        {
            node.AsTraversable(x => null, selectChildren)
                .Traverse(x => callback.Invoke(x.Instance), candidateSelector);
        }

        public static Task TraverseAsync<TNode>(
            this TNode node,
            Action<TNode> callback,
            Func<TNode, IEnumerable<TNode>> selectChildren,
            TraversalMode traversalMode = TraversalMode.DepthFirst,
            CancellationToken cancellationToken = default)
            where TNode : class
        {
            return node.AsTraversable(x => null, selectChildren)
                .TraverseAsync(x => callback.Invoke(x.Instance), traversalMode, cancellationToken);
        }

        public static Task TraverseAsync<TNode>(
            this TNode node,
            Action<TNode> callback,
            Func<TNode, IEnumerable<TNode>> selectChildren,
            ICandidateSelector<DefaultTraversableAdapter<TNode>> candidateSelector,
            CancellationToken cancellationToken = default)
            where TNode : class
        {
            return node.AsTraversable(x => null, selectChildren)
                .TraverseAsync(x => callback.Invoke(x.Instance), candidateSelector, cancellationToken);
        }

        #endregion

        #region Common Methods

        public static DefaultTraversableAdapter<TConvertible> AsTraversable<TConvertible>(
            this TConvertible convertible,
            Func<TConvertible, TConvertible> getParentFunc,
            Func<TConvertible, IEnumerable<TConvertible>> getChildrenFunc)
            where TConvertible : class
        {
            return new DefaultTraversableAdapter<TConvertible>(convertible, getParentFunc, getChildrenFunc);
        }

        #endregion
    }
}
