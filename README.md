# Bertiooo's Traversal Library

This C# lightweight library provides extension methods to traverse hierarchy structures. It targets .NET Standard 2.0.
See [Wiki of its GitHub Repository](https://github.com/bertiooo/Bertiooo.Traversal/wiki) for more details on how to use this library.

## Usage

### Install the nuget package

See [https://www.nuget.org/packages/Bertiooo.Traversal/](https://www.nuget.org/packages/Bertiooo.Traversal/)

### Prepare your node class

There are several ways to make your classes suitable for traversal:

#### Use extension overloads which define how to traverse your nodes.

For example:

```
MyNode node;
var descendants = node.Descendants(n => n.Children);
var ancestors = node.Ancestors(n => n.Parent);
```

#### Create a custom strategy

```
var strategy = new MyTraversalStrategy(); // implements ITraversalStrategy<MyNode>

MyNode node;
var descendants = node.Descendants(strategy);
var ancestors = node.Ancestors(strategy);
```

#### Implement `ITraversable<T>`

```
public class MyNode : ITraversable<MyNode> 
{
  public MyNode Parent { get; }
  public IEnumerable<MyNode> Children { get; }
}
```

#### Declare a strategy with an attribute

```
using Bertiooo.Traversal.Attributes;

[TraversalStrategy(typeof(MyTraversalStrategy))]
public class MyNode 
{
}
```

#### Declare attributes on your node's properties

```
using Bertiooo.Traversal.Attributes;

public class MyNode 
{
	[Parent]
	public MyNode? Parent { get;}

	[Children]	
	public IEnumerable<MyNode> Children { get; }
}
```

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

result = node.Root();

IEnumerable<Node> result;

result = node.WithParent();
result = node.WithChildren();
result = node.Siblings();
result = node.WithSiblings();
result = node.Descendants();
result = node.WithDescendants();
result = node.Ancestors();
result = node.WithAncestors();
result = node.Leaves();
result = node.InnerNodes();
```

#### Traverse extensions

```
Action<Node> action = n => n.DoSomething();

node.Traverse(action); // per default depth first traversal
node.Traverse(action, TraversalMode.BreadthFirst);

CancellationToken token;
await node.TraverseAsync(action, cancellationToken: token);

IComparer<Node> customComparer = new CustomComparer();
node.Traverse(action, comparer);

ICandidateSelector<Node> customSelector = new CustomCandidateSelector();
node.Traverse(action, customSelector);
```


	
	

