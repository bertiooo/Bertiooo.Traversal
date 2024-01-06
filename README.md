Bertiooo's Traversal Library

This library provides extension methods to traverse hierarchy structures. It may work for other graph types as well.

If you like this framework, I appreciate any donations on my PayPal account: paypal.me/puigrodr

Usage

1. Implement the ITraversable<T> interface

In order to use the extension methods, you need a node class implementing the ITraversable<T> interface.

public class Node : ITraversable<Node> 
{
	public Node Parent { get; }
	
	public IEnumerable<Node> Children { get; }
}

Sometimes you will find it difficult to implement this interface, either because the property names of your class do not fit
or because the collection type of the children property differs from IEnumerable<T>. In this case, it is recommended to use 
an adapter as described in How to adapt to ITraversable

2. Use the extension methods

Here are some examples:

For a full list of available extensions, see Extensions Overview

3. Use the fluent API

// Node is your class implementing the interface ITraversable which exposes the properties "Parent" and "Children"
Node node = new Node();

Node parent = node.Parent;
IEnumerable<Node> children = node.Children;

// extension methods for node analysis

bool result;

result = node.IsRoot();
result = node.IsInnerNode();
result = node.IsLeaf();

result = node.HasParent(); 
result = node.HasChildren();
result = node.HasSiblings();

int result;

result = node.GetLevel();
result = node.GetMaxDepth();

result = node.IsChildOf();
result = node.IsParentOf();
result = node.IsSiblingOf();
result = node.IsDescendantOf();
result = node.IsAncestorOf();

// retrieve related nodes

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

// various ways to retrieve descendants

result = node.Descendants(); // per default depth first traversal
result = node.Descendants(TraversalMode.DepthFirst);
result = node.Descendants(TraversalMode.BreadthFirst);

var candidateSelector = new CustomCandidateSelector(); // implements ICandidateSelector<T>
result = node.Descendants(candidateSelector);

// Traverse extensions

Action<Node> action;

node.Traverse(action); // per default depth first traversal
node.Traverse(action, TraversalMode.BreadthFirst);
node.Traverse(action, candidateSelector);

// all traverse methods can be executed asynchronously:
CancellationToken token;
node.TraverseAsync(action, cancellationToken: token);

For further control over the traversal process, use the fluent API. 

ITraverser<Node> traverser = node.Traverse()
	.Use(TraversalMode.BreadthFirst) // per default depth-first traversal
	.Use(candidateSelector)
	.Use(cancellationToken)
	.Skip(someNode) 
	.Skip(someNodes)
	.Skip(tmpNode => tmpNode.IsLeaf())
	.Ignore(someNode)
	.Ignore(someNodes)
	.Ignore(tmpNode => tmpNode.IsLeaf())
	.Exclude(someNode)
	.Exclude(someNodes)
	.Exclude(tmpNode => tmpNode.IsLeaf())
	.CancelIf(tmpNode => someCondition())
	.CancelIf(tmpNode => someOtherCondition(tmpNode))
	.WithAction(tmpNode => action(tmpNode))
	.WithAction(action2)
	.Prepare(action)
	.OnFailure(Action<Exception> action)
	.OnFailure<T>(Action<T> action) where T : Exception
	.OnCanceled(action)
	.OnSuccess(action)
	.Finalize(action);
	
Difference between Skip, Exclude, Ignore and DisableCallbacksFor: 
1. Skip: When a node is specified to be skipped, then the node doesn't get added to the candidate selector, i.e. the node itself and its descendants won't be traversed at all.
Thus, there will be no callbacks for the node and its descendants and the GetNodes won't return those nodes.
2. Exclude: The Exclude(...) method will cause the defined nodes not to be included in the enumerable returned by the GetNodes method. 
But the traversal still continues on the node's descendants and also the callbacks will be invoked. 
3. DisableCallbacksFor: With this method you can define nodes for which the callbacks won't be invoked. Still, the nodes will be traversed and be included in the enumerable 
returned by GetNodes()
4. Ignore: The Ignore method combines the methods Exclude and DisableCallbacksFor. Thus, the defined nodes won't be included in the enumerable 
returned by GetNodes() and no callbacks will be invoked for them.
	
// then you have different options to execute the traversal:

// Execute() does not return a value
traverser.Execute();
await traverser.ExecuteAsync();

// be careful: Invoking GetNodes() alone won't cause the traverser to perform the traversal.
// You need to iterate the nodes in order to proceed with the traversal.
IEnumerable<Node> nodes = traverser.GetNodes();

// The async method in contrast will turn the enumerable into a list by full traversal.
IList<Node> nodes = await traverser.GetNodesAsync();

Adapt to ITraversable

Often times, you will find yourself with a class that does not allow an easy implementation of ITraversable<T>, 
either because the property names for "Parent" and "Children" do not fit or because the collection type of the Children property
differs from IEnumerable<T>. In this case, you can choose between two options:

1. Use the AsTraversable() extension method to define how parent and children of the node should be retrieved.
This will wrap a DefaultTraversableAdapter around your node. 
In order to retrieve the nodes later on, you must use the Instance property of the adapter.

// this requires a separate namespace 
using Bertiooo.Traversal.NonConvertible;

MyNode node;
IEnumerable<MyNode> descendants = node.AsTraversable(n => n.CustomParent, n => n.CustomChildren).Descendants().Select(x => x.Instance);

// or simply use the shortcut:
descendants = node.Descendants(n => n.CustomChildren);

The "non-convertible" extensions are extensions on any class and will be suggested by the code editor on any class.
This can be sort of annoying. Consider using the second method instead.

2. Implement the ITraversalConvertible<TAdapter, TInstance> interface by defining an AsTraversable() method in your node class.

public class MyNode : ITraversalConvertible<DefaultTraversableAdapter<MyNode>, MyNode>
{
	private readonly Lazy<DefaultTraversableAdapter<MyNode>> lazyAdapter;
		
	public MyNode() 
	{
		lazyAdapter = new Lazy<DefaultTraversableAdapter<MyNode>>(
			() => new DefaultTraversableAdapter<MyNode>(this, x => x.CustomParent, x => x.CustomChildren));
	}

	public MyNode? CustomParent { get; set; }

	public IList<MyNode> CustomChildren { get; set; } = new List<MyNode>();

	public DefaultTraversableAdapter<MyNode> AsTraversable()
	{
		return lazyAdapter.Value;
	}
}

You can also define your own adapter by deriving from AbstractTraversableAdapter:

public class MyNodeToTraversableAdapter : AbstractTraversableAdapter<MyNodeToTraversableAdapter, MyNode>
{
	public MyNodeToTraversableAdapter(MyNode node) : base(node)
	{
	}

	protected override MyNode? ParentInstance => this.Instance.CustomParent;

	protected override IEnumerable<MyNode> ChildInstances => this.Instance.CustomChildren;

	protected override MyNodeToTraversableAdapter CreateAdapter(MyNode convertible)
	{
		return new MyNodeToTraversableAdapter(convertible);
	}
}

and return it in the AsTraversable() method:

public class MyNode : ITraversalConvertible<MyNodeToTraversableAdapter, MyNode>
{	
	private readonly Lazy<MyNodeToTraversableAdapter> lazyAdapter;
		
	public MyNode() 
	{
		lazyAdapter = new Lazy<MyNodeToTraversableAdapter>(
			() => new MyNodeToTraversableAdapter(this));
	}

	// ...

	public MyNodeToTraversableAdapter AsTraversable()
	{
		return lazyAdapter.Value;
	}
}

You can then either call the AsTraversable method explicitly:

MyNode node;
IEnumerable<MyNode> descendants = node.AsTraversable().Descendants().Select(x => x.Instance);

.. or simply use the shortcut:

descendants = node.Descendants();



	
	

