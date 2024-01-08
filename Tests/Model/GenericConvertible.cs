using Bertiooo.Traversal;

namespace Tests.Model
{
	public class GenericConvertible : ITraversalConvertible<DefaultTraversableAdapter<GenericConvertible>, GenericConvertible>
	{
		private readonly Lazy<DefaultTraversableAdapter<GenericConvertible>> lazyAdapter;

		public GenericConvertible()
		{
			lazyAdapter = new Lazy<DefaultTraversableAdapter<GenericConvertible>>(
				() => new DefaultTraversableAdapter<GenericConvertible>(this, x => x.Parent, x => x.Children));
		}

		public string? Name { get; set; }

		public GenericConvertible? Parent { get; set; }

		public IList<GenericConvertible> Children { get; set; } = new List<GenericConvertible>();

		public DefaultTraversableAdapter<GenericConvertible> AsTraversable()
		{
			return lazyAdapter.Value;
		}
	}
}
