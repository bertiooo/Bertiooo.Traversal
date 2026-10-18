using Bertiooo.Traversal.Selectors;
using System;
using System.Collections.Generic;

namespace Bertiooo.Traversal.Factories
{
    public class CandidateSelectorFactory
    {
        private static readonly Lazy<CandidateSelectorFactory> lazy =
            new Lazy<CandidateSelectorFactory>(() => new CandidateSelectorFactory());

        public static CandidateSelectorFactory Instance => lazy.Value;

        private CandidateSelectorFactory()
        {
        }

        public ICandidateSelector<T> CreateSelector<T>(TraversalMode mode)
        {
            switch (mode)
            {
                case TraversalMode.DepthFirst:
                    return new DepthFirstSelector<T>();

                case TraversalMode.BreadthFirst:
                    return new BreadthFirstSelector<T>();

                default:
                    throw new NotImplementedException();
            }
        }

        public ICandidateSelector<T> CreateSelector<T>(IComparer<T> comparer)
        {
            return new DefaultCandidateSelector<T>(comparer);
        }
    }
}
