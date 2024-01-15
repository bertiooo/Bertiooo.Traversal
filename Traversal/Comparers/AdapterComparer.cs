using System;
using System.Collections.Generic;

namespace Bertiooo.Traversal.Comparers
{
    public class AdapterComparer<TAdapter, TConvertible> : IComparer<TAdapter>
        where TAdapter : IInstanceProvider<TConvertible>
    {
        private readonly IComparer<TConvertible> _comparer;

        public AdapterComparer(IComparer<TConvertible> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            _comparer = comparer;
        }

        public int Compare(TAdapter x, TAdapter y)
        {
            return _comparer.Compare(x.Instance, y.Instance);
        }
    }
}
