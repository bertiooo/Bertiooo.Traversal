using Bertiooo.Traversal;

namespace Tests.Model
{
	public class GenericConvertible : ITraversalConvertible<GenericConvertible?>
	{
		private AbstractTraversableAdapter<GenericConvertible?>? adapter;

		public string? Name { get; set; }

		public GenericConvertible? Parent { get; set; }

		public IList<GenericConvertible> Children { get; set; } = new List<GenericConvertible>();

		public AbstractTraversableAdapter<GenericConvertible?> AsTraversable()
		{
			// lazy instantiation
			if(adapter == null)
			{
				adapter = new DefaultTraversableAdapter<GenericConvertible?>(this, x => x?.Parent, x => x?.Children);
			}

			return adapter;
		}
	}
}
