using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using Bertiooo.Traversal.Selectors;
using System.Linq;

namespace Bertiooo.Traversal.Traverser
{
	internal class TraversableTraverser<TNode> : ITraverser<TNode>
		where TNode : class, IChildrenProvider<TNode>
	{
		private readonly TNode root;

		public TraversableTraverser(TNode root)
		{
			this.root = root;
		}

		#region Properties

		protected TNode Root => root;

		protected Action Preparation { get; set; }

		protected Action Finalization { get; set; }

		protected Action CanceledCallback { get; set; }

		protected Func<Exception, bool> FailureCallback { get; set; }

		protected Action SuccessCallback { get; set; }

		protected Action<TNode> Callbacks { get; set; }

		protected bool ThrowIfCancellationRequested { get; set; }

		protected CancellationToken CancellationToken { get; set; } = CancellationToken.None;

		protected IList<Func<TNode, bool>> CancelPredicates { get; set; }

		protected IList<TNode> SkipNodes { get; set; }

		protected IList<Func<TNode, bool>> SkipPredicates { get; set; }

		protected IList<TNode> DisabledNodes { get; set; }

		protected IList<Func<TNode, bool>> DisabledPredicates { get; set; }

		protected IList<TNode> ExcludeNodes { get; set; }

		protected IList<Func<TNode, bool>> ExcludePredicates { get; set; }

		protected ICandidateSelector<TNode> Selector { get; set; } = new DepthFirstSelector<TNode>();

		#endregion

		#region Methods

		protected virtual void AddToSelector(TNode node)
		{
			if (this.SkipNodes != null && this.SkipNodes.Contains(node))
				return;

			if (this.SkipPredicates != null && this.SkipPredicates.Any(x => x.Invoke(node)))
				return;

			this.Selector.Add(node);
		}

		protected virtual void PrepareTraversal()
		{
			this.Selector.Reset();
			this.AddToSelector(this.root);

			this.Preparation?.Invoke();
		}

		protected virtual void FinishTraversal()
		{
			this.Finalization?.Invoke();
		}

		protected virtual void VisitNode(TNode node)
		{
			if ((this.DisabledNodes == null || this.DisabledNodes.Contains(node) == false) &&
				(this.DisabledPredicates == null || this.DisabledPredicates.Any(x => x.Invoke(node)) == false))
			{
				this.Callbacks?.Invoke(node);
			}

			foreach (var child in node.Children)
			{
				this.AddToSelector(child);
			}
		}

		protected virtual bool CheckForCancellation(TNode node)
		{
			if (this.CancellationToken.IsCancellationRequested)
			{
				this.CanceledCallback?.Invoke();

				if (this.ThrowIfCancellationRequested)
				{
					this.CancellationToken.ThrowIfCancellationRequested();
				}
				else
				{
					return true;
				}
			}

			if (this.CancelPredicates != null && this.CancelPredicates.Any(x => x.Invoke(node)))
			{
				this.CanceledCallback?.Invoke();
				return true;
			}

			return false;
		}

		public void Execute()
		{
			var enumerator = this.GetNodes().GetEnumerator();
			while (enumerator.MoveNext());
		}

		public Task ExecuteAsync()
		{
			return Task.Factory.StartNew(this.Execute, this.CancellationToken);
		}

		public IEnumerable<TNode> GetNodes()
		{
			try
			{
				try
				{
					this.PrepareTraversal();
				}
				catch (Exception e)
				{
					var handled = this.FailureCallback?.Invoke(e);

					if (handled.HasValue == false || handled.Value == false)
						throw;
				}

				while (this.Selector.HasItems)
				{
					TNode node = null;
					bool includeNode = true;

					try
					{
						node = this.Selector.Next();

						includeNode = (this.ExcludeNodes == null || this.ExcludeNodes.Contains(node) == false) &&
							(this.ExcludePredicates == null || this.ExcludePredicates.Any(x => x.Invoke(node)) == false);

						if (this.CheckForCancellation(node))
							break;

						this.VisitNode(node);
					}
					catch (Exception e)
					{
						var handled = this.FailureCallback?.Invoke(e);

						if (handled.HasValue == false || handled.Value == false)
							throw;
					}

					if (includeNode)
					{
						yield return node;
					}
				}

				this.SuccessCallback?.Invoke();
			}
			finally
			{
				this.FinishTraversal();
			}
		}

		public Task<IList<TNode>> GetNodesAsync()
		{
			return Task<IList<TNode>>.Factory.StartNew(() => this.GetNodes().ToList(), this.CancellationToken);
		}

		public ITraverser<TNode> DisableCallbacksFor(TNode node)
		{
			if(this.DisabledNodes == null)
			{
				this.DisabledNodes = new List<TNode>();
			}

			this.DisabledNodes.Add(node);
			return this;
		}

		public ITraverser<TNode> DisableCallbacksFor(IEnumerable<TNode> nodes)
		{
			if (this.DisabledNodes == null)
			{
				this.DisabledNodes = new List<TNode>();
			}

			foreach (var node in nodes)
			{
				this.DisabledNodes.Add(node);
			}

			return this;
		}

		public ITraverser<TNode> DisableCallbacksFor(Func<TNode, bool> predicate)
		{
			if (predicate == null)
				throw new ArgumentNullException(nameof(predicate));

			if (this.DisabledPredicates == null)
			{
				this.DisabledPredicates = new List<Func<TNode, bool>>();
			}

			this.DisabledPredicates.Add(predicate);
			return this;
		}

		public ITraverser<TNode> DisableCallbacksFor<T>() where T : class, TNode
		{
			return this.DisableCallbacksFor(x => x is T);
		}

		public ITraverser<TNode> DisableCallbacksFor<T>(Func<T, bool> predicate)
			where T : class, TNode
		{
			if (predicate == null)
				throw new ArgumentNullException(nameof(predicate));

			Func<TNode, bool> wrapper = node =>
			{
				var derivative = node as T;

				if(derivative != null)
				{
					return predicate.Invoke(derivative);
				}

				return false;
			};

			return this.DisableCallbacksFor(wrapper);
		}

		public ITraverser<TNode> Exclude(TNode node)
		{
			if(this.ExcludeNodes == null)
			{
				this.ExcludeNodes = new List<TNode>();
			}

			this.ExcludeNodes.Add(node);
			return this;
		}

		public ITraverser<TNode> Exclude(IEnumerable<TNode> nodes)
		{
			if (this.ExcludeNodes == null)
			{
				this.ExcludeNodes = new List<TNode>();
			}

			foreach (var node in nodes)
			{
				this.ExcludeNodes.Add(node);
			}

			return this;
		}

		public ITraverser<TNode> Exclude(Func<TNode, bool> predicate)
		{
			if (predicate == null)
				throw new ArgumentNullException(nameof(predicate));

			if (this.ExcludePredicates == null)
			{
				this.ExcludePredicates = new List<Func<TNode, bool>>();
			}

			this.ExcludePredicates.Add(predicate);
			return this;
		}

		public ITraverser<TNode> Exclude<T>() where T : class, TNode
		{
			return this.Exclude(x => x is T);
		}

		public ITraverser<TNode> Exclude<T>(Func<T, bool> predicate) where T : class, TNode
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

		public ITraverser<TNode> Finish(Action action)
		{
			this.Finalization += action;
			return this;
		}

		public ITraverser<TNode> Ignore(TNode node)
		{
			this.DisableCallbacksFor(node);
			this.Exclude(node);

			return this;
		}

		public ITraverser<TNode> Ignore(IEnumerable<TNode> nodes)
		{
			this.DisableCallbacksFor(nodes);
			this.Exclude(nodes);

			return this;
		}

		public ITraverser<TNode> Ignore(Func<TNode, bool> predicate)
		{
			if (predicate == null)
				throw new ArgumentNullException(nameof(predicate));

			this.DisableCallbacksFor(predicate);
			this.Exclude(predicate);

			return this;
		}

		public ITraverser<TNode> Ignore<T>() where T : class, TNode
		{
			this.DisableCallbacksFor<T>();
			this.Exclude<T>();

			return this;
		}

		public ITraverser<TNode> Ignore<T>(Func<T, bool> predicate) where T : class, TNode
		{
			if (predicate == null)
				throw new ArgumentNullException(nameof(predicate));

			this.DisableCallbacksFor(predicate);
			this.Exclude(predicate);

			return this;
		}

		public ITraverser<TNode> Skip(TNode node)
		{
			if(this.SkipNodes == null)
			{
				this.SkipNodes = new List<TNode>();
			}

			this.SkipNodes.Add(node);
			return this;
		}

		public ITraverser<TNode> Skip(IEnumerable<TNode> nodes)
		{
			if (this.SkipNodes == null)
			{
				this.SkipNodes = new List<TNode>();
			}

			foreach (var node in nodes)
			{
				this.SkipNodes.Add(node);
			}

			return this;
		}

		public ITraverser<TNode> Skip(Func<TNode, bool> predicate)
		{
			if (predicate == null)
				throw new ArgumentNullException(nameof(predicate));

			if (this.SkipPredicates == null)
			{
				this.SkipPredicates = new List<Func<TNode, bool>>();
			}

			this.SkipPredicates.Add(predicate);
			return this;
		}

		public ITraverser<TNode> Skip<T>() where T : class, TNode
		{
			return this.Skip(x => x is T);
		}

		public ITraverser<TNode> Skip<T>(Func<T, bool> predicate) where T : class, TNode
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

		public ITraverser<TNode> Use(ICandidateSelector<TNode> selector)
		{
			if (selector == null)
				throw new ArgumentNullException(nameof(selector));

			this.Selector = selector;
			return this;
		}

		public ITraverser<TNode> Use(TraversalMode mode)
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

				default:
					throw new NotImplementedException($"No candidate selector implementation for traversal mode '{mode}' exists.");
			}

			return this.Use(candidateSelector);
		}

		public ITraverser<TNode> Use(CancellationToken cancellationToken, bool throwException = true)
		{
			this.CancellationToken = cancellationToken;
			this.ThrowIfCancellationRequested = throwException;

			return this;
		}

		public ITraverser<TNode> WithAction(Action action)
		{
			if (action == null)
				throw new ArgumentNullException(nameof(action));

			Action<TNode> wrapper = node => action.Invoke();
			return this.WithAction(wrapper);
		}

		public ITraverser<TNode> WithAction(Action<TNode> action)
		{
			this.Callbacks += action;
			return this;
		}

		public ITraverser<TNode> WithAction<T>(Action<T> action) 
			where T : class, TNode
		{
			if (action == null)
				throw new ArgumentNullException(nameof(action));

			Action<TNode> wrapper = node =>
			{
				var derivative = node as T;

				if(derivative != null)
				{
					action.Invoke(derivative);
				}
			};
			
			return this.WithAction(wrapper);
		}

		public ITraverser<TNode> Prepare(Action action)
		{
			this.Preparation += action;
			return this;
		}

		public ITraverser<TNode> OnCanceled(Action action)
		{
			this.CanceledCallback += action;
			return this;
		}

		public ITraverser<TNode> OnSuccess(Action action)
		{
			this.SuccessCallback += action;
			return this;
		}

		public ITraverser<TNode> OnFailure<T>(Func<T, bool> action) where T : Exception
		{
			if (action == null)
				throw new ArgumentNullException(nameof(action));

			this.FailureCallback += (Exception e) =>
			{
				var derivation = e as T;

				if (derivation != null)
					return action.Invoke(derivation);

				return false;
			};

			return this;
		}

		public ITraverser<TNode> CancelIf(Func<bool> predicate)
		{
			if (predicate == null)
				throw new ArgumentNullException(nameof(predicate));

			Func<TNode, bool> wrapper = node => predicate.Invoke();
			return this.CancelIf(wrapper);
		}

		public ITraverser<TNode> CancelIf(Func<TNode, bool> predicate)
		{
			if (predicate == null)
				throw new ArgumentNullException(nameof(predicate));

			if (this.CancelPredicates == null)
			{
				this.CancelPredicates = new List<Func<TNode, bool>>();
			}

			this.CancelPredicates.Add(predicate);
			return this;
		}

		public ITraverser<TNode> CancelIf<T>(Func<T, bool> predicate) where T : class, TNode
		{
			if (predicate == null)
				throw new ArgumentNullException(nameof(predicate));

			Func<TNode, bool> wrapper = node =>
			{
				var derivative = node as T;

				if(derivative != null)
				{
					return predicate.Invoke(derivative);
				}

				return false;
			};

			return this.CancelIf(wrapper);
		}

		#endregion
	}
}
