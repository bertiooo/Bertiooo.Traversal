using Tests.Model;

namespace Tests.Fixtures
{
    public class TraversableFixture
    {
        private readonly Traversable root;

        public TraversableFixture()
        {
            var root = new Traversable() { Name = "Root" };

            var rootChildren = new List<Traversable>();
            root.Children = rootChildren;

            for (var i = 0; i < 2; i++)
            {
                var inner = new Traversable() { Parent = root, Name = "Inner " + (i + 1) };
                rootChildren.Add(inner);

                var innerChildren = new List<Traversable>();
                inner.Children = innerChildren;

                for (var j = 0; j < 2; j++)
                {
                    var leaf = new Traversable() { Parent = inner, Name = "Leaf " + (i * 2 + j + 1) };
                    innerChildren.Add(leaf);
                }
            }

            this.root = root;
        }

        public Traversable Root => root;
    }
}
