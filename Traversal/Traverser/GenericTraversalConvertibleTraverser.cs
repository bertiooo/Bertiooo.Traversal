using System;

namespace Bertiooo.Traversal.Traverser
{
	internal class GenericTraversalConvertibleTraverser<TAdapter, TConvertible> : AbstractAdapterTraverser<TAdapter, TConvertible>
		where TAdapter : class, IInstanceProvider<TConvertible>, ITraversable<TAdapter>
		where TConvertible : ITraversalConvertible<TAdapter, TConvertible>
	{
		public GenericTraversalConvertibleTraverser(TAdapter root) : base(root)
		{
		}

		protected override TAdapter GetAdapter(TConvertible convertible)
		{
			if (convertible == null)
				throw new ArgumentNullException(nameof(convertible));

			return convertible.AsTraversable();
		}
	}
}
