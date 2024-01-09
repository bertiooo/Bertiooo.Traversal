using Bertiooo.Traversal;

namespace Tests.Model
{
	public class CustomAdapter : AbstractTraversableAdapter<CustomAdapter, CustomGenericConvertible?>
	{
		public CustomAdapter(CustomGenericConvertible? convertible) : base(convertible)
		{
		}

		protected override CustomGenericConvertible? ParentInstance => this.Instance?.Parent;

		protected override IEnumerable<CustomGenericConvertible>? ChildInstances => this.Instance?.Children;

		protected override CustomAdapter CreateAdapter(CustomGenericConvertible? convertible)
		{
			return new CustomAdapter(convertible);
		}
	}
}
