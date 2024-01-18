using Bertiooo.Traversal.Comparers;
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
	{
		protected AbstractAdapterTraverser(AbstractTraversableAdapter<TConvertible> root) : base(root)
		{
		}

		protected AbstractAdapterTraverser(IEnumerable<AbstractTraversableAdapter<TConvertible>> startNodes) : base(startNodes)
		{
		}

		protected AbstractAdapterTraverser(ITraverser<AbstractTraversableAdapter<TConvertible>> traverser)
			: base(traverser)
		{
		}
	}

	internal abstract class AbstractAdapterTraverser<TAdapter, TConvertible> : AbstractTraverser<TConvertible>
		where TAdapter : IInstanceProvider<TConvertible>, IChildrenProvider<TAdapter>
	{
		protected readonly ITraverser<TAdapter> Traverser;

		protected AbstractAdapterTraverser(TAdapter root)
		{
			this.Traverser = new TraversableTraverser<TAdapter>(root);
		}

		protected AbstractAdapterTraverser(IEnumerable<TAdapter> startNodes)
		{
			this.Traverser = new TraversableTraverser<TAdapter>(startNodes);
		}

		protected AbstractAdapterTraverser(ITraverser<TAdapter> traverser)
		{
			this.Traverser = traverser;
		}

		protected abstract TAdapter GetAdapter(TConvertible convertible);

		public override ITraverser<TConvertible> CancelIf(Func<TConvertible, bool> predicate)
		{
			if (predicate == null)
				throw new ArgumentNullException(nameof(predicate));

			Func<TAdapter, bool> wrapper = adapter => predicate.Invoke(adapter.Instance);
			this.Traverser.CancelIf(wrapper);

			return this;
		}

		public override ITraverser<TConvertible> Catch(Func<Exception, TConvertible, bool> action)
		{
			if (action == null)
				throw new ArgumentNullException(nameof(action));

			Func<Exception, TAdapter, bool> wrapper = (e, n) => action.Invoke(e, n.Instance);
			this.Traverser.Catch(wrapper);

			return this;
		}

		public override ITraverser<TConvertible> DisableCallbacksFor(TConvertible node)
		{
			var adapter = this.GetAdapter(node);
			this.Traverser.DisableCallbacksFor(adapter);

			return this;
		}

		public override ITraverser<TConvertible> DisableCallbacksFor(Func<TConvertible, bool> predicate)
		{
			if (predicate == null)
				throw new ArgumentNullException(nameof(predicate));

			Func<TAdapter, bool> wrapper = adapter => predicate.Invoke(adapter.Instance);
			this.Traverser.DisableCallbacksFor(wrapper);

			return this;
		}

		public override ITraverser<TConvertible> DisableReset()
		{
			this.Traverser.DisableReset();
			return this;
		}

		public override ITraverser<TConvertible> DisableResetAfter()
		{
			this.Traverser.DisableResetAfter();
			return this;
		}

		public override ITraverser<TConvertible> DisableResetBefore()
		{
			this.Traverser.DisableResetBefore();
			return this;
		}

		public override ITraverser<TConvertible> Exclude(TConvertible node)
		{
			var adapter = this.GetAdapter(node);
			this.Traverser.Exclude(adapter);

			return this;
		}

		public override ITraverser<TConvertible> Exclude(Func<TConvertible, bool> predicate)
		{
			if (predicate == null)
				throw new ArgumentNullException(nameof(predicate));

			Func<TAdapter, bool> wrapper = adapter => predicate.Invoke(adapter.Instance);
			this.Traverser.Exclude(wrapper);

			return this;
		}

		public override void Execute()
		{
			this.Traverser.Execute();
		}

		public override Task ExecuteAsync()
		{
			return this.Traverser.ExecuteAsync();
		}

		public override ITraverser<TConvertible> Finish(Action action)
		{
			this.Traverser.Finish(action);
			return this;
		}

		public override IEnumerable<TConvertible> GetNodes()
		{
			return this.Traverser.GetNodes().Select(x => x.Instance);
		}

		public override async Task<IList<TConvertible>> GetNodesAsync()
		{
			var nodes = await this.Traverser.GetNodesAsync();
			return nodes.Select(x => x.Instance).ToList();
		}

		public override ITraverser<TConvertible> OnCanceled(Action action)
		{
			this.Traverser.OnCanceled(action);
			return this;
		}

		public override ITraverser<TConvertible> OnSuccess(Action action)
		{
			this.Traverser.OnSuccess(action);
			return this;
		}

		public override ITraverser<TConvertible> Prepare(Action action)
		{
			this.Traverser.Prepare(action);
			return this;
		}

		public override ITraverser<TConvertible> ReverseOrder()
		{
			this.Traverser.ReverseOrder();
			return this;
		}

		public override ITraverser<TConvertible> Skip(TConvertible node)
		{
			var adapter = this.GetAdapter(node);
			this.Traverser.Skip(adapter);

			return this;
		}

		public override ITraverser<TConvertible> Skip(Func<TConvertible, bool> predicate)
		{
			if (predicate == null)
				throw new ArgumentNullException(nameof(predicate));

			Func<TAdapter, bool> wrapper = adapter => predicate.Invoke(adapter.Instance);
			this.Traverser.Skip(wrapper);

			return this;
		}

		public override ITraverser<TConvertible> Use(ICandidateSelector<TConvertible> selector)
		{
			var adapterSelector = new AdapterSelector<TAdapter, TConvertible>(selector);
			this.Traverser.Use(adapterSelector);

			return this;
		}

		public override ITraverser<TConvertible> Use(IComparer<TConvertible> comparer, bool ascending = false)
		{
			var adapterComparer = new AdapterComparer<TAdapter, TConvertible>(comparer);
			this.Traverser.Use(adapterComparer, ascending);

			return this;
		}

		public override ITraverser<TConvertible> Use(TraversalMode mode)
		{
			this.Traverser.Use(mode);
			return this;
		}

		public override ITraverser<TConvertible> Use(CancellationToken cancellationToken, bool throwException = true)
		{
			this.Traverser.Use(cancellationToken, throwException);
			return this;
		}

		public override ITraverser<TConvertible> WithAction(Action<TConvertible> action)
		{
			if (action == null)
				throw new ArgumentNullException(nameof(action));

			Action<TAdapter> wrapper = adapter => action.Invoke(adapter.Instance);
			this.Traverser.WithAction(wrapper);

			return this;
		}
	}
}
