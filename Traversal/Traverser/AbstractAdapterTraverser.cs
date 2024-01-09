using Bertiooo.Traversal.Selectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

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

	internal abstract class AbstractAdapterTraverser<TAdapter, TConvertible> : IAdapterTraverser<TAdapter, TConvertible>
		where TAdapter : class, IInstanceProvider<TConvertible>, IChildrenProvider<TAdapter>
	{
		private readonly ITraverser<TAdapter> _traverser;

		public AbstractAdapterTraverser(TAdapter root)
		{
			_traverser = new TraversableTraverser<TAdapter>(root);
		}

		protected abstract TAdapter GetAdapter(TConvertible convertible);

		public IAdapterTraverser<TAdapter, TConvertible> CancelIf(Func<bool> predicate)
		{
			_traverser.CancelIf(predicate);
			return this;
		}

		public IAdapterTraverser<TAdapter, TConvertible> CancelIf(Func<TConvertible, bool> predicate)
		{
			if (predicate == null)
				throw new ArgumentNullException(nameof(predicate));

			Func<TAdapter, bool> wrapper = adapter => predicate.Invoke(adapter.Instance);
			_traverser.CancelIf(wrapper);

			return this;
		}

		public IAdapterTraverser<TAdapter, TConvertible> CancelIf<T>(Func<T, bool> predicate)
			where T : class, TConvertible
		{
			if (predicate == null)
				throw new ArgumentNullException(nameof(predicate));

			Func<TConvertible, bool> wrapper = node =>
			{
				var derivative = node as T;

				if (derivative != null)
				{
					return predicate.Invoke(derivative);
				}

				return false;
			};

			return this.CancelIf(wrapper);
		}

		public IAdapterTraverser<TAdapter, TConvertible> DisableCallbacksFor(TConvertible node)
		{
			var adapter = this.GetAdapter(node);
			_traverser.DisableCallbacksFor(adapter);

			return this;
		}

		public IAdapterTraverser<TAdapter, TConvertible> DisableCallbacksFor(IEnumerable<TConvertible> nodes)
		{
			if (nodes == null)
				throw new ArgumentNullException(nameof(nodes));

			var adapters = nodes.Select(this.GetAdapter);
			_traverser.DisableCallbacksFor(adapters);

			return this;
		}

		public IAdapterTraverser<TAdapter, TConvertible> DisableCallbacksFor(Func<TConvertible, bool> predicate)
		{
			if (predicate == null)
				throw new ArgumentNullException(nameof(predicate));

			Func<TAdapter, bool> wrapper = adapter => predicate.Invoke(adapter.Instance);
			_traverser.DisableCallbacksFor(wrapper);

			return this;
		}

		public IAdapterTraverser<TAdapter, TConvertible> DisableCallbacksFor<T>() where T : class, TConvertible
		{
			return this.DisableCallbacksFor(x => x is T);
		}

		public IAdapterTraverser<TAdapter, TConvertible> DisableCallbacksFor<T>(Func<T, bool> predicate)
			where T : class, TConvertible
		{
			if (predicate == null)
				throw new ArgumentNullException(nameof(predicate));

			Func<TConvertible, bool> wrapper = node =>
			{
				var derivative = node as T;

				if (derivative != null)
				{
					return predicate.Invoke(derivative);
				}

				return false;
			};

			return this.DisableCallbacksFor(wrapper);
		}

		public IAdapterTraverser<TAdapter, TConvertible> Exclude(TConvertible node)
		{
			var adapter = this.GetAdapter(node);
			_traverser.Exclude(adapter);

			return this;
		}

		public IAdapterTraverser<TAdapter, TConvertible> Exclude(IEnumerable<TConvertible> nodes)
		{
			if (nodes == null)
				throw new ArgumentNullException(nameof(nodes));

			var adapters = nodes.Select(this.GetAdapter);
			_traverser.Exclude(adapters);

			return this;
		}

		public IAdapterTraverser<TAdapter, TConvertible> Exclude(Func<TConvertible, bool> predicate)
		{
			if (predicate == null)
				throw new ArgumentNullException(nameof(predicate));

			Func<TAdapter, bool> wrapper = adapter => predicate.Invoke(adapter.Instance);
			_traverser.Exclude(wrapper);

			return this;
		}

		public IAdapterTraverser<TAdapter, TConvertible> Exclude<T>() where T : class, TConvertible
		{
			return this.Exclude(x => x is T);
		}

		public IAdapterTraverser<TAdapter, TConvertible> Exclude<T>(Func<T, bool> predicate)
			where T : class, TConvertible
		{
			if (predicate == null)
				throw new ArgumentNullException(nameof(predicate));

			Func<TConvertible, bool> wrapper = node =>
			{
				var derivative = node as T;

				if (derivative != null)
				{
					return predicate.Invoke(derivative);
				}

				return false;
			};

			return this.Exclude(wrapper);
		}

		public void Execute()
		{
			_traverser.Execute();
		}

		public Task ExecuteAsync()
		{
			return _traverser.ExecuteAsync();
		}

		public IAdapterTraverser<TAdapter, TConvertible> Finish(Action action)
		{
			_traverser.Finish(action);
			return this;
		}

		public IEnumerable<TConvertible> GetNodes()
		{
			return _traverser.GetNodes().Select(x => x.Instance);
		}

		public async Task<IList<TConvertible>> GetNodesAsync()
		{
			var nodes = await _traverser.GetNodesAsync();
			return nodes.Select(x => x.Instance).ToList();
		}

		public IAdapterTraverser<TAdapter, TConvertible> Ignore(TConvertible node)
		{
			var adapter = this.GetAdapter(node);
			_traverser.Ignore(adapter);

			return this;
		}

		public IAdapterTraverser<TAdapter, TConvertible> Ignore(IEnumerable<TConvertible> nodes)
		{
			if (nodes == null)
				throw new ArgumentNullException(nameof(nodes));

			var adapters = nodes.Select(this.GetAdapter);
			_traverser.Ignore(adapters);

			return this;
		}

		public IAdapterTraverser<TAdapter, TConvertible> Ignore(Func<TConvertible, bool> predicate)
		{
			if (predicate == null)
				throw new ArgumentNullException(nameof(predicate));

			Func<TAdapter, bool> wrapper = adapter => predicate.Invoke(adapter.Instance);
			_traverser.Ignore(wrapper);

			return this;
		}

		public IAdapterTraverser<TAdapter, TConvertible> Ignore<T>() where T : class, TConvertible
		{
			return this.Ignore(x => x is T);
		}

		public IAdapterTraverser<TAdapter, TConvertible> Ignore<T>(Func<T, bool> predicate)
			where T : class, TConvertible
		{
			if (predicate == null)
				throw new ArgumentNullException(nameof(predicate));

			Func<TConvertible, bool> wrapper = node =>
			{
				var derivative = node as T;

				if (derivative != null)
				{
					return predicate.Invoke(derivative);
				}

				return false;
			};

			return this.Ignore(wrapper);
		}

		public IAdapterTraverser<TAdapter, TConvertible> OnCanceled(Action action)
		{
			_traverser.OnCanceled(action);
			return this;
		}

		public IAdapterTraverser<TAdapter, TConvertible> OnFailure<T>(Action<T> action) where T : Exception
		{
			_traverser.OnFailure(action);
			return this;
		}

		public IAdapterTraverser<TAdapter, TConvertible> OnFailure<T>(Func<T, bool> action) where T : Exception
		{
			_traverser.OnFailure(action);
			return this;
		}

		public IAdapterTraverser<TAdapter, TConvertible> OnSuccess(Action action)
		{
			_traverser.OnSuccess(action);
			return this;
		}

		public IAdapterTraverser<TAdapter, TConvertible> Prepare(Action action)
		{
			_traverser.Prepare(action);
			return this;
		}

		public IAdapterTraverser<TAdapter, TConvertible> Skip(TConvertible node)
		{
			var adapter = this.GetAdapter(node);
			_traverser.Skip(adapter);

			return this;
		}

		public IAdapterTraverser<TAdapter, TConvertible> Skip(IEnumerable<TConvertible> nodes)
		{
			if (nodes == null)
				throw new ArgumentNullException(nameof(nodes));

			var adapters = nodes.Select(this.GetAdapter);
			_traverser.Skip(adapters);

			return this;
		}

		public IAdapterTraverser<TAdapter, TConvertible> Skip(Func<TConvertible, bool> predicate)
		{
			if (predicate == null)
				throw new ArgumentNullException(nameof(predicate));

			Func<TAdapter, bool> wrapper = adapter => predicate.Invoke(adapter.Instance);
			_traverser.Skip(wrapper);

			return this;
		}

		public IAdapterTraverser<TAdapter, TConvertible> Skip<T>() where T : class, TConvertible
		{
			return this.Skip(x => x is T);
		}

		public IAdapterTraverser<TAdapter, TConvertible> Skip<T>(Func<T, bool> predicate)
			where T : class, TConvertible
		{
			if (predicate == null)
				throw new ArgumentNullException(nameof(predicate));

			Func<TConvertible, bool> wrapper = node =>
			{
				var derivative = node as T;

				if (derivative != null)
				{
					return predicate.Invoke(derivative);
				}

				return false;
			};

			return this.Skip(wrapper);
		}

		public IAdapterTraverser<TAdapter, TConvertible> Use(ICandidateSelector<TAdapter> selector)
		{
			_traverser.Use(selector);
			return this;
		}

		public IAdapterTraverser<TAdapter, TConvertible> Use(TraversalMode mode)
		{
			_traverser.Use(mode);
			return this;
		}

		public IAdapterTraverser<TAdapter, TConvertible> Use(CancellationToken cancellationToken, bool throwException = true)
		{
			_traverser.Use(cancellationToken, throwException);
			return this;
		}

		public IAdapterTraverser<TAdapter, TConvertible> WithAction(Action action)
		{
			_traverser.WithAction(action);
			return this;
		}

		public IAdapterTraverser<TAdapter, TConvertible> WithAction(Action<TConvertible> action)
		{
			if (action == null)
				throw new ArgumentNullException(nameof(action));

			Action<TAdapter> wrapper = adapter => action.Invoke(adapter.Instance);
			_traverser.WithAction(wrapper);

			return this;
		}

		public IAdapterTraverser<TAdapter, TConvertible> WithAction<T>(Action<T> action)
			where T : class, TConvertible
		{
			if (action == null)
				throw new ArgumentNullException(nameof(action));

			Action<TConvertible> wrapper = node =>
			{
				var derivative = node as T;

				if (derivative != null)
				{
					action.Invoke(derivative);
				}
			};

			return this.WithAction(wrapper);
		}
	}
}
