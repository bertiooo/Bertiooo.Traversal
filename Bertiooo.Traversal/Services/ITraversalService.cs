using Bertiooo.Traversal.Selectors;
using System.Collections.Generic;
using System.Threading;

namespace Bertiooo.Traversal.Services
{
    public interface ITraversalService<T>
    {
        IEnumerable<T> Traverse(
            ITraversalStrategy<T> strategy, 
            ICandidateSelector<T> selector,
            CancellationToken cancellationToken = default);
    }
}
