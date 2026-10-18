using Bertiooo.Traversal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests.Helpers
{
    internal class TraversalStrategyAdapter<TNode> : ITraversalStrategy<object>
    {
        private readonly ITraversalStrategy<TNode> _strategy;

        public TraversalStrategyAdapter(ITraversalStrategy<TNode> strategy)
        {
            _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
        }

        public IEnumerable<object> GetChildren(object node)
        {
            return _strategy.GetChildren((TNode)node).Cast<object>();
        }

        public object? GetParent(object node)
        {
            return _strategy.GetParent((TNode)node);
        }
    }
}
