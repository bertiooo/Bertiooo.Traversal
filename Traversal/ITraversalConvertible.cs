using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bertiooo.Traversal
{
	public interface ITraversalConvertible
	{
	}

	public interface ITraversalConvertible<TConvertible> 
		: ITraversalConvertible<AbstractTraversableAdapter<TConvertible>, TConvertible>
		where TConvertible : class
	{
	}

	public interface ITraversalConvertible<out TAdapter, out TConvertible>
		where TAdapter : ITraversable<TAdapter>, IInstanceProvider<TConvertible>
	{
		TAdapter AsTraversable();
	}
}
