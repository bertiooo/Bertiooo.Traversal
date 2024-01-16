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
		ITraverser<TNode> CancelIf(Func<bool> predicate);

		/// <summary>
		/// Cancels the complete traversal if the predicate returns true. Will be called each time a node gets visited.
		/// </summary>
		ITraverser<TNode> CancelIf(Func<TNode, bool> predicate);

		/// <summary>
		/// Cancels the complete traversal if the predicate returns true. 
		/// Will be called each time a node of the specified type gets visited.
		/// </summary>
		ITraverser<TNode> CancelIf<T>(Func<T, bool> predicate)
			where T : class, TNode;

		/// <summary>
		/// Invokes the given action when an exception occurs on an action that has been registered via <see cref="WithAction(Action)"/>. 
		/// Exceptions that occur during traversal will not be catched.
		/// After invoking, the exception will be thrown again.
		/// </summary>
		/// <param name="throwException">Whether to throw the exception after the action has been invoked. Default false.</param>
		ITraverser<TNode> Catch(Action<Exception> action, bool throwException = false);

		/// <summary>
		/// Invokes the given action when an exception occurs on an action that has been registered via <see cref="WithAction(Action)"/>. 
		/// Exceptions that occur during traversal will not be catched.
		/// After invoking, the exception will be thrown again.
		/// </summary>
		/// <param name="action">A callback with the exception and the node on which the exception occurred.</param>
		/// <param name="throwException">Whether to throw the exception after the action has been invoked. Default false.</param>
		ITraverser<TNode> Catch(Action<Exception, TNode> action, bool throwException = false);

		/// <summary>
		/// Invokes the given action when an exception occurs on an action that has been registered via <see cref="WithAction(Action)"/>. 
		/// Exceptions that occur during traversal will not be catched.
		/// After invoking, the exception will be thrown again.
		/// </summary>
		/// <param name="throwException">Whether to throw the exception after the action has been invoked. Default false.</param>
		ITraverser<TNode> Catch<T>(Action<T> action, bool throwException = false) where T : Exception;

		/// <summary>
		/// Invokes the given action when an exception occurs on an action that has been registered via <see cref="WithAction(Action)"/>. 
		/// Exceptions that occur during traversal will not be catched.
		/// After invoking, the exception will be thrown again.
		/// </summary>
		/// <param name="action">A callback with the exception and the node on which the exception occurred.</param>
		/// <param name="throwException">Whether to throw the exception after the action has been invoked. Default false.</param>
		ITraverser<TNode> Catch<T>(Action<T, TNode> action, bool throwException = false) where T : Exception;

		/// <summary>
		/// Invokes the given action when an exception occurs on an action that has been registered via <see cref="WithAction(Action)"/>. 
		/// Exceptions that occur during traversal will not be catched.
		/// </summary>
		/// <param name="action">A function returning whether the exception is being handled (return true) or should be thrown again (return false).</param>
		ITraverser<TNode> Catch(Func<Exception, bool> action);

		/// <summary>
		/// Invokes the given action when an exception occurs on an action that has been registered via <see cref="WithAction(Action)"/>. 
		/// Exceptions that occur during traversal will not be catched.
		/// </summary>
		/// <param name="action">A function returning whether the exception is being handled (return true) or should be thrown again (return false).</param>
		ITraverser<TNode> Catch(Func<Exception, TNode, bool> action);

		/// <summary>
		/// Invokes the given action when an exception occurs on an action that has been registered via <see cref="WithAction(Action)"/>. 
		/// Exceptions that occur during traversal will not be catched.
		/// </summary>
		/// <param name="action">A function returning whether the exception is being handled (return true) or should be thrown again (return false).</param>
		ITraverser<TNode> Catch<T>(Func<T, bool> action)
			where T : Exception;

		/// <summary>
		/// Invokes the given action when an exception occurs on an action that has been registered via <see cref="WithAction(Action)"/>. 
		/// Exceptions that occur during traversal will not be catched.
		/// </summary>
		/// <param name="action">A function returning whether the exception is being handled (return true) or should be thrown again (return false).</param>
		ITraverser<TNode> Catch<T>(Func<T, TNode, bool> action)
			where T : Exception;

		/// <summary>
		/// Creates a deep copy of the traverser instance.
		/// The candidate selector is only cloned if it implements the interface <see cref="ICloneable"/>. Otherwise the reference is copied.
		/// </summary>
		ITraverser<TNode> Clone();

		/// <summary>
		/// With this method you can define nodes for which the callbacks installed with <see cref="WithAction(Action)"/> won't be invoked. 
		/// Still, the nodes will be traversed and be included in the <see cref="IEnumerable{TNode}"/> returned by <see cref="GetNodes"/>.
		/// </summary>
		ITraverser<TNode> DisableCallbacksFor<T>()
			where T : class, TNode;

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
		/// With this method you can define nodes for which the callbacks installed with <see cref="WithAction(Action)"/> won't be invoked. 
		/// Still, the nodes will be traversed and be included in the <see cref="IEnumerable{TNode}"/> returned by <see cref="GetNodes"/>.
		/// </summary>
		ITraverser<TNode> DisableCallbacksFor<T>(Func<T, bool> predicate)
			where T : class, TNode;

		/// <summary>
		/// This method will cause the defined nodes of the specified type not to be included in the <see cref="IEnumerable{TNode}"/> returned by <see cref="GetNodes"/>. 
		/// But the traversal still continues on the node's descendants and also the callbacks will be invoked.
		/// </summary>
		ITraverser<TNode> Exclude<T>() where T : class, TNode;

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

		/// <summary>
		/// This method will cause the defined nodes of the specified type not to be included in the <see cref="IEnumerable{TNode}"/> returned by <see cref="GetNodes"/>. 
		/// But the traversal still continues on the node's descendants and also the callbacks will be invoked.
		/// </summary>
		ITraverser<TNode> Exclude<T>(Func<T, bool> predicate) where T : class, TNode;

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
		/// This method combines <see cref="Exclude{T}()"/> and <see cref="DisableCallbacksFor{T}()"/>. 
		/// Thus, the defined nodes won't be included in the <see cref="IEnumerable{TNode}"/> returned by <see cref="GetNodes"/>
		/// and no callbacks will be invoked for them.
		/// </summary>
		ITraverser<TNode> Ignore<T>() where T : class, TNode;

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
		/// This method combines <see cref="Exclude{T}(Func{T, bool})"/> and <see cref="DisableCallbacksFor{T}(Func{T, bool})"/>. 
		/// Thus, the defined nodes won't be included in the <see cref="IEnumerable{TNode}"/> returned by <see cref="GetNodes"/>
		/// and no callbacks will be invoked for them.
		/// </summary>
		ITraverser<TNode> Ignore<T>(Func<T, bool> predicate) where T : class, TNode;

		/// <summary>
		/// The specified callback will be invoked when the operation is canceled via cancellation token defined with <see cref="Use(CancellationToken, bool)"/> 
		/// or with <see cref="CancelIf(Func{TNode, bool})"/>.
		/// </summary>
		/// <remarks>
		/// When the token is cancelled before the operation (task) has started, the callback will NOT be invoked.
		/// </remarks>
		ITraverser<TNode> OnCanceled(Action action);

		/// <summary>
		/// Defining a success action, which will be called if and only if no cancellation or exception occurred.
		/// </summary>
		ITraverser<TNode> OnSuccess(Action action);

		/// <summary>
		/// This action will be invoked at the very beginning of the traversal.
		/// </summary>
		ITraverser<TNode> Prepare(Action action);

		/// <summary>
		/// Reverses the order the children of a node are added to the candidate selector.
		/// Calling this action twice won't restore the initial order.
		/// </summary>
		ITraverser<TNode> ReverseOrder();

		/// <summary>
		/// When a node is specified to be skipped, then the node doesn't get added to the candidate selector, 
		/// i.e. the node itself and its descendants won't be traversed at all.
		/// Thus, there will be no callbacks for the node and its descendants and <see cref="GetNodes"/> won't return those nodes.
		/// </summary>
		ITraverser<TNode> Skip<T>() where T : class, TNode;

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

		/// <summary>
		/// When a node is specified to be skipped, then the node doesn't get added to the candidate selector, 
		/// i.e. the node itself and its descendants won't be traversed at all.
		/// Thus, there will be no callbacks for the node and its descendants and <see cref="GetNodes"/> won't return those nodes.
		/// </summary>
		ITraverser<TNode> Skip<T>(Func<T, bool> predicate) where T : class, TNode;

		/// <summary>
		/// Define a comparer stating which nodes should be preferred over the other during traversal.
		/// </summary>
		/// <param name="ascending">
		/// When true, the nodes will be retrieved in ascending order. Default false. 
		/// That is, the nodes with the highest values will be traversed first.
		/// </param>
		ITraverser<TNode> Use(IComparer<TNode> comparer, bool ascending = false);

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
