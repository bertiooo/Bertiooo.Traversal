using Bertiooo.Traversal;

namespace Tests.Model
{
	public class Convertible : ITraversalConvertible<DefaultTraversableAdapter<Convertible>, Convertible>
	{
		private readonly Lazy<DefaultTraversableAdapter<Convertible>> lazyAdapter;

		public Convertible()
		{
			lazyAdapter = new Lazy<DefaultTraversableAdapter<Convertible>>(
				() => new DefaultTraversableAdapter<Convertible>(this, x => x.Parent, x => x.Children));
		}

		public string? Name { get; set; }

		public Convertible? Parent { get; set; }

		public IList<Convertible> Children { get; set; } = new List<Convertible>();

		public DefaultTraversableAdapter<Convertible> AsTraversable()
		{
			return lazyAdapter.Value;
		}
	}
}
