# Bertiooo's Traversal Library

This library provides extension methods to traverse hierarchy structures. It may work for other graph types as well.

If you like this framework, I appreciate any donations on my [PayPal account](https://paypal.me/puigrodr).

## Usage

### Install the nuget package

See [https://www.nuget.org/packages/Bertiooo.Traversal/](https://www.nuget.org/packages/Bertiooo.Traversal/)

### Implement the ITraversable<T> interface

In order to use the extension methods, you need a node class implementing the `ITraversable<T>` interface.
```
public class Node : ITraversable<Node> 
{
  public Node Parent { get; }
  public IEnumerable<Node> Children { get; }
}
```
Sometimes you will find it difficult to implement this interface, either because the property names of your class do not fit
or because the collection type of the children property differs from `IEnumerable<T>`. In this case, it is recommended to use 
an adapter as described in [How to adapt to ITraversable](https://github.com/bertiooo/Bertiooo.Traversal/wiki/How-to-adapt-to-ITraversable).

### Use the extension methods

Here are some examples:

#### Extension methods for node analysis

```
Node node = new Node();

bool result;

result = node.IsRoot();
result = node.IsInnerNode();
result = node.IsLeaf();

result = node.HasParent(); 
result = node.HasChildren();
result = node.HasSiblings();

Node other = new Node();

result = node.IsChildOf(other);
result = node.IsParentOf(other);
result = node.IsSiblingOf(other);
result = node.IsDescendantOf(other);
result = node.IsAncestorOf(other);

int result;

result = node.GetLevel();
result = node.GetMaxDepth();
```

#### Retrieval of related nodes

```
Node result;

result = node.GetRoot();

IEnumerable<Node> result;

result = node.WithParent();
result = node.WithChildren();
result = node.Siblings();
result = node.WithSiblings();
result = node.Descendants();
result = node.WithDescendants();
result = node.Ancestors();
result = node.WithAncestors();
```

#### Traverse extensions

```
Action<Node> action = n => n.DoSomething();

node.Traverse(action); // per default depth first traversal
node.Traverse(action, TraversalMode.BreadthFirst);

ICandidateSelector<Node> customSelector = new CustomCandidateSelector();
node.Traverse(action, customSelector);

CancellationToken token;
await node.TraverseAsync(action, cancellationToken: token);
```

### 3. Use the fluent API

For further control over the traversal process, use the fluent API. 

```
ITraverser<Node> traverser = node.Traverse()
	.Use(TraversalMode.BreadthFirst) // per default depth-first traversal
	.Use(candidateSelector)
	.Use(cancellationToken)
	.Skip(someNode) 
	.Ignore(someNodes)
	.Exclude(tmpNode => tmpNode.IsLeaf())
	.CancelIf(tmpNode => someCondition())
	.WithAction(tmpNode => action(tmpNode))
	.Prepare(action)
	.OnFailure<InvalidOperationException>(e => Debug(e))
	.OnCanceled(action)
	.OnSuccess(action)
	.Finalize(action);
```

See [Difference between Skip, Exclude, Ignore and DisableCallbacksFor](https://github.com/bertiooo/Bertiooo.Traversal/wiki/Difference-between-Skip,-Exclude,-Ignore-and-DisableCallbacksFor) for more information about those methods.

Then you have different options to execute the traversal:

```
// Execute() does not return a value
traverser.Execute();
await traverser.ExecuteAsync();

// be careful: Invoking GetNodes() alone won't cause the traverser to perform the traversal.
// You need to iterate the nodes in order to proceed with the traversal.
IEnumerable<Node> nodes = traverser.GetNodes();

// The async method in contrast will turn the enumerable into a list by full traversal.
IList<Node> nodes = await traverser.GetNodesAsync();
```



	
	

