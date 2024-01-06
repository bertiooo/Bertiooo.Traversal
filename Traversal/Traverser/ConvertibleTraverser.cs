using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bertiooo.Traversal.Traverser
{
	internal class ConvertibleTraverser<TAdapter, TConvertible> : AbstractAdapterTraverser<TAdapter, TConvertible>
		where TAdapter : IInstanceProvider<TConvertible>, ITraversable<TAdapter>
		where TConvertible : ITraversalConvertible<TAdapter, TConvertible>
	{
		public ConvertibleTraverser(TAdapter root) : base(root)
		{
		}

		protected override TAdapter GetAdapter(TConvertible convertible)
		{
			return convertible.AsTraversable();
		}
	}
}
