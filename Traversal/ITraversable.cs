using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bertiooo.Traversal
{
	public interface ITraversable<out TNode>
		where TNode : ITraversable<TNode>
	{
		TNode? Parent { get; }

		IEnumerable<TNode> Children { get; }
	}
}
