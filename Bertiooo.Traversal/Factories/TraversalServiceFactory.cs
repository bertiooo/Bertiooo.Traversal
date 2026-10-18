using Bertiooo.Traversal.Services;
using System;

namespace Bertiooo.Traversal.Factories
{
    public class TraversalServiceFactory
    {
        private static readonly Lazy<TraversalServiceFactory> lazy =
            new Lazy<TraversalServiceFactory>(() => new TraversalServiceFactory());

        public static TraversalServiceFactory Instance => lazy.Value;

        private TraversalServiceFactory()
        {
        }

        public ITraversalService<T> CreateService<T>()
        {
            return new TraversalService<T>();
        }
    }
}
