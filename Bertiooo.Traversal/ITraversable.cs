using System;
using System.Collections.Generic;
using System.Text;

namespace Bertiooo.Traversal
{
    public interface ITraversable<T>
    {
        T? Parent { get; }

        IEnumerable<T> Children { get; }
    }
}
