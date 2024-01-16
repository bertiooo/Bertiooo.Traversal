using System;
using System.Collections.Generic;
using System.Linq;

namespace Bertiooo.Traversal.Traverser
{
	internal class TraversalConvertibleTraverser<TConvertible> 
		: AbstractAdapterTraverser<TConvertible>
		where TConvertible : class, ITraversalConvertible
	{
		private readonly Func<TConvertible, IEnumerable<TConvertible>> getChildrenFunc;

		public TraversalConvertibleTraverser(
			TConvertible root,
			Func<TConvertible, IEnumerable<TConvertible>> getChildrenFunc) 
			: base(root.AsChildrenProvider(getChildrenFunc))
		{
			this.getChildrenFunc = getChildrenFunc;
		}

		public TraversalConvertibleTraverser(
			IEnumerable<TConvertible> startNodes,
			Func<TConvertible, IEnumerable<TConvertible>> getChildrenFunc)
			: base(startNodes.Select(x => x.AsChildrenProvider(getChildrenFunc)))
		{
			this.getChildrenFunc = getChildrenFunc;
		}

		protected TraversalConvertibleTraverser(ITraverser<AbstractTraversableAdapter<TConvertible>> traverser)
			: base(traverser)
		{
		}

		public override ITraverser<TConvertible> Clone()
		{
			return new TraversalConvertibleTraverser<TConvertible>(this.Traverser.Clone());
		}

		protected override AbstractTraversableAdapter<TConvertible> GetAdapter(TConvertible convertible)
		{
			if (convertible == null)
				throw new ArgumentNullException(nameof(convertible));

			return convertible.AsChildrenProvider(this.getChildrenFunc);
		}
	}
}
