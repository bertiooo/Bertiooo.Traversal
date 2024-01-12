using Bertiooo.Traversal.Selectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bertiooo.Traversal.Traverser
{
	internal abstract class AbstractAdapterTraverser<TConvertible>
		: AbstractAdapterTraverser<AbstractTraversableAdapter<TConvertible>, TConvertible>
		where TConvertible : class
	{
		protected AbstractAdapterTraverser(AbstractTraversableAdapter<TConvertible> root) : base(root)
		{
		}
	}

	internal abstract class AbstractAdapterTraverser<TAdapter, TConvertible> : AbstractTraverser<TConvertible>
		where TAdapter : class, IInstanceProvider<TConvertible>, IChildrenProvider<TAdapter>
	{
		private readonly ITraverser<TAdapter> _traverser;

		public AbstractAdapterTraverser(TAdapter root)
		{
			_traverser = new TraversableTraverser<TAdapter>(root);
		}

		protected abstract TAdapter GetAdapter(TConvertible convertible);

		public override ITraverser<TConvertible> CancelIf(Func<TConvertible, bool> predicate)
		{
			if (predicate == null)
				throw new ArgumentNullException(nameof(predicate));

			Func<TAdapter, bool> wrapper = adapter => predicate.Invoke(adapter.Instance);
			_traverser.CancelIf(wrapper);

			return this;
		}

		public override ITraverser<TConvertible> Catch(Func<Exception, TConvertible, bool> action)
		{
			if (action == null)
				throw new ArgumentNullException(nameof(action));

			Func<Exception, TAdapter, bool> wrapper = (e, n) => action.Invoke(e, n.Instance);
			_traverser.Catch(wrapper);

			return this;
		}

		public override ITraverser<TConvertible> DisableCallbacksFor(TConvertible node)
		{
			var adapter = this.GetAdapter(node);
			_traverser.DisableCallbacksFor(adapter);

			return this;
		}

		public override ITraverser<TConvertible> DisableCallbacksFor(Func<TConvertible, bool> predicate)
		{
			if (predicate == null)
				throw new ArgumentNullException(nameof(predicate));

			Func<TAdapter, bool> wrapper = adapter => predicate.Invoke(adapter.Instance);
			_traverser.DisableCallbacksFor(wrapper);

			return this;
		}

		public override ITraverser<TConvertible> Exclude(TConvertible node)
		{
			var adapter = this.GetAdapter(node);
			_traverser.Exclude(adapter);

			return this;
		}

		public override ITraverser<TConvertible> Exclude(Func<TConvertible, bool> predicate)
		{
			if (predicate == null)
				throw new ArgumentNullException(nameof(predicate));

			Func<TAdapter, bool> wrapper = adapter => predicate.Invoke(adapter.Instance);
			_traverser.Exclude(wrapper);

			return this;
		}

		public override void Execute()
		{
			_traverser.Execute();
		}

		public override Task ExecuteAsync()
		{
			return _traverser.ExecuteAsync();
		}

		public override ITraverser<TConvertible> Finish(Action action)
		{
			_traverser.Finish(action);
			return this;
		}

		public override IEnumerable<TConvertible> GetNodes()
		{
			return _traverser.GetNodes().Select(x => x.Instance);
		}

		public override async Task<IList<TConvertible>> GetNodesAsync()
		{
			var nodes = await _traverser.GetNodesAsync();
			return nodes.Select(x => x.Instance).ToList();
		}

		public override ITraverser<TConvertible> OnCanceled(Action action)
		{
			_traverser.OnCanceled(action);
			return this;
		}

		public override ITraverser<TConvertible> OnSuccess(Action action)
		{
			_traverser.OnSuccess(action);
			return this;
		}

		public override ITraverser<TConvertible> Prepare(Action action)
		{
			_traverser.Prepare(action);
			return this;
		}

		public override ITraverser<TConvertible> ReverseOrder()
		{
			_traverser.ReverseOrder();
			return this;
		}

		public override ITraverser<TConvertible> Skip(TConvertible node)
		{
			var adapter = this.GetAdapter(node);
			_traverser.Skip(adapter);

			return this;
		}

		public override ITraverser<TConvertible> Skip(Func<TConvertible, bool> predicate)
		{
			if (predicate == null)
				throw new ArgumentNullException(nameof(predicate));

			Func<TAdapter, bool> wrapper = adapter => predicate.Invoke(adapter.Instance);
			_traverser.Skip(wrapper);

			return this;
		}

		public override ITraverser<TConvertible> Use(ICandidateSelector<TConvertible> selector)
		{
			var adapterSelector = new AdapterSelector<TAdapter, TConvertible>(selector);
			_traverser.Use(adapterSelector);

			return this;
		}

		public override ITraverser<TConvertible> Use(IComparer<TConvertible> comparer, bool ascending = false)
		{
			var adapterComparer = new AdapterComparer<TAdapter, TConvertible>(comparer);
			_traverser.Use(adapterComparer, ascending);

			return this;
		}

		public override ITraverser<TConvertible> Use(TraversalMode mode)
		{
			_traverser.Use(mode);
			return this;
		}

		public override ITraverser<TConvertible> Use(CancellationToken cancellationToken, bool throwException = true)
		{
			_traverser.Use(cancellationToken, throwException);
			return this;
		}

		public override ITraverser<TConvertible> WithAction(Action<TConvertible> action)
		{
			if (action == null)
				throw new ArgumentNullException(nameof(action));

			Action<TAdapter> wrapper = adapter => action.Invoke(adapter.Instance);
			_traverser.WithAction(wrapper);

			return this;
		}
	}
}
