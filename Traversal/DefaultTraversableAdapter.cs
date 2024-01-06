using System;
using System.Collections.Generic;

namespace Bertiooo.Traversal
{
	public class DefaultTraversableAdapter<TConvertible> 
		: AbstractTraversableAdapter<DefaultTraversableAdapter<TConvertible>, TConvertible>
		where TConvertible : class
	{
		private readonly Func<TConvertible, TConvertible> getParentFunc;
		private readonly Func<TConvertible, IEnumerable<TConvertible>> getChildrenFunc;

		public DefaultTraversableAdapter(
			TConvertible convertible,
			Func<TConvertible, TConvertible> getParentFunc,
			Func<TConvertible, IEnumerable<TConvertible>> getChildrenFunc)
			: base(convertible)
		{
			this.getParentFunc = getParentFunc;
			this.getChildrenFunc = getChildrenFunc;
		}

		protected override TConvertible ParentInstance => getParentFunc.Invoke(this.Instance);

		protected override IEnumerable<TConvertible> ChildInstances => getChildrenFunc.Invoke(this.Instance);

		protected override DefaultTraversableAdapter<TConvertible> CreateAdapter(TConvertible convertible)
		{
			return new DefaultTraversableAdapter<TConvertible>(convertible, getParentFunc, getChildrenFunc);
		}
	}
}
