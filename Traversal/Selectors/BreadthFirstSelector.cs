using System;
using System.Collections.Generic;

namespace Bertiooo.Traversal.Selectors
{
	public class BreadthFirstSelector<T> : ICandidateSelector<T>, ICloneable
    {
        private readonly Queue<T> _queue;

        public BreadthFirstSelector()
        {
			_queue = new Queue<T>();
		}

		public BreadthFirstSelector(IEnumerable<T> items)
        {
			_queue = new Queue<T>(items);
        }

        public bool HasItems => _queue.Count > 0;

        public void Add(T item)
        {
            _queue.Enqueue(item);
        }

        public void Reset()
        {
            _queue.Clear();
        }

        public T Next()
        {
            return _queue.Dequeue();
        }

		public object Clone()
		{
            return new BreadthFirstSelector<T>(_queue);
		}
	}
}
