using System.Collections.Generic;

namespace Bertiooo.Traversal
{
	public interface IChildrenProvider<out T>
		where T : IChildrenProvider<T>
	{
		IEnumerable<T> Children { get; }
	}
}
