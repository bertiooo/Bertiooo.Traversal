using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bertiooo.Traversal
{
	public interface ITraversalConvertible<out TAdapter, out TConvertible>
		where TAdapter : ITraversable<TAdapter>, IInstanceProvider<TConvertible>
	{
		TAdapter AsTraversable();
	}
}
