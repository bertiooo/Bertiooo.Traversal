using System.Xml.Linq;
using Tests.Model;

namespace Tests.Fixtures
{
    public class GenericConvertibleFixture
    {
        private readonly GenericConvertible root;

        public GenericConvertibleFixture()
        {
            var root = new GenericConvertible() { Name = "Root" };

			var rootChildren = new List<GenericConvertible>();
            root.Children = rootChildren;

            string name;

            for (var i = 0; i < 2; i++)
            {
				name = "Inner " + (i + 1);

                GenericConvertible inner;

				if (i == 0)
				{
					inner = new GenericConvertible() { Parent = root, Name = name };
				}
				else
				{
					inner = new DerivativeGenericConvertible() { Parent = root, Name = name };
				}

                rootChildren.Add(inner);

                var innerChildren = new List<GenericConvertible>();
                inner.Children = innerChildren;

                for (var j = 0; j < 2; j++)
                {
                    name = "Leaf " + (i * 2 + j + 1);

                    GenericConvertible leaf;

                    if(i == 0)
                    {
						leaf = new GenericConvertible() { Parent = inner, Name = name };
					}
                    else
                    {
						leaf = new DerivativeGenericConvertible() { Parent = inner, Name = name };
					}

                    innerChildren.Add(leaf);
                }
            }

            this.root = root;
        }

        public GenericConvertible Root => root;
    }
}
