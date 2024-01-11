using Bertiooo.Traversal;

namespace Tests.Model
{
	public class GenericConvertible : ITraversalConvertible<GenericConvertible>, IComparable<GenericConvertible>
	{
		private AbstractTraversableAdapter<GenericConvertible>? adapter;

		public string? Name { get; set; }

		public GenericConvertible? Parent { get; set; }

		public IList<GenericConvertible> Children { get; set; } = new List<GenericConvertible>();

		public AbstractTraversableAdapter<GenericConvertible> AsTraversable()
		{
			// lazy instantiation
			if(adapter == null)
			{
				adapter = new DefaultTraversableAdapter<GenericConvertible>(this, x => x.Parent, x => x.Children);
			}

			return adapter;
		}

		public int CompareTo(GenericConvertible? other)
		{
			if (other == null)
				return 1;

			return Comparer<string>.Default.Compare(this.Name, other.Name);
		}
	}
}
