using System;
using System.Collections.Generic;
using System.Text;

namespace Bertiooo.Traversal.Selectors
{
    public class BreadthFirstSelector<T> : ICandidateSelector<T>
    {
        private readonly Queue<T> _queue = new Queue<T>();

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
    }
}
