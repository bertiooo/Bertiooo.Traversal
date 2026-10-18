using System.Collections.Generic;

namespace Bertiooo.Traversal.Selectors
{
	internal class BreadthFirstSelector<T> : ICandidateSelector<T>
    {
        private readonly Queue<T> _queue;

        public BreadthFirstSelector()
        {
			_queue = new Queue<T>();
		}

        public bool HasItems => _queue.Count > 0;

        public void Add(T item)
        {
            _queue.Enqueue(item);
        }

        public T Next()
        {
            return _queue.Dequeue();
        }

        public void Clear()
        {
            _queue.Clear();
        }
	}
}
