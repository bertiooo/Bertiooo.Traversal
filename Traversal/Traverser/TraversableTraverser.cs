using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using Bertiooo.Traversal.Selectors;
using System.Linq;

namespace Bertiooo.Traversal.Traverser
{
	public class TraversableTraverser<TNode> : AbstractTraverser<TNode>
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

		protected IList<Func<Exception, TNode, bool>> FailureCallbacks { get; set; }

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

		#region Protected Methods

		protected virtual void AddToSelector(TNode node)
		{
			if (this.SkipNodes != null && this.SkipNodes.Contains(node))
				return;

			if (this.SkipPredicates != null && this.SkipPredicates.Any(x => x.Invoke(node)))
				return;

			this.Selector.Add(node);
		}

		protected virtual bool CheckForCancellation(TNode node)
		{
			if (this.CancellationToken.IsCancellationRequested)
			{
				if (this.ThrowIfCancellationRequested)
				{
					this.CanceledCallback?.Invoke();
					this.CancellationToken.ThrowIfCancellationRequested();
				}
				else
				{
					return true;
				}
			}

			if (this.CancelPredicates != null && this.CancelPredicates.Any(x => x.Invoke(node)))
			{
				return true;
			}

			return false;
		}

		protected virtual bool AreCallbacksEnabledFor(TNode node)
		{
			return (this.DisabledNodes == null || this.DisabledNodes.Contains(node) == false) &&
				(this.DisabledPredicates == null || this.DisabledPredicates.Any(x => x.Invoke(node)) == false);
		}

		protected virtual bool IsNodeIncluded(TNode node)
		{
			return (this.ExcludeNodes == null || this.ExcludeNodes.Contains(node) == false) &&
				(this.ExcludePredicates == null || this.ExcludePredicates.Any(x => x.Invoke(node)) == false);
		}

		#endregion

		#region Public Methods

		public override ITraverser<TNode> CancelIf(Func<TNode, bool> predicate)
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

		public override ITraverser<TNode> Catch(Func<Exception, TNode, bool> action)
		{
			if (action == null)
				throw new ArgumentNullException(nameof(action));

			if (this.FailureCallbacks == null)
			{
				this.FailureCallbacks = new List<Func<Exception, TNode, bool>>();
			}

			this.FailureCallbacks.Add(action);
			return this;
		}

		public override ITraverser<TNode> DisableCallbacksFor(TNode node)
		{
			if (this.DisabledNodes == null)
			{
				this.DisabledNodes = new List<TNode>();
			}

			this.DisabledNodes.Add(node);
			return this;
		}

		public override ITraverser<TNode> DisableCallbacksFor(Func<TNode, bool> predicate)
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

		public override ITraverser<TNode> Exclude(TNode node)
		{
			if (this.ExcludeNodes == null)
			{
				this.ExcludeNodes = new List<TNode>();
			}

			this.ExcludeNodes.Add(node);
			return this;
		}

		public override ITraverser<TNode> Exclude(Func<TNode, bool> predicate)
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

		public override void Execute()
		{
			var enumerator = this.GetNodes().GetEnumerator();
			while (enumerator.MoveNext());
		}

		public override Task ExecuteAsync()
		{
			return Task.Factory.StartNew(this.Execute, this.CancellationToken);
		}

		public override ITraverser<TNode> Finish(Action action)
		{
			this.Finalization += action;
			return this;
		}

		public override IEnumerable<TNode> GetNodes()
		{
			try
			{
				this.Preparation?.Invoke();

				var canceled = false;

				this.Selector.Reset();
				this.AddToSelector(this.root);

				while (this.Selector.HasItems)
				{
					var node = this.Selector.Next();

					if (this.CheckForCancellation(node))
					{
						canceled = true;
						break;
					}

					if (this.AreCallbacksEnabledFor(node))
					{
						try
						{
							this.Callbacks?.Invoke(node);
						}
						catch (Exception e)
						{
							if (this.FailureCallbacks == null)
								throw;

							bool throwException = true;

							foreach(var callback in this.FailureCallbacks)
							{
								var handled = callback.Invoke(e, node);

								if (handled)
									throwException = false;
							}

							if (throwException)
								throw;
						}
					}

					foreach (var child in node.Children)
					{
						this.AddToSelector(child);
					}

					if (this.IsNodeIncluded(node))
					{
						yield return node;
					}
				}

				if(canceled)
				{
					this.CanceledCallback?.Invoke();
				}
				else
				{
					this.SuccessCallback?.Invoke();
				}
			}
			finally
			{
				this.Finalization?.Invoke();
			}
		}

		public override Task<IList<TNode>> GetNodesAsync()
		{
			return Task<IList<TNode>>.Factory.StartNew(() => this.GetNodes().ToList(), this.CancellationToken);
		}

		public override ITraverser<TNode> OnCanceled(Action action)
		{
			this.CanceledCallback += action;
			return this;
		}

		public override ITraverser<TNode> OnSuccess(Action action)
		{
			this.SuccessCallback += action;
			return this;
		}

		public override ITraverser<TNode> Prepare(Action action)
		{
			this.Preparation += action;
			return this;
		}

		public override ITraverser<TNode> Skip(TNode node)
		{
			if(this.SkipNodes == null)
			{
				this.SkipNodes = new List<TNode>();
			}

			this.SkipNodes.Add(node);
			return this;
		}

		public override ITraverser<TNode> Skip(Func<TNode, bool> predicate)
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

		public override ITraverser<TNode> Use(ICandidateSelector<TNode> selector)
		{
			if (selector == null)
				throw new ArgumentNullException(nameof(selector));

			this.Selector = selector;
			return this;
		}

		public override ITraverser<TNode> Use(CancellationToken cancellationToken, bool throwException = true)
		{
			this.CancellationToken = cancellationToken;
			this.ThrowIfCancellationRequested = throwException;

			return this;
		}

		public override ITraverser<TNode> WithAction(Action<TNode> action)
		{
			this.Callbacks += action;
			return this;
		}

		#endregion

		#endregion
	}
}
