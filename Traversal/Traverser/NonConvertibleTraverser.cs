using Bertiooo.Traversal.NonConvertible;
using System;
using System.Collections.Generic;

namespace Bertiooo.Traversal.Traverser
{
	internal class NonConvertibleTraverser<TConvertible> : AbstractAdapterTraverser<DefaultTraversableAdapter<TConvertible>, TConvertible>
		where TConvertible : class
	{
		private readonly Func<TConvertible, IEnumerable<TConvertible>> getChildrenFunc;

		public NonConvertibleTraverser(
			DefaultTraversableAdapter<TConvertible> root,
			Func<TConvertible, IEnumerable<TConvertible>> getChildrenFunc) 
			: base(root)
		{
			this.getChildrenFunc = getChildrenFunc;
		}

		protected override DefaultTraversableAdapter<TConvertible> GetAdapter(TConvertible convertible)
		{
			return convertible.AsTraversable(x => null, this.getChildrenFunc);
		}
	}
}
