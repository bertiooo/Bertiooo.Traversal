using Bertiooo.Traversal;
using Tests.Model;

namespace Tests
{
	public class AdapterTests
	{
		[Fact]
		public void AdapterHandlesChildNodeChanges()
		{
			var root = new GenericConvertible();

			var firstChild = new GenericConvertible() { Parent = root };
			var secondChild = new GenericConvertible() { Parent = root };

			root.Children.Add(firstChild);
			root.Children.Add(secondChild);

			var adapter = root.AsTraversable();
			adapter.Traverse().Execute();

			root.Children.Remove(firstChild);
			adapter.Traverse().Execute();
		}
	}
}
