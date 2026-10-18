using System;
using System.Collections.Generic;

namespace Bertiooo.Traversal.Decorators
{
    public class InverseComparisonDecorator<T> : IComparer<T>
    {
        private readonly IComparer<T> _comparer;

        public InverseComparisonDecorator(IComparer<T> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            _comparer = comparer;
        }

        public int Compare(T x, T y)
        {
            return _comparer.Compare(x, y) * -1;
        }
    }
}
