using System;
using System.Collections.Generic;
using System.Linq;

namespace Bertiooo.Traversal.Decorators
{
    public class ReverseOrderDecorator<T> : ITraversalStrategy<T>
    {
        private readonly ITraversalStrategy<T> _strategy;

        public ReverseOrderDecorator(ITraversalStrategy<T> strategy)
        {
            _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
        }

        public T? GetParent(T node)
        {
            return _strategy.GetParent(node);
        }

        public IEnumerable<T> GetChildren(T node)
        {
            return _strategy.GetChildren(node).Reverse();
        }
    }
}
