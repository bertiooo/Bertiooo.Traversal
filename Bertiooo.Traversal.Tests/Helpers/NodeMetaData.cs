using Bertiooo.Traversal;

namespace Tests.Helpers
{
    public class NodeMetaData
    {
        public NodeMetaData(
            Type nodeType,
            bool hasImplicitStrategy,
            Func<object?, object> factoryMethod,
            ITraversalStrategy<object>? strategy = null,
            Func<object, object?>? getParentFunc = null,
            Func<object, IEnumerable<object>>? getChildrenFunc = null)
        { 
            NodeType = nodeType;
            HasImplicitStrategy = hasImplicitStrategy;
            FactoryMethod = factoryMethod;
            Strategy = strategy;
            GetParentFunc = getParentFunc;
            GetChildrenFunc = getChildrenFunc;
        }

        public Type NodeType { get; set; }

        public bool HasImplicitStrategy { get; set; }

        public Func<object?, object> FactoryMethod { get; set; }

        public ITraversalStrategy<object>? Strategy { get; set; }

        public Func<object, object?>? GetParentFunc { get; set; }

        public Func<object, IEnumerable<object>>? GetChildrenFunc { get; set; }
    }
}
