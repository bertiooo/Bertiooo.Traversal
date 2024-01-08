using Tests.Model;

namespace Tests
{
    public class ConvertibleFixture
    {
        private readonly Convertible root;

        public ConvertibleFixture()
        {
            var root = new Convertible() { Name = "Root" };

			var rootChildren = new List<Convertible>();
            root.Children = rootChildren;

            for (var i = 0; i < 2; i++)
            {
                var inner = new Convertible() { Parent = root, Name = "Inner " + (i + 1) };
                rootChildren.Add(inner);

                var innerChildren = new List<Convertible>();
                inner.Children = innerChildren;

                for (var j = 0; j < 2; j++)
                {
                    var leaf = new Convertible() { Parent = inner, Name = "Leaf " + (i * 2 + j + 1) };
                    innerChildren.Add(leaf);
                }
            }

            this.root = root;
        }

        public Convertible Root => root;
    }
}
