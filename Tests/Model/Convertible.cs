using Bertiooo.Traversal;

namespace Tests.Model
{
	public class Convertible : ITraversalConvertible
	{
		public string? Name { get; set; }

		public Convertible Parent { get; set; }

		public IEnumerable<Convertible> Children { get; set; } = Enumerable.Empty<Convertible>();
	}
}
