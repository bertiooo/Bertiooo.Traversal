using System.Collections.Generic;
using System.Linq;

namespace Bertiooo.Traversal
{
	public abstract class AbstractTraversableAdapter<TConvertible>
		: AbstractTraversableAdapter<AbstractTraversableAdapter<TConvertible>, TConvertible>
		where TConvertible : class
	{
		protected AbstractTraversableAdapter(TConvertible convertible) 
			: base(convertible)
		{
		}
	}

	/// <remarks>
	/// Two adapters are equal if their instances are equal.
	/// Adapters created for parent and children are cached, thus will not be created
	/// each time the properties are accessed.
	/// </remarks>
	public abstract class AbstractTraversableAdapter<TAdapter, TConvertible>
		: ITraversable<TAdapter>, IInstanceProvider<TConvertible>
		where TAdapter : class, ITraversable<TAdapter>, IInstanceProvider<TConvertible>
		where TConvertible : class
	{
		private readonly TConvertible convertible;

		// cached adapters
		private TAdapter parentAdapter;
		private IList<TAdapter> childAdapters = new List<TAdapter>();

		protected AbstractTraversableAdapter(TConvertible convertible) 
		{
			this.convertible = convertible;
		}

		public TConvertible Instance => this.convertible;

		public TAdapter Parent 
		{
			get
			{
				var parentInstance = this.ParentInstance;

				if (parentInstance == null)
				{
					this.parentAdapter = null;
					return null;
				}

				if(parentAdapter != null && parentInstance.Equals(parentAdapter.Instance))
				{
					return parentAdapter;
				}

				// unsetting the old reference (if any) should make it a candidate for garbage collection
				this.parentAdapter = this.CreateAdapter(parentInstance);

				return this.parentAdapter;
			}
		}

		public IEnumerable<TAdapter> Children
		{
			get
			{
				var childInstances = this.ChildInstances;

				if (childInstances == null || childInstances.Any() == false)
				{
					this.childAdapters.Clear();
					yield break;
				}

				// remove each adapter with no corresponding instance
				var obsoleteAdapters = this.childAdapters.Where(x => childInstances.Contains(x.Instance) == false);
			
				foreach(var adapter in obsoleteAdapters)
				{
					this.childAdapters.Remove(adapter);
				}

				// iterate through each child instance and return the associated adapter
				foreach (var childInstance in childInstances)
				{
					var adapter = this.childAdapters.FirstOrDefault(x => Equals(x.Instance, childInstance));

					if(adapter == null)
					{
						adapter = this.CreateAdapter(childInstance);
						this.childAdapters.Add(adapter);
					}

					yield return adapter;
				}
			}
		}

		protected abstract TConvertible ParentInstance { get; }

		protected abstract IEnumerable<TConvertible> ChildInstances { get; }

		protected abstract TAdapter CreateAdapter(TConvertible convertible);

		public override bool Equals(object obj)
		{
			var adapter = obj as AbstractTraversableAdapter<TAdapter, TConvertible>;

			if (adapter == null)
				return false;

			return this.Instance.Equals(adapter.Instance);
		}

		public override int GetHashCode()
		{
			return this.Instance.GetHashCode();
		}
	}
}
