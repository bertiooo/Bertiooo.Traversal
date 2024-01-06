using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using Bertiooo.Traversal.Selectors;

namespace Bertiooo.Traversal.Traverser
{
	public interface ITraverser<TNode>
	{
		/// <summary>
		/// Cancels the complete traversal if the predicate returns true. Will be called each time a node gets visited.
		/// </summary>
		ITraverser<TNode> CancelIf(Func<TNode, bool> predicate);

		/// <summary>
		/// With this method you can define nodes for which the callbacks installed with <see cref="WithAction(Action)"/> won't be invoked. 
		/// Still, the nodes will be traversed and be included in the <see cref="IEnumerable{TNode}"/> returned by <see cref="GetNodes"/>.
		/// </summary>
		ITraverser<TNode> DisableCallbacksFor(TNode node);

		/// <summary>
		/// With this method you can define nodes for which the callbacks installed with <see cref="WithAction(Action)"/> won't be invoked. 
		/// Still, the nodes will be traversed and be included in the <see cref="IEnumerable{TNode}"/> returned by <see cref="GetNodes"/>.
		/// </summary>
		ITraverser<TNode> DisableCallbacksFor(IEnumerable<TNode> nodes);

		/// <summary>
		/// With this method you can define nodes for which the callbacks installed with <see cref="WithAction(Action)"/> won't be invoked. 
		/// Still, the nodes will be traversed and be included in the <see cref="IEnumerable{TNode}"/> returned by <see cref="GetNodes"/>.
		/// </summary>
		ITraverser<TNode> DisableCallbacksFor(Func<TNode, bool> predicate);

		/// <summary>
		/// This method will cause the defined nodes not to be included in the <see cref="IEnumerable{TNode}"/> returned by <see cref="GetNodes"/>. 
		/// But the traversal still continues on the node's descendants and also the callbacks will be invoked.
		/// </summary>
		ITraverser<TNode> Exclude(TNode node);

		/// <summary>
		/// This method will cause the defined nodes not to be included in the <see cref="IEnumerable{TNode}"/> returned by <see cref="GetNodes"/>. 
		/// But the traversal still continues on the node's descendants and also the callbacks will be invoked.
		/// </summary>
		ITraverser<TNode> Exclude(IEnumerable<TNode> nodes);

		/// <summary>
		/// This method will cause the defined nodes not to be included in the <see cref="IEnumerable{TNode}"/> returned by <see cref="GetNodes"/>. 
		/// But the traversal still continues on the node's descendants and also the callbacks will be invoked.
		/// </summary>
		ITraverser<TNode> Exclude(Func<TNode, bool> predicate);

		void Execute();

		Task ExecuteAsync();

		/// <summary>
		/// The specified action will be called regardless of whether the traversal was canceled or an exception occurred.
		/// </summary>
		ITraverser<TNode> Finish(Action action);

		/// <summary>
		/// Be careful: Invoking this method alone will not cause the traverser
		/// to traverse the complete structure. It will only return the <see cref="IEnumerable{TNode}"/>.
		/// The traversal continues as the <see cref="IEnumerable{TNode}"/> gets iterated. 
		/// When this happens, the callbacks installed with <see cref="WithAction(Action)"/> will be invoked too.
		/// In order to execute the complete traversal, e.g. call <see cref="Enumerable.ToList{TSource}(IEnumerable{TSource})"/> after <see cref="GetNodes"/>.
		/// </summary>
		IEnumerable<TNode> GetNodes();

		Task<IList<TNode>> GetNodesAsync();

		/// <summary>
		/// This method combines <see cref="Exclude(TNode)"/> and <see cref="DisableCallbacksFor(TNode)"/>. 
		/// Thus, the defined nodes won't be included in the <see cref="IEnumerable{TNode}"/> returned by <see cref="GetNodes"/>
		/// and no callbacks will be invoked for them.
		/// </summary>
		ITraverser<TNode> Ignore(TNode node);

		/// <summary>
		/// This method combines <see cref="Exclude(IEnumerable{TNode})"/> and <see cref="DisableCallbacksFor(IEnumerable{TNode})"/>. 
		/// Thus, the defined nodes won't be included in the <see cref="IEnumerable{TNode}"/> returned by <see cref="GetNodes"/>
		/// and no callbacks will be invoked for them.
		/// </summary>
		ITraverser<TNode> Ignore(IEnumerable<TNode> nodes);

		/// <summary>
		/// This method combines <see cref="Exclude(Func{TNode, bool})"/> and <see cref="DisableCallbacksFor(Func{TNode, bool})"/>. 
		/// Thus, the defined nodes won't be included in the <see cref="IEnumerable{TNode}"/> returned by <see cref="GetNodes"/>
		/// and no callbacks will be invoked for them.
		/// </summary>
		ITraverser<TNode> Ignore(Func<TNode, bool> predicate);

		/// <summary>
		/// The specified callback will be invoked when the operation is canceled via cancellation token defined with <see cref="Use(CancellationToken, bool)"/> 
		/// or with <see cref="CancelIf(Func{TNode, bool})"/>.
		/// </summary>
		/// <remarks>
		/// When the token is cancelled before the operation (task) has started, the callback will NOT be invoked.
		/// </remarks>
		ITraverser<TNode> OnCanceled(Action action);

		/// <param name="action">A function returning whether the exception is being handled or should be thrown again.</param>
		ITraverser<TNode> OnFailure<T>(Func<T, bool> action)
			where T : Exception;

		/// <summary>
		/// Defining a success action, which will only be called if and only if no cancellation or exception occurred.
		/// </summary>
		ITraverser<TNode> OnSuccess(Action action);

		/// <summary>
		/// This action will be invoked at the very beginning of the traversal.
		/// </summary>
		ITraverser<TNode> Prepare(Action action);

		/// <summary>
		/// When a node is specified to be skipped, then the node doesn't get added to the candidate selector, 
		/// i.e. the node itself and its descendants won't be traversed at all.
		/// Thus, there will be no callbacks for the node and its descendants and <see cref="GetNodes"/> won't return those nodes.
		/// </summary>
		ITraverser<TNode> Skip(TNode node);

		/// <summary>
		/// When a node is specified to be skipped, then the node doesn't get added to the candidate selector, 
		/// i.e. the node itself and its descendants won't be traversed at all.
		/// Thus, there will be no callbacks for the node and its descendants and <see cref="GetNodes"/> won't return those nodes.
		/// </summary>
		ITraverser<TNode> Skip(IEnumerable<TNode> nodes);

		/// <summary>
		/// When a node is specified to be skipped, then the node doesn't get added to the candidate selector, 
		/// i.e. the node itself and its descendants won't be traversed at all.
		/// Thus, there will be no callbacks for the node and its descendants and <see cref="GetNodes"/> won't return those nodes.
		/// </summary>
		ITraverser<TNode> Skip(Func<TNode, bool> predicate);

		ITraverser<TNode> Use(ICandidateSelector<TNode> selector);

		ITraverser<TNode> Use(TraversalMode mode);

		/// <summary>
		/// Specify a cancellation token with which the traversal can be canceled.
		/// If not specified otherwise by the second parameter, by canceling the traversal an
		/// <see cref="OperationCanceledException"/> will be thrown, which will be evaluated
		/// by the asynchronous task if you use the async methods <see cref="ExecuteAsync"/> and <see cref="GetNodesAsync"/>.
		/// See https://learn.microsoft.com/en-us/dotnet/standard/parallel-programming/task-cancellation for more details.
		/// If you specify not to throw an exception, then the traversal will quietly be canceled.
		/// </summary>
		ITraverser<TNode> Use(CancellationToken cancellationToken, bool throwException = true);

		/// <summary>
		/// The action will be called each time a node is visited.
		/// </summary>
		ITraverser<TNode> WithAction(Action action);

		/// <summary>
		/// The action will be called each time a node is visited.
		/// </summary>
		ITraverser<TNode> WithAction(Action<TNode> action);

		/// <summary>
		/// The action will be called each time a node of the specified type is visited.
		/// </summary>
		ITraverser<TNode> WithAction<T>(Action<T> action)
			where T : class, TNode;
	}
}
