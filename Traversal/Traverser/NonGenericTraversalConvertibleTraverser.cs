using System;
using System.Collections.Generic;

namespace Bertiooo.Traversal.Traverser
{
    internal class NonGenericTraversalConvertibleTraverser<TConvertible> 
		: AbstractAdapterTraverser<TConvertible>, IAdapterTraverser<TConvertible>
		where TConvertible : class, ITraversalConvertible
	{
		private readonly Func<TConvertible, IEnumerable<TConvertible>> getChildrenFunc;

		public NonGenericTraversalConvertibleTraverser(
			TConvertible root,
			Func<TConvertible, IEnumerable<TConvertible>> getChildrenFunc) 
			: base(root.AsChildrenProvider(getChildrenFunc))
		{
			this.getChildrenFunc = getChildrenFunc;
		}

		protected override AbstractTraversableAdapter<TConvertible> GetAdapter(TConvertible convertible)
		{
			if (convertible == null)
				throw new ArgumentNullException(nameof(convertible));

			return convertible.AsChildrenProvider(this.getChildrenFunc);
		}
	}
}
