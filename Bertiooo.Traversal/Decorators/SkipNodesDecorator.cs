using Bertiooo.Traversal.Selectors;
using System;

namespace Bertiooo.Traversal.Decorators
{
    public class SkipNodesDecorator<T> : ICandidateSelector<T>
    {
        private readonly ICandidateSelector<T> _selector;
        private readonly Func<T, bool> _predicate;

        /// <summary>
        /// Constructs a new <see cref="SkipNodesDecorator{T}"/>.
        /// </summary>
        /// <param name="selector">The source selector to use internally.</param>
        /// <param name="predicate">A predicate selecting those nodes to be skipped.</param>
        public SkipNodesDecorator(
            ICandidateSelector<T> selector,
            Func<T, bool> predicate) 
        { 
            _selector = selector ?? throw new ArgumentNullException(nameof(selector));
            _predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
        }

        public bool HasItems => _selector.HasItems;

        public void Add(T item)
        {
            var skipItem = _predicate.Invoke(item);

            if (skipItem)
                return;

            _selector.Add(item);
        }

        public T Next()
        {
            return _selector.Next();
        }

        public void Clear()
        {
            _selector.Clear();
        }
    }
}
