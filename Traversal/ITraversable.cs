using System.Collections.Generic;

namespace Bertiooo.Traversal
{
	public interface ITraversable<out TNode>
		where TNode : ITraversable<TNode>
	{
		TNode Parent { get; }

		IEnumerable<TNode> Children { get; }
	}
}
