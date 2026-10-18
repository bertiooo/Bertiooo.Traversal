using Bertiooo.Traversal.Selectors;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Bertiooo.Traversal.Services
{
    internal class TraversalService<T> : ITraversalService<T>
    {
        public IEnumerable<T> Traverse(
            ITraversalStrategy<T> strategy, 
            ICandidateSelector<T> selector, 
            CancellationToken cancellationToken = default)
        {
            if (strategy == null)
                throw new ArgumentNullException(nameof(strategy));

            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Iterator();

            IEnumerable<T> Iterator()
            {
                while (selector.HasItems)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var node = selector.Next();
                    yield return node;

                    var children = strategy.GetChildren(node);

                    foreach (var child in children)
                    {
                        selector.Add(child);
                    }
                }
            }
        }
    }
}
