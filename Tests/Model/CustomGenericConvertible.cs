using Bertiooo.Traversal;

namespace Tests.Model
{
	public class CustomGenericConvertible : ITraversalConvertible<CustomAdapter, CustomGenericConvertible?>
	{
		private CustomAdapter? adapter;

		public string? Name { get; set; }

		public CustomGenericConvertible? Parent { get; set; }

		public IList<CustomGenericConvertible> Children { get; set; } = new List<CustomGenericConvertible>();

		public CustomAdapter AsTraversable()
		{
			// lazy instantiation
			if(adapter == null)
			{
				adapter = new CustomAdapter(this);
			}

			return adapter;
		}
	}
}
