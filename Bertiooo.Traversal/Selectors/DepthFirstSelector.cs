using System.Collections.Generic;

namespace Bertiooo.Traversal.Selectors
{
	internal class DepthFirstSelector<T> : ICandidateSelector<T>
    {
        private readonly Stack<T> _stack;

		public DepthFirstSelector() 
		{
			_stack = new Stack<T>();
		}

		public bool HasItems => _stack.Count > 0;

        public void Add(T item)
        {
            _stack.Push(item);
        }

		public T Next()
        {
            return _stack.Pop();
        }

        public void Clear()
        {
            _stack.Clear();
        }
	}
}
