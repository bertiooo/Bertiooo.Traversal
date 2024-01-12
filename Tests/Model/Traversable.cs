using Bertiooo.Traversal;

namespace Tests.Model
{
	public class Traversable : ITraversable<Traversable>
    {
        public string? Name { get; set; }

        public Traversable Parent { get; set; }

        public IEnumerable<Traversable> Children { get; set; } = Enumerable.Empty<Traversable>();
    }
}
