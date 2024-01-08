using Bertiooo.Traversal;
using Tests.Model;

namespace Tests
{
    public class ConvertibleTests : IClassFixture<ConvertibleFixture>
	{
		private readonly ConvertibleFixture fixture;

		public ConvertibleTests(ConvertibleFixture fixture)
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

			Assert.True(root.Parent == null);
			Assert.True(root.Children.Any());
			Assert.True(root.IsRoot(x => x.Parent));
			Assert.True(root.IsInnerNode(x => x.Children));
			Assert.False(root.IsLeaf(x => x.Children));
			Assert.False(root.HasParent(x => x.Parent));
			Assert.True(root.HasChildren(x => x.Children));
			Assert.False(root.HasSiblings(x => x.Parent, x => x.Children));
			Assert.Equal(0, root.GetLevel(x => x.Parent));
			Assert.Equal(2, root.GetMaxDepth(x => x.Parent, x => x.Children));
			Assert.False(root.IsChildOf(child, x => x.Parent));
			Assert.True(root.IsParentOf(child, x => x.Parent));
			Assert.True(root.IsAncestorOf(grandchild, x => x.Parent));
			Assert.False(root.IsDescendantOf(grandchild, x => x.Parent));

			Assert.True(child.Parent != null);
			Assert.True(child.Children.Any());
			Assert.False(child.IsRoot(x => x.Parent));
			Assert.True(child.IsInnerNode(x => x.Children));
			Assert.False(child.IsLeaf(x => x.Children));
			Assert.True(child.HasParent(x => x.Parent));
			Assert.True(child.HasChildren(x => x.Children));
			Assert.True(child.HasSiblings(x => x.Parent, x => x.Children));
			Assert.Equal(1, child.GetLevel(x => x.Parent));
			Assert.Equal(1, child.GetMaxDepth(x => x.Parent, x => x.Children));
			Assert.True(child.IsChildOf(root, x => x.Parent));
			Assert.True(child.IsParentOf(grandchild, x => x.Parent));
			Assert.True(child.IsSiblingOf(secondChild, x => x.Parent));
			Assert.True(child.IsAncestorOf(grandchild, x => x.Parent));
			Assert.True(child.IsDescendantOf(root, x => x.Parent));

			Assert.True(grandchild.Parent != null);
			Assert.False(grandchild.Children.Any());
			Assert.False(grandchild.IsRoot(x => x.Parent));
			Assert.False(grandchild.IsInnerNode(x => x.Children));
			Assert.True(grandchild.IsLeaf(x => x.Children));
			Assert.True(grandchild.HasParent(x => x.Parent));
			Assert.False(grandchild.HasChildren(x => x.Children));
			Assert.True(grandchild.HasSiblings(x => x.Parent, x => x.Children));
			Assert.Equal(2, grandchild.GetLevel(x => x.Parent));
			Assert.Equal(0, grandchild.GetMaxDepth(x => x.Parent, x => x.Children));
			Assert.True(grandchild.IsChildOf(child, x => x.Parent));
			Assert.False(grandchild.IsParentOf(child, x => x.Parent));
			Assert.False(grandchild.IsSiblingOf(secondChild, x => x.Parent));
			Assert.False(grandchild.IsAncestorOf(root, x => x.Parent));
			Assert.True(grandchild.IsDescendantOf(root, x => x.Parent));
		}

		[Fact]
		public void ExtensionsRetrieveRelatedNodes()
		{
			var root = this.fixture.Root;
			var firstChild = root.Children.First();
			var secondChild = root.Children.ElementAt(1);
			var grandchild = firstChild.Children.First();

			Assert.Equal(root, grandchild.GetRoot(x => x.Parent));

			Assert.Equal(new Convertible[] { root }, root.WithParent(x => x.Parent));
			Assert.Equal(new Convertible[] { grandchild, grandchild.Parent }, grandchild.WithParent(x => x.Parent));

			var rootWithChildren = new List<Convertible>() { root };
			rootWithChildren.AddRange(root.Children);

			Assert.Equal(rootWithChildren, root.WithChildren(x => x.Children));

			var child = root.Children.First();
			var siblings = root.Children.Where(x => Equals(x, child) == false);

			Assert.Equal(siblings, child.Siblings(x => x.Parent, x => x.Children));
			Assert.Equal(root.Children, child.WithSiblings(x => x.Parent, x => x.Children));

			Assert.NotEqual(root.Children, secondChild.WithSiblings(x => x.Parent, x => x.Children)); // order differs

			var descendants = new List<Convertible>() { root };
			descendants.AddRange(root.Children);
			descendants.AddRange(firstChild.Children);
			descendants.AddRange(secondChild.Children);

			Assert.Equal(descendants, root.WithDescendants(x => x.Children, TraversalMode.BreadthFirst));

			descendants.Remove(root);
			Assert.Equal(descendants, root.Descendants(x => x.Children, TraversalMode.BreadthFirst));

			var ancestors = new List<Convertible>() { grandchild, firstChild, root };
			Assert.Equal(ancestors, grandchild.WithAncestors(x => x.Parent));

			ancestors.Remove(grandchild);
			Assert.Equal(ancestors, grandchild.Ancestors(x => x.Parent));
		}
	}
}
