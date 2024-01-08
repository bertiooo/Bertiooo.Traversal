using System;
using System.Collections.Generic;
using System.Text;

namespace Bertiooo.Traversal
{
	public interface IParentProvider<out T>
		where T : IParentProvider<T>
	{
		T Parent { get; }
	}
}
