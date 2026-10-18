using System;
using System.Collections.Generic;
using System.Linq;

namespace Bertiooo.Traversal.Adapters
{
    internal class TraversalStrategyAdapter<TNode, TTarget> : ITraversalStrategy<TTarget>
        where TNode : class, TTarget
    {
        private readonly ITraversalStrategy<TNode> _strategy;

        public TraversalStrategyAdapter(ITraversalStrategy<TNode> strategy)
        {
            _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
        }

        public IEnumerable<TTarget> GetChildren(TTarget node)
        {
            var concreteNode = node as TNode;

            if (concreteNode == null)
            {
                throw new ArgumentException($"Node must be of type {typeof(TNode).Name}");
            }

            return _strategy.GetChildren(concreteNode).Cast<TTarget>();
        }

        public TTarget? GetParent(TTarget node)
        {
            var concreteNode = node as TNode;

            if (concreteNode == null)
            {
                throw new ArgumentException($"Node must be of type {typeof(TNode).Name}");
            }

            return _strategy.GetParent(concreteNode);
        }
    }
}
