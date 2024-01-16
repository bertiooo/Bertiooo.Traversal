using System;
using System.Collections.Generic;

namespace Bertiooo.Traversal.Selectors
{
	public class AdapterSelector<TAdapter, TConvertible> : ICandidateSelector<TAdapter>, ICloneable
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

		protected AdapterSelector(ICandidateSelector<TConvertible> selector, IDictionary<TConvertible, TAdapter> adapters)
		{
			if (selector == null)
				throw new ArgumentNullException(nameof(selector));

			if (adapters == null)
				throw new ArgumentNullException(nameof(adapters));

			this.Selector = selector;
			this.Adapters = new Dictionary<TConvertible, TAdapter>(adapters);
		}

		public virtual bool HasItems => this.Selector.HasItems;

		protected virtual ICandidateSelector<TConvertible> GetSelectorClone()
		{
			var cloneable = this.Selector as ICloneable;

			if (cloneable == null)
				return this.Selector;

			var clone = cloneable.Clone() as ICandidateSelector<TConvertible>;

			if (clone == null)
				return this.Selector;

			return clone;
		}

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

		public virtual object Clone()
		{
			var selectorClone = this.GetSelectorClone();
			return new AdapterSelector<TAdapter, TConvertible>(selectorClone, this.Adapters);
		}
	}
}
