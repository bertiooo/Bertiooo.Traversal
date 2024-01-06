using System.Xml.Linq;
using Tests.Model;

namespace Tests.Fixtures
{
    public class ConvertibleFixture
    {
        private readonly Convertible root;

        public ConvertibleFixture()
        {
            var root = new Convertible() { Name = "Root" };

			var rootChildren = new List<Convertible>();
            root.Children = rootChildren;

            string name;

            for (var i = 0; i < 2; i++)
            {
				name = "Inner " + (i + 1);

                Convertible inner;

				if (i == 0)
				{
					inner = new Convertible() { Parent = root, Name = name };
				}
				else
				{
					inner = new DerivativeConvertible() { Parent = root, Name = name };
				}

                rootChildren.Add(inner);

                var innerChildren = new List<Convertible>();
                inner.Children = innerChildren;

                for (var j = 0; j < 2; j++)
                {
                    name = "Leaf " + (i * 2 + j + 1);

                    Convertible leaf;

                    if(i == 0)
                    {
						leaf = new Convertible() { Parent = inner, Name = name };
					}
                    else
                    {
						leaf = new DerivativeConvertible() { Parent = inner, Name = name };
					}

                    innerChildren.Add(leaf);
                }
            }

            this.root = root;
        }

        public Convertible Root => root;
    }
}
