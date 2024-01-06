using Bertiooo.Traversal.Selectors;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Bertiooo.Traversal.Traverser
{
	public interface IAdapterTraverser<TAdapter, TConvertible>
		where TAdapter : IInstanceProvider<TConvertible>
	{
		/// <inheritdoc cref="ITraverser{TAdapter}.CancelIf(Func{TAdapter, bool})" />
		IAdapterTraverser<TAdapter, TConvertible> CancelIf(Func<TConvertible, bool> predicate);

		/// <inheritdoc cref="ITraverser{TAdapter}.DisableCallbacksFor(TAdapter)" />
		IAdapterTraverser<TAdapter, TConvertible> DisableCallbacksFor(TConvertible node);

		/// <inheritdoc cref="ITraverser{TAdapter}.DisableCallbacksFor(IEnumerable{TAdapter})" />
		IAdapterTraverser<TAdapter, TConvertible> DisableCallbacksFor(IEnumerable<TConvertible> nodes);

		/// <inheritdoc cref="ITraverser{TAdapter}.DisableCallbacksFor(Func{TAdapter, bool})" />
		IAdapterTraverser<TAdapter, TConvertible> DisableCallbacksFor(Func<TConvertible, bool> predicate);

		/// <inheritdoc cref="ITraverser{TAdapter}.Exclude(TAdapter)" />
		IAdapterTraverser<TAdapter, TConvertible> Exclude(TConvertible node);

		/// <inheritdoc cref="ITraverser{TAdapter}.Exclude(IEnumerable{TAdapter})" />
		IAdapterTraverser<TAdapter, TConvertible> Exclude(IEnumerable<TConvertible> nodes);

		/// <inheritdoc cref="ITraverser{TAdapter}.Exclude(Func{TAdapter, bool})" />
		IAdapterTraverser<TAdapter, TConvertible> Exclude(Func<TConvertible, bool> predicate);
		void Execute();

		/// <inheritdoc cref="ITraverser{TAdapter}.ExecuteAsync" />
		Task ExecuteAsync();

		/// <inheritdoc cref="ITraverser{TAdapter}.TraversableTraverser(Action)" />
		IAdapterTraverser<TAdapter, TConvertible> Finish(Action action);

		/// <inheritdoc cref="ITraverser{TAdapter}.GetNodes" />
		IEnumerable<TConvertible> GetNodes();

		/// <inheritdoc cref="ITraverser{TAdapter}.GetNodesAsync" />
		Task<IList<TConvertible>> GetNodesAsync();

		/// <inheritdoc cref="ITraverser{TAdapter}.Ignore(TAdapter)" />
		IAdapterTraverser<TAdapter, TConvertible> Ignore(TConvertible node);

		/// <inheritdoc cref="ITraverser{TAdapter}.Ignore(IEnumerable{TAdapter})" />
		IAdapterTraverser<TAdapter, TConvertible> Ignore(IEnumerable<TConvertible> nodes);

		/// <inheritdoc cref="ITraverser{TAdapter}.Ignore(Func{TAdapter, bool})" />
		IAdapterTraverser<TAdapter, TConvertible> Ignore(Func<TConvertible, bool> predicate);

		/// <inheritdoc cref="ITraverser{TAdapter}.OnCanceled(Action)" />
		IAdapterTraverser<TAdapter, TConvertible> OnCanceled(Action action);

		/// <inheritdoc cref="ITraverser{TAdapter}.OnFailure{T}(Func{T, bool})" />
		IAdapterTraverser<TAdapter, TConvertible> OnFailure<T>(Func<T, bool> action)
			where T : Exception;

		/// <inheritdoc cref="ITraverser{TAdapter}.OnSuccess(Action)" />
		IAdapterTraverser<TAdapter, TConvertible> OnSuccess(Action action);

		/// <inheritdoc cref="ITraverser{TAdapter}.Prepare(Action)" />
		IAdapterTraverser<TAdapter, TConvertible> Prepare(Action action);

		/// <inheritdoc cref="ITraverser{TAdapter}.Skip(TAdapter)" />
		IAdapterTraverser<TAdapter, TConvertible> Skip(TConvertible node);

		/// <inheritdoc cref="ITraverser{TAdapter}.Skip(IEnumerable{TAdapter})" />
		IAdapterTraverser<TAdapter, TConvertible> Skip(IEnumerable<TConvertible> nodes);

		/// <inheritdoc cref="ITraverser{TAdapter}.Skip(Func{TAdapter, bool})" />
		IAdapterTraverser<TAdapter, TConvertible> Skip(Func<TConvertible, bool> predicate);

		/// <inheritdoc cref="ITraverser{TAdapter}.Use(ICandidateSelector{TAdapter})" />
		IAdapterTraverser<TAdapter, TConvertible> Use(ICandidateSelector<TAdapter> selector);

		/// <inheritdoc cref="ITraverser{TAdapter}.Use(TraversalMode)" />
		IAdapterTraverser<TAdapter, TConvertible> Use(TraversalMode mode);

		/// <inheritdoc cref="ITraverser{TAdapter}.Use(CancellationToken, bool)" />
		IAdapterTraverser<TAdapter, TConvertible> Use(CancellationToken cancellationToken, bool throwException = true);

		/// <inheritdoc cref="ITraverser{TAdapter}.WithAction(Action)" />
		IAdapterTraverser<TAdapter, TConvertible> WithAction(Action action);

		/// <inheritdoc cref="ITraverser{TAdapter}.WithAction(Action{TAdapter})" />
		IAdapterTraverser<TAdapter, TConvertible> WithAction(Action<TConvertible> action);

		/// <inheritdoc cref="ITraverser{TAdapter}.WithAction{T}(Action{T})" />
		IAdapterTraverser<TAdapter, TConvertible> WithAction<T>(Action<T> action)
			where T : class, TConvertible;
	}
}
