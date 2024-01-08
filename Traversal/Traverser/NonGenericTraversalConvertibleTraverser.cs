using System;
using System.Collections.Generic;

namespace Bertiooo.Traversal.Traverser
{
    internal class NonGenericTraversalConvertibleTraverser<TConvertible> : AbstractAdapterTraverser<DefaultTraversableAdapter<TConvertible>, TConvertible>
		where TConvertible : class, ITraversalConvertible
	{
		private readonly Func<TConvertible, IEnumerable<TConvertible>> getChildrenFunc;

		public NonGenericTraversalConvertibleTraverser(
			TConvertible root,
			Func<TConvertible, IEnumerable<TConvertible>> getChildrenFunc) 
			: base(root.AsTraversable(x => null, getChildrenFunc))
		{
			this.getChildrenFunc = getChildrenFunc;
		}

		protected override DefaultTraversableAdapter<TConvertible> GetAdapter(TConvertible convertible)
		{
			if (convertible == null)
				throw new ArgumentNullException(nameof(convertible));

			return convertible.AsTraversable(x => null, this.getChildrenFunc);
		}
	}
}
