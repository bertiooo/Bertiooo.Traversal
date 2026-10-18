using System.Collections.Generic;

namespace Bertiooo.Traversal
{
    public interface ITraversalStrategy<T>
    {
        T? GetParent(T node);

        IEnumerable<T> GetChildren(T node);
    }
}
