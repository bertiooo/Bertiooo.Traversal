using System.Collections.Generic;

namespace Bertiooo.Traversal.Selectors
{
	public class DepthFirstSelector<T> : ICandidateSelector<T>
    {
        private readonly Stack<T> _stack = new Stack<T>();

        public bool HasItems => _stack.Count > 0;

        public void Add(T item)
        {
            _stack.Push(item);
        }

		public void Reset()
		{
			_stack.Clear();
		}

		public T Next()
        {
            return _stack.Pop();
        }
    }
}
