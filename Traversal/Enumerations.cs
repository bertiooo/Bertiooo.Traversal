using System;
using System.Collections.Generic;
using Bertiooo.Traversal.Traverser;

namespace Bertiooo.Traversal
{
	public enum TraversalMode
	{
		DepthFirst,
		BreadthFirst,

		/// <summary>
		/// Uses the <see cref="Comparer{T}.Default"/> to determine in which order to traverse the nodes.
		/// It is advisable that the node should implement the <see cref="IComparable{T}"/> interface therefore.
		/// By default, the nodes will be traversed in descending order. 
		/// To change that, use the <see cref="ITraverser{TNode}.Use(IComparer{TNode}, bool)"/> method.
		/// </summary>
		DefaultComparer
	}
}
