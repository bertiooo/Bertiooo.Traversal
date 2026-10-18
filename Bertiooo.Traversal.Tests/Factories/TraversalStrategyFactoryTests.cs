using Bertiooo.Traversal;
using Bertiooo.Traversal.Factories;
using Tests.Model;

namespace Tests.Factories
{
    public class TraversalStrategyFactoryTests
    {
        [Fact]
        public void CreateStrategy_UsingDelegates_CreatesStrategies()
        {
            // Arrange
            ITraversalStrategy<NodeWithoutStrategy> strategy;

            // Act
            strategy = TraversalStrategyFactory.Instance.CreateStrategy<NodeWithoutStrategy>(n => n.Parent);
            Assert.NotNull(strategy);

            strategy = TraversalStrategyFactory.Instance.CreateStrategy<NodeWithoutStrategy>(n => n.Children);
            Assert.NotNull(strategy);

            strategy = TraversalStrategyFactory.Instance.CreateStrategy<NodeWithoutStrategy>(n => n.Parent, n => n.Children);
            Assert.NotNull(strategy);
        }

        [Fact]
        public void CreateStrategy_UsingInvalidNodeType_ThrowsException()
        {
            // Act
            Assert.Throws<InvalidOperationException>(() =>
            {
                TraversalStrategyFactory.Instance.CreateStrategy<NodeWithoutStrategy>();
            });
        }

        [Fact]
        public void CreateStrategy_UsingNodeTypeWithInvalidStrategy_ThrowsException()
        {
            // Act
            Assert.Throws<InvalidOperationException>(() =>
            {
                TraversalStrategyFactory.Instance.CreateStrategy<NodeWithInvalidStrategy>();
            });
        }

        [Fact]
        public void CreateStrategy_UsingNodeTypeWithInvalidPropertyAttributes_ThrowsException()
        {
            // Act
            Assert.Throws<InvalidOperationException>(() =>
            {
                TraversalStrategyFactory.Instance.CreateStrategy<NodeWithInvalidPropertyAttributes>();
            });
        }

        [Fact]
        public void CreateStrategy_UsingValidNodeTypes_CreatesStrategies()
        {
            // Act
            var strategy1 = TraversalStrategyFactory.Instance.CreateStrategy<Traversable>();
            Assert.NotNull(strategy1);

            var strategy2 = TraversalStrategyFactory.Instance.CreateStrategy<NodeWithTraversalStrategyAttribute>();
            Assert.NotNull(strategy2);

            var strategy3 = TraversalStrategyFactory.Instance.CreateStrategy<NodeWithPropertyAttributes>();
            Assert.NotNull(strategy3);
        }
    }
}
