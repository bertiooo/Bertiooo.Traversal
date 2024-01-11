using Bertiooo.Traversal.Selectors;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Bertiooo.Traversal.Traverser
{
	public abstract class AbstractTraverser<TNode> : ITraverser<TNode>
	{
		public virtual ITraverser<TNode> CancelIf(Func<bool> predicate)
		{
			if (predicate == null)
				throw new ArgumentNullException(nameof(predicate));

			Func<TNode, bool> wrapper = node => predicate.Invoke();
			return this.CancelIf(wrapper);
		}

		public abstract ITraverser<TNode> CancelIf(Func<TNode, bool> predicate);

		public virtual ITraverser<TNode> CancelIf<T>(Func<T, bool> predicate) where T : class, TNode
		{
			if (predicate == null)
				throw new ArgumentNullException(nameof(predicate));

			Func<TNode, bool> wrapper = node =>
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

		public virtual ITraverser<TNode> Catch(Action<Exception> action, bool throwException = true)
		{
			if (action == null)
				throw new ArgumentNullException(nameof(action));

			Func<Exception, TNode, bool> wrapper = (e, n) =>
			{
				action.Invoke(e);
				return throwException == false;
			};

			return this.Catch(wrapper);
		}

		public virtual ITraverser<TNode> Catch(Action<Exception, TNode> action, bool throwException = true)
		{
			if (action == null)
				throw new ArgumentNullException(nameof(action));

			Func<Exception, TNode, bool> wrapper = (e, n) =>
			{
				action.Invoke(e, n);
				return throwException == false;
			};

			return this.Catch(wrapper);
		}

		public ITraverser<TNode> Catch<T>(Action<T> action, bool throwException = true) where T : Exception
		{
			if (action == null)
				throw new ArgumentNullException(nameof(action));

			Func<Exception, TNode, bool> wrapper = (e, n) =>
			{
				var derivative = e as T;

				if(derivative != null)
				{
					action.Invoke(derivative);
				}

				return throwException == false;
			};

			return this.Catch(wrapper);
		}

		public ITraverser<TNode> Catch<T>(Action<T, TNode> action, bool throwException = true) where T : Exception
		{
			if (action == null)
				throw new ArgumentNullException(nameof(action));

			Func<Exception, TNode, bool> wrapper = (e, n) =>
			{
				var derivative = e as T;

				if (derivative != null)
				{
					action.Invoke(derivative, n);
				}

				return throwException == false;
			};

			return this.Catch(wrapper);
		}

		public virtual ITraverser<TNode> Catch(Func<Exception, bool> action)
		{
			if (action == null)
				throw new ArgumentNullException(nameof(action));

			Func<Exception, TNode, bool> wrapper = (e, n) => action.Invoke(e);
			return this.Catch(wrapper);
		}

		public abstract ITraverser<TNode> Catch(Func<Exception, TNode, bool> action);

		public ITraverser<TNode> Catch<T>(Func<T, bool> action) where T : Exception
		{
			if (action == null)
				throw new ArgumentNullException(nameof(action));

			Func<Exception, TNode, bool> wrapper = (e, n) =>
			{
				var derivative = e as T;

				if (derivative != null)
				{
					return action.Invoke(derivative);
				}

				return false;
			};

			return this.Catch(wrapper);
		}

		public ITraverser<TNode> Catch<T>(Func<T, TNode, bool> action) where T : Exception
		{
			if (action == null)
				throw new ArgumentNullException(nameof(action));

			Func<Exception, TNode, bool> wrapper = (e, n) =>
			{
				var derivative = e as T;

				if (derivative != null)
				{
					return action.Invoke(derivative, n);
				}

				return false;
			};

			return this.Catch(wrapper);
		}

		public abstract ITraverser<TNode> DisableCallbacksFor(TNode node);

		public virtual ITraverser<TNode> DisableCallbacksFor(IEnumerable<TNode> nodes)
		{
			if (nodes == null)
				throw new ArgumentNullException(nameof(nodes));

			foreach(var node in nodes)
			{
				this.DisableCallbacksFor(node);
			}

			return this;
		}

		public abstract ITraverser<TNode> DisableCallbacksFor(Func<TNode, bool> predicate);

		public virtual ITraverser<TNode> DisableCallbacksFor<T>() where T : class, TNode
		{
			return this.DisableCallbacksFor(x => x is T);
		}

		public virtual ITraverser<TNode> DisableCallbacksFor<T>(Func<T, bool> predicate) where T : class, TNode
		{
			if (predicate == null)
				throw new ArgumentNullException(nameof(predicate));

			Func<TNode, bool> wrapper = node =>
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

		public abstract ITraverser<TNode> Exclude(TNode node);

		public virtual ITraverser<TNode> Exclude(IEnumerable<TNode> nodes)
		{
			if (nodes == null)
				throw new ArgumentNullException(nameof(nodes));

			foreach (var node in nodes)
			{
				this.Exclude(node);
			}

			return this;
		}

		public abstract ITraverser<TNode> Exclude(Func<TNode, bool> predicate);

		public virtual ITraverser<TNode> Exclude<T>() where T : class, TNode
		{
			return this.Exclude(x => x is T);
		}

		public virtual ITraverser<TNode> Exclude<T>(Func<T, bool> predicate) where T : class, TNode
		{
			if (predicate == null)
				throw new ArgumentNullException(nameof(predicate));

			Func<TNode, bool> wrapper = node =>
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

		public abstract void Execute();

		public abstract Task ExecuteAsync();

		public abstract ITraverser<TNode> Finish(Action action);

		public abstract IEnumerable<TNode> GetNodes();

		public abstract Task<IList<TNode>> GetNodesAsync();

		public virtual ITraverser<TNode> Ignore(TNode node)
		{
			this.DisableCallbacksFor(node);
			this.Exclude(node);

			return this;
		}

		public virtual ITraverser<TNode> Ignore(IEnumerable<TNode> nodes)
		{
			if (nodes == null)
				throw new ArgumentNullException(nameof(nodes));

			this.DisableCallbacksFor(nodes);
			this.Exclude(nodes);

			return this;
		}

		public virtual ITraverser<TNode> Ignore(Func<TNode, bool> predicate)
		{
			if (predicate == null)
				throw new ArgumentNullException(nameof(predicate));

			this.DisableCallbacksFor(predicate);
			this.Exclude(predicate);

			return this;
		}

		public virtual ITraverser<TNode> Ignore<T>() where T : class, TNode
		{
			this.DisableCallbacksFor<T>();
			this.Exclude<T>();

			return this;
		}

		public virtual ITraverser<TNode> Ignore<T>(Func<T, bool> predicate) where T : class, TNode
		{
			if (predicate == null)
				throw new ArgumentNullException(nameof(predicate));

			this.DisableCallbacksFor(predicate);
			this.Exclude(predicate);

			return this;
		}

		public abstract ITraverser<TNode> OnCanceled(Action action);

		public abstract ITraverser<TNode> OnSuccess(Action action);

		public abstract ITraverser<TNode> Prepare(Action action);

		public abstract ITraverser<TNode> Skip(TNode node);

		public virtual ITraverser<TNode> Skip(IEnumerable<TNode> nodes)
		{
			if (nodes == null)
				throw new ArgumentNullException(nameof(nodes));

			foreach (var node in nodes)
			{
				this.Skip(node);
			}

			return this;
		}

		public abstract ITraverser<TNode> Skip(Func<TNode, bool> predicate);

		public virtual ITraverser<TNode> Skip<T>() where T : class, TNode
		{
			return this.Skip(x => x is T);
		}

		public virtual ITraverser<TNode> Skip<T>(Func<T, bool> predicate) where T : class, TNode
		{
			if (predicate == null)
				throw new ArgumentNullException(nameof(predicate));

			Func<TNode, bool> wrapper = node =>
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

		public abstract ITraverser<TNode> Use(ICandidateSelector<TNode> selector);

		public virtual ITraverser<TNode> Use(TraversalMode mode)
		{
			ICandidateSelector<TNode> candidateSelector;

			switch (mode)
			{
				case TraversalMode.DepthFirst:
					candidateSelector = new DepthFirstSelector<TNode>();
					break;

				case TraversalMode.BreadthFirst:
					candidateSelector = new BreadthFirstSelector<TNode>();
					break;

				case TraversalMode.DefaultComparer:

					var comparer = Comparer<TNode>.Default;
					var inverseComparer = new InverseComparer<TNode>(comparer);

					candidateSelector = new DefaultCandidateSelector<TNode>(inverseComparer);

					break;

				default:
					throw new NotImplementedException($"No candidate selector implementation for traversal mode '{mode}' exists.");
			}

			return this.Use(candidateSelector);
		}

		public abstract ITraverser<TNode> Use(CancellationToken cancellationToken, bool throwException = true);

		public virtual ITraverser<TNode> Use(IComparer<TNode> comparer, bool ascending = false)
		{
			IComparer<TNode> finalComparer = ascending
				? comparer
				: new InverseComparer<TNode>(comparer);

			var selector = new DefaultCandidateSelector<TNode>(finalComparer);
			return this.Use(selector);
		}

		public virtual ITraverser<TNode> WithAction(Action action)
		{
			if (action == null)
				throw new ArgumentNullException(nameof(action));

			Action<TNode> wrapper = node => action.Invoke();
			return this.WithAction(wrapper);
		}

		public abstract ITraverser<TNode> WithAction(Action<TNode> action);

		public virtual ITraverser<TNode> WithAction<T>(Action<T> action) where T : class, TNode
		{
			if (action == null)
				throw new ArgumentNullException(nameof(action));

			Action<TNode> wrapper = node =>
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
