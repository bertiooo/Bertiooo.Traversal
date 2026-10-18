using Bertiooo.Traversal.Attributes;

namespace Tests.Model
{
    [TraversalStrategy(typeof(CustomTraversalStrategy))]
    internal class NodeWithTraversalStrategyAttribute
    {
        public string? Name { get; set; }

        public NodeWithTraversalStrategyAttribute? Parent { get; set; }

        public IEnumerable<NodeWithTraversalStrategyAttribute> Children { get; set; } = Enumerable.Empty<NodeWithTraversalStrategyAttribute>();
    }
}
