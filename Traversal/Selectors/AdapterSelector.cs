using System;

namespace Bertiooo.Traversal.Selectors
{
	public class AdapterSelector<TAdapter, TConvertible> : ICandidateSelector<TAdapter>
		where TAdapter : IInstanceProvider<TConvertible>
	{
		private readonly ICandidateSelector<TConvertible> _selector;
		private readonly Func<TConvertible, TAdapter> _createAdapterFunc;

		public AdapterSelector(
			ICandidateSelector<TConvertible> selector, 
			Func<TConvertible, TAdapter> createAdapterFunc) 
		{
			if(selector == null) 
				throw new ArgumentNullException(nameof(selector));

			if(createAdapterFunc == null)
				throw new ArgumentNullException(nameof(createAdapterFunc));

			_selector = selector;
			_createAdapterFunc = createAdapterFunc;
		}

		public bool HasItems => _selector.HasItems;

		public void Add(TAdapter item)
		{
			if(item == null)
				throw new ArgumentNullException(nameof(item));

			_selector.Add(item.Instance);
		}

		public TAdapter Next()
		{
			var next = _selector.Next();
			return _createAdapterFunc.Invoke(next);
		}

		public void Reset()
		{
			_selector.Reset();
		}
	}
}
