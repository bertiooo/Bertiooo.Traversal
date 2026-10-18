using Bertiooo.Traversal;
using Bertiooo.Traversal.Factories;
using Tests.Model;

namespace Tests.Helpers
{
    internal class TheoryData : TheoryData<NodeMetaData>
    {
        public TheoryData() 
        {
            // nodes with implicit strategies

            this.Add<Traversable>(
                true,
                CreateStrategy<Traversable>(),
                n => ((Traversable)n).Parent,
                (n,p) => ((Traversable)n).Parent = (Traversable?)p,
                n => ((Traversable)n).Children,
                (n,c) => ((Traversable)n).Children = c.Cast<Traversable>());

            this.Add<NodeWithTraversalStrategyAttribute>(
                true,
                CreateStrategy<NodeWithTraversalStrategyAttribute>(),
                n => ((NodeWithTraversalStrategyAttribute)n).Parent,
                (n, p) => ((NodeWithTraversalStrategyAttribute)n).Parent = (NodeWithTraversalStrategyAttribute?)p,
                n => ((NodeWithTraversalStrategyAttribute)n).Children,
                (n, c) => ((NodeWithTraversalStrategyAttribute)n).Children = c.Cast<NodeWithTraversalStrategyAttribute>());

            this.Add<NodeWithPropertyAttributes>(
                true,
                CreateStrategy<NodeWithPropertyAttributes>(),
                n => ((NodeWithPropertyAttributes)n).Parent,
                (n, p) => ((NodeWithPropertyAttributes)n).Parent = (NodeWithPropertyAttributes?)p,
                n => ((NodeWithPropertyAttributes)n).Children,
                (n, c) => ((NodeWithPropertyAttributes)n).Children = c.Cast<NodeWithPropertyAttributes>());

            // nodes without implicit strategies

            this.Add<NodeWithoutStrategy>(
                false,
                null,
                n => ((NodeWithoutStrategy)n).Parent,
                (n, p) => ((NodeWithoutStrategy)n).Parent = (NodeWithoutStrategy?)p,
                n => ((NodeWithoutStrategy)n).Children,
                (n, c) => ((NodeWithoutStrategy)n).Children = c.Cast<NodeWithoutStrategy>());

            this.Add<NodeWithInvalidStrategy>(
                false,
                null);

            this.Add<NodeWithInvalidPropertyAttributes>(
                false,
                null,
                n => ((NodeWithInvalidPropertyAttributes)n).Parent,
                (n, p) => ((NodeWithInvalidPropertyAttributes)n).Parent = p,
                n => ((NodeWithInvalidPropertyAttributes)n).Children,
                (n, c) => ((NodeWithInvalidPropertyAttributes)n).Children = c);
        }

        public void Add<TNode>(
            bool hasImplicitStrategy,
            ITraversalStrategy<object>? strategy = null,
            Func<object, object?>? getParentFunc = null,
            Action<object, object?>? setParentFunc = null,
            Func<object, IEnumerable<object>>? getChildrenFunc = null,
            Action<object, IEnumerable<object>>? setChildrenFunc = null)
        {
            Func<object?, object>? factoryMethod = null;

            if (getParentFunc != null &&
                setParentFunc != null &&
                getChildrenFunc != null &&
                setChildrenFunc != null)
            {
                factoryMethod = this.CreateFactoryMethod<TNode>(
                   getParentFunc,
                   setParentFunc,
                   getChildrenFunc,
                   setChildrenFunc);
            }
            else
            {
                factoryMethod = p => Activator.CreateInstance(typeof(TNode))!;
            }

            this.Add(
                typeof(TNode),
                hasImplicitStrategy,
                factoryMethod,
                strategy,
                getParentFunc,
                getChildrenFunc);
        }

        public void Add(
            Type nodeType,
            bool hasImplicitStrategy,
            Func<object?, object> factoryMethod,
            ITraversalStrategy<object>? strategy = null,
            Func<object, object?>? getParentFunc = null,
            Func<object, IEnumerable<object>>? getChildrenFunc = null)
        {
            var metaData = new NodeMetaData(
                nodeType,
                hasImplicitStrategy,
                factoryMethod,
                strategy,
                getParentFunc,
                getChildrenFunc
            );

            this.Add(metaData);
        }

        private ITraversalStrategy<object> CreateStrategy<TNode>()
        {
            var concreteStrategy = TraversalStrategyFactory.Instance.CreateStrategy<TNode>();
            return new TraversalStrategyAdapter<TNode>(concreteStrategy);
        }

        private Func<object?, object> CreateFactoryMethod<TNode>(
            Func<object, object?> getParentFunc,
            Action<object, object?> setParentFunc,
            Func<object, IEnumerable<object>> getChildrenFunc,
            Action<object, IEnumerable<object>> setChildrenFunc)
        {
            return p =>
            {
                var cp = (TNode?)p;
                var n = Activator.CreateInstance(typeof(TNode));

                if (n == null)
                {
                    throw new InvalidOperationException($"Could not create an instance of '{typeof(TNode).Name}'.");
                }

                setParentFunc(n, cp);

                if (cp != null)
                {
                    var children = getChildrenFunc(cp);

                    if (children == null || children.Any() == false)
                    {
                        setChildrenFunc(cp, new List<object>() { n });
                    }
                    else
                    {
                        var list = children.ToList();
                        list.Add(n);

                        setChildrenFunc(cp, list);
                    }
                }

                return n;
            };
        }
    }
}
