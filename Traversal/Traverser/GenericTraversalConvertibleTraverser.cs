﻿using System;
using System.Collections.Generic;

namespace Bertiooo.Traversal.Traverser
{
	internal class GenericTraversalConvertibleTraverser<TAdapter, TConvertible> : AbstractAdapterTraverser<TAdapter, TConvertible>
		where TAdapter : IInstanceProvider<TConvertible>, ITraversable<TAdapter>
		where TConvertible : ITraversalConvertible<TAdapter, TConvertible>
	{
		public GenericTraversalConvertibleTraverser(TAdapter root) : base(root)
		{
		}

		public GenericTraversalConvertibleTraverser(IEnumerable<TAdapter> startNodes) : base(startNodes)
		{
		}

		protected GenericTraversalConvertibleTraverser(ITraverser<TAdapter> traverser)
			: base(traverser)
		{
		}

		public override ITraverser<TConvertible> Clone()
		{
			return new GenericTraversalConvertibleTraverser<TAdapter, TConvertible>(this.Traverser.Clone());
		}

		protected override TAdapter GetAdapter(TConvertible convertible)
		{
			if (convertible == null)
				throw new ArgumentNullException(nameof(convertible));

			return convertible.AsTraversable();
		}
	}
}
