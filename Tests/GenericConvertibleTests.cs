using Bertiooo.Traversal;
using Tests.Fixtures;
using Tests.Model;

namespace Tests
{
	/// <summary>
	/// Tests extensions on <see cref="ITraversalConvertible{TAdapter, TConvertible}"/> 
	/// from static class <see cref="GenericTraversalConvertibleExtensions"/>.
	/// </summary>
	public class GenericConvertibleTests : IClassFixture<GenericConvertibleFixture>
	{
		private readonly GenericConvertibleFixture fixture;

		public GenericConvertibleTests(GenericConvertibleFixture fixture)
		{
			this.fixture = fixture;
		}

		[Fact]
		public void ExtensionsPerformNodeAnalysis()
		{
			var root = this.fixture.Root;
			var child = root.Children.First();
			var secondChild = root.Children.ElementAt(1);
			var grandchild = child.Children.First();

			Assert.True(root.Parent() == null);
			Assert.True(root.Children().Any());
			Assert.True(root.IsRoot());
			Assert.True(root.IsInnerNode());
			Assert.False(root.IsLeaf());
			Assert.False(root.HasParent());
			Assert.True(root.HasChildren());
			Assert.False(root.HasSiblings());
			Assert.Equal(0, root.GetLevel());
			Assert.Equal(2, root.GetMaxDepth());
			Assert.False(root.IsChildOf(child));
			Assert.True(root.IsParentOf(child));
			Assert.True(root.IsAncestorOf(grandchild));
			Assert.False(root.IsDescendantOf(grandchild));

			Assert.True(child.Parent() != null);
			Assert.True(child.Children().Any());
			Assert.False(child.IsRoot());
			Assert.True(child.IsInnerNode());
			Assert.False(child.IsLeaf());
			Assert.True(child.HasParent());
			Assert.True(child.HasChildren());
			Assert.True(child.HasSiblings());
			Assert.Equal(1, child.GetLevel());
			Assert.Equal(1, child.GetMaxDepth());
			Assert.True(child.IsChildOf(root));
			Assert.True(child.IsParentOf(grandchild));
			Assert.True(child.IsSiblingOf(secondChild));
			Assert.True(child.IsAncestorOf(grandchild));
			Assert.True(child.IsDescendantOf(root));

			Assert.True(grandchild.Parent() != null);
			Assert.False(grandchild.Children().Any());
			Assert.False(grandchild.IsRoot());
			Assert.False(grandchild.IsInnerNode());
			Assert.True(grandchild.IsLeaf());
			Assert.True(grandchild.HasParent());
			Assert.False(grandchild.HasChildren());
			Assert.True(grandchild.HasSiblings());
			Assert.Equal(2, grandchild.GetLevel());
			Assert.Equal(0, grandchild.GetMaxDepth());
			Assert.True(grandchild.IsChildOf(child));
			Assert.False(grandchild.IsParentOf(child));
			Assert.False(grandchild.IsSiblingOf(secondChild));
			Assert.False(grandchild.IsAncestorOf(root));
			Assert.True(grandchild.IsDescendantOf(root));
		}

		[Fact]
		public void ExtensionsRetrieveRelatedNodes()
		{
			var root = this.fixture.Root;
			var firstChild = root.Children.First();
			var secondChild = root.Children.ElementAt(1);
			var grandchild = firstChild.Children.First();

			Assert.Equal(root, grandchild.Root());

			Assert.Equal(new GenericConvertible[] { root }, root.WithParent());
			Assert.Equal(new GenericConvertible[] { grandchild, grandchild.Parent }, grandchild.WithParent());

			var rootWithChildren = new List<GenericConvertible>() { root };
			rootWithChildren.AddRange(root.Children);

			Assert.Equal(rootWithChildren, root.WithChildren());

			var child = root.Children.First();
			var siblings = root.Children.Where(x => Equals(x, child) == false);

			Assert.Equal(siblings, child.Siblings());
			Assert.Equal(root.Children, child.WithSiblings());

			Assert.NotEqual(root.Children, secondChild.WithSiblings()); // order differs

			var descendants = new List<GenericConvertible>() { root };
			descendants.AddRange(root.Children);
			descendants.AddRange(firstChild.Children);
			descendants.AddRange(secondChild.Children);

			Assert.Equal(descendants, root.WithDescendants(TraversalMode.BreadthFirst));

			descendants.Remove(root);
			Assert.Equal(descendants, root.Descendants(TraversalMode.BreadthFirst));

			var ancestors = new List<GenericConvertible>() { grandchild, firstChild, root };
			Assert.Equal(ancestors, grandchild.WithAncestors());

			ancestors.Remove(grandchild);
			Assert.Equal(ancestors, grandchild.Ancestors());

			var innerNodes = new List<GenericConvertible>() { firstChild, secondChild };
			innerNodes.Reverse();

			Assert.Equal(innerNodes, root.InnerNodes());

			var leaves = new List<GenericConvertible>();
			leaves.AddRange(firstChild.Children);
			leaves.AddRange(secondChild.Children);
			leaves.Reverse();

			Assert.Equal(leaves, root.Leaves());
		}
	}
}
