using System.Collections.Generic;

namespace Bertiooo.Traversal.Strategies
{
    internal class TraversableTraversalStrategy<T> : ITraversalStrategy<T>
        where T : ITraversable<T>
    {
        public T? GetParent(T node)
        {
            return node.Parent;
        }

        public IEnumerable<T> GetChildren(T node)
        {
            return node.Children;
        }
    }
}
