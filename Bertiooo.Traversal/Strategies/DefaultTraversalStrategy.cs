using System;
using System.Collections.Generic;

namespace Bertiooo.Traversal.Strategies
{
    internal class DefaultTraversalStrategy<T> : ITraversalStrategy<T>
    {
        private readonly Func<T, T?> _getParentFunc;
        private readonly Func<T, IEnumerable<T>> _getChildrenFunc;

        public DefaultTraversalStrategy(
            Func<T, T?> getParentFunc,
            Func<T, IEnumerable<T>> getChildrenFunc)
        {
            _getParentFunc = getParentFunc ?? throw new ArgumentNullException(nameof(getParentFunc));
            _getChildrenFunc = getChildrenFunc ?? throw new ArgumentNullException(nameof(getChildrenFunc));
        }

        public T? GetParent(T node)
        {
            return _getParentFunc(node);
        }

        public IEnumerable<T> GetChildren(T node)
        {
            return _getChildrenFunc(node);
        }
    }
}
