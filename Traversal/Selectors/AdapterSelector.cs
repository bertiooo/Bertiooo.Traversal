using System;
using System.Collections.Generic;

namespace Bertiooo.Traversal.Selectors
{
	public class AdapterSelector<TAdapter, TConvertible> : ICandidateSelector<TAdapter>
		where TAdapter : IInstanceProvider<TConvertible>
	{
		protected readonly ICandidateSelector<TConvertible> Selector;
		protected readonly IDictionary<TConvertible, TAdapter> Adapters;

		public AdapterSelector(ICandidateSelector<TConvertible> selector)
		{
			if (selector == null)
				throw new ArgumentNullException(nameof(selector));

			this.Selector = selector;
			this.Adapters = new Dictionary<TConvertible, TAdapter>();
		}

		public virtual bool HasItems => this.Selector.HasItems;

		public virtual void Add(TAdapter item)
		{
			if(item == null)
				throw new ArgumentNullException(nameof(item));

			this.Selector.Add(item.Instance);
			this.Adapters.Add(item.Instance, item);
		}

		public virtual TAdapter Next()
		{
			var next = this.Selector.Next();

			TAdapter adapter;
			var success = this.Adapters.TryGetValue(next, out adapter);

			if(success == false)
			{
				throw new InvalidOperationException($"The item '{next?.ToString() ?? string.Empty}' was not added to the candidate selector.");
			}

			this.Adapters.Remove(next);
			return adapter;
		}

		public virtual void Reset()
		{
			this.Selector.Reset();
		}
	}
}
