using Bertiooo.Traversal;

namespace Tests.Model
{
    internal class CustomTraversalStrategy : ITraversalStrategy<NodeWithTraversalStrategyAttribute>
    {
        public IEnumerable<NodeWithTraversalStrategyAttribute> GetChildren(NodeWithTraversalStrategyAttribute node)
        {
            return node.Children;
        }

        public NodeWithTraversalStrategyAttribute? GetParent(NodeWithTraversalStrategyAttribute node)
        {
            return node.Parent;
        }
    }
}
