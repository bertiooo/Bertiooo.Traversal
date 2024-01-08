using System.Collections.Generic;

namespace Bertiooo.Traversal
{
	public interface ITraversable<out TNode> : IParentProvider<TNode>, IChildrenProvider<TNode>
		where TNode : ITraversable<TNode>
	{
	}
}
