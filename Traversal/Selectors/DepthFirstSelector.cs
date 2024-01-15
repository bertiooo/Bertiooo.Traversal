using System.Collections.Generic;
using System.Linq;

namespace Bertiooo.Traversal.Selectors
{
	public class DepthFirstSelector<T> : ICandidateSelector<T>
    {
        private readonly Stack<T> _stack;

		public DepthFirstSelector() 
		{
			_stack = new Stack<T>();
		}

		public DepthFirstSelector(IEnumerable<T> items)
		{
			_stack = new Stack<T>(items);
		}

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

		public object Clone()
		{
			return new DepthFirstSelector<T>(_stack.Reverse());
		}
	}
}
