using System.Collections.Generic;
using System.Linq;

namespace Bertiooo.Traversal.Selectors
{
	/// <summary>
	/// The <see cref="DefaultCandidateSelector{T}"/> returns the first item in a <see cref="SortedSet{T}"/>
	/// as the next item to traverse. The <see cref="IComparer{T}"/> given as parameter in the constructor
	/// should therefore mark items as lesser for those who should appear in order first.
	/// </summary>
	public class DefaultCandidateSelector<T> : ICandidateSelector<T>
	{
		private readonly SortedSet<T> _candidates;

		public DefaultCandidateSelector(IComparer<T> comparer)
		{
			_candidates = new SortedSet<T>(comparer);
		}

		public bool HasItems => _candidates.Count > 0;

		public void Add(T item)
		{
			_candidates.Add(item);
		}

		public T Next()
		{
			var item = _candidates.First();
			_candidates.Remove(item);

			return item;
		}

		public void Reset()
		{
			_candidates.Clear();
		}
	}
}
