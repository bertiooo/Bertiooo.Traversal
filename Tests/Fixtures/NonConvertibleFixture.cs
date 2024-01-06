using Tests.Model;

namespace Tests
{
    public class NonConvertibleFixture
    {
        private readonly NonConvertible root;

        public NonConvertibleFixture()
        {
            var root = new NonConvertible() { Name = "Root" };

			var rootChildren = new List<NonConvertible>();
            root.Children = rootChildren;

            for (var i = 0; i < 2; i++)
            {
                var inner = new NonConvertible() { Parent = root, Name = "Inner " + (i + 1) };
                rootChildren.Add(inner);

                var innerChildren = new List<NonConvertible>();
                inner.Children = innerChildren;

                for (var j = 0; j < 2; j++)
                {
                    var leaf = new NonConvertible() { Parent = inner, Name = "Leaf " + (i * 2 + j + 1) };
                    innerChildren.Add(leaf);
                }
            }

            this.root = root;
        }

        public NonConvertible Root => root;
    }
}
