using Bertiooo.Traversal;
using Bertiooo.Traversal.Selectors;
using System.Xml.Linq;
using Tests.Helpers;
using Tests.Model;

namespace Tests
{
    public class TraversalExtensionTests
    {
        #region Node Analysis

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void IsRoot_ReturnsTrue_WhenNodeHasNoParent(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            bool result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.IsRoot((dynamic)root);
                Assert.True(result);
            }
            
            if (metaData.GetParentFunc != null)
            {
                var parent = metaData.GetParentFunc.Invoke(root);
                Assert.Equal(default, parent);

                result = TraversalExtensions.IsRoot((dynamic)root, metaData.GetParentFunc);
                Assert.True(result);
            }

            if (metaData.Strategy != null)
            {
                var parent = metaData.Strategy.GetParent(root);
                Assert.Equal(default, parent);

                result = TraversalExtensions.IsRoot((dynamic)root, metaData.Strategy);
                Assert.True(result);
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void IsRoot_ReturnsFalse_WhenNodeHasParent(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            var child = metaData.FactoryMethod.Invoke(root);

            bool result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.IsRoot((dynamic)child);
                Assert.False(result);
            }

            if (metaData.GetParentFunc != null)
            {
                var parent = metaData.GetParentFunc.Invoke(child);
                Assert.NotEqual(default, parent);

                result = TraversalExtensions.IsRoot((dynamic)child, metaData.GetParentFunc);
                Assert.False(result);
            }

            if (metaData.Strategy != null)
            {
                var parent = metaData.Strategy.GetParent(child);
                Assert.NotEqual(default, parent);

                result = TraversalExtensions.IsRoot((dynamic)child, metaData.Strategy);
                Assert.False(result);
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void IsRoot_ThrowsException_WhenNoStrategyCanBeDerivedFromNodeType(NodeMetaData metaData)
        {
            if (metaData.HasImplicitStrategy)
                return;

            var root = metaData.FactoryMethod.Invoke(null);
            var action = () => TraversalExtensions.IsRoot((dynamic)root);

            Assert.Throws<InvalidOperationException>(action);
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void IsInnerNode_ReturnsTrue_WhenNodeIsNotARootNodeAndNotALeafNode(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            var child = metaData.FactoryMethod.Invoke(root);
            var grandchild = metaData.FactoryMethod.Invoke(child);

            bool result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.IsInnerNode((dynamic)child);
                Assert.True(result);
            }

            if (metaData.GetParentFunc != null && metaData.GetChildrenFunc != null)
            {
                var isRoot = TraversalExtensions.IsRoot((dynamic)child, metaData.GetParentFunc);
                Assert.False(isRoot);

                var isLeaf = TraversalExtensions.IsLeaf((dynamic)child, metaData.GetChildrenFunc);
                Assert.False(isLeaf);

                result = TraversalExtensions.IsInnerNode((dynamic)child, metaData.GetParentFunc, metaData.GetChildrenFunc);
                Assert.True(result);
            }

            if(metaData.Strategy != null)
            {
                var isRoot = TraversalExtensions.IsRoot((dynamic)child, metaData.Strategy);
                Assert.False(isRoot);

                var isLeaf = TraversalExtensions.IsLeaf((dynamic)child, metaData.Strategy);
                Assert.False(isLeaf);

                result = TraversalExtensions.IsInnerNode((dynamic)child, metaData.Strategy);
                Assert.True(result);
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void IsInnerNode_ReturnsFalse_WhenNodeIsARootNode(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);

            bool result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.IsInnerNode((dynamic)root);
                Assert.False(result);
            }

            if (metaData.GetParentFunc != null && metaData.GetChildrenFunc != null)
            {
                var isRoot = TraversalExtensions.IsRoot((dynamic)root, metaData.GetParentFunc);
                Assert.True(isRoot);

                result = TraversalExtensions.IsInnerNode((dynamic)root, metaData.GetParentFunc, metaData.GetChildrenFunc);
                Assert.False(result);
            }

            if (metaData.Strategy != null)
            {
                var isRoot = TraversalExtensions.IsRoot((dynamic)root, metaData.Strategy);
                Assert.True(isRoot);

                result = TraversalExtensions.IsInnerNode((dynamic)root, metaData.Strategy);
                Assert.False(result);
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void IsInnerNode_ReturnsFalse_WhenNodeIsALeafNode(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            var child = metaData.FactoryMethod.Invoke(root);

            bool result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.IsInnerNode((dynamic)child);
                Assert.False(result);
            }

            if (metaData.GetParentFunc != null && metaData.GetChildrenFunc != null)
            {
                var isLeaf = TraversalExtensions.IsLeaf((dynamic)child, metaData.GetChildrenFunc);
                Assert.True(isLeaf);

                result = TraversalExtensions.IsInnerNode((dynamic)child, metaData.GetParentFunc, metaData.GetChildrenFunc);
                Assert.False(result);
            }

            if (metaData.Strategy != null)
            {
                var isLeaf = TraversalExtensions.IsLeaf((dynamic)child, metaData.Strategy);
                Assert.True(isLeaf);

                result = TraversalExtensions.IsInnerNode((dynamic)child, metaData.Strategy);
                Assert.False(result);
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void IsInnerNode_ThrowsException_WhenNoStrategyCanBeDerivedFromNodeType(NodeMetaData metaData)
        {
            if (metaData.HasImplicitStrategy)
                return;

            var root = metaData.FactoryMethod.Invoke(null);
            var action = () => TraversalExtensions.IsInnerNode((dynamic)root);

            Assert.Throws<InvalidOperationException>(action);
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void IsLeaf_ReturnsTrue_WhenNodeHasNoChildren(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            var child = metaData.FactoryMethod.Invoke(root);

            bool result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.IsLeaf((dynamic)child);
                Assert.True(result);
            }

            if (metaData.GetChildrenFunc != null)
            {
                var children = metaData.GetChildrenFunc.Invoke(child);
                Assert.Empty(children);

                result = TraversalExtensions.IsLeaf((dynamic)child, metaData.GetChildrenFunc);
                Assert.True(result);
            }

            if (metaData.Strategy != null)
            {
                var children = metaData.Strategy.GetChildren(child);
                Assert.Empty(children);

                result = TraversalExtensions.IsLeaf((dynamic)child, metaData.Strategy);
                Assert.True(result);
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void IsLeaf_ReturnsFalse_WhenNodeHasChildren(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            var child = metaData.FactoryMethod.Invoke(root);
            var grandchild = metaData.FactoryMethod.Invoke(child);

            bool result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.IsLeaf((dynamic)child);
                Assert.False(result);
            }

            if (metaData.GetChildrenFunc != null)
            {
                var children = metaData.GetChildrenFunc.Invoke(child);
                Assert.NotEmpty(children);

                result = TraversalExtensions.IsLeaf((dynamic)child, metaData.GetChildrenFunc);
                Assert.False(result);
            }

            if (metaData.Strategy != null)
            {
                var children = metaData.Strategy.GetChildren(child);
                Assert.NotEmpty(children);

                result = TraversalExtensions.IsLeaf((dynamic)child, metaData.Strategy);
                Assert.False(result);
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void IsLeaf_ThrowsException_WhenNoStrategyCanBeDerivedFromNodeType(NodeMetaData metaData)
        {
            if (metaData.HasImplicitStrategy)
                return;

            var root = metaData.FactoryMethod.Invoke(null);
            var action = () => TraversalExtensions.IsLeaf((dynamic)root);

            Assert.Throws<InvalidOperationException>(action);
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void HasParent_ReturnsTrue_WhenParentIsNotDefault(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            var child = metaData.FactoryMethod.Invoke(root);

            bool result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.HasParent((dynamic)child);
                Assert.True(result);
            }

            if (metaData.GetParentFunc != null)
            {
                var parent = metaData.GetParentFunc.Invoke(child);
                Assert.NotEqual(default, parent);

                result = TraversalExtensions.HasParent((dynamic)child, metaData.GetParentFunc);
                Assert.True(result);
            }

            if (metaData.Strategy != null)
            {
                var parent = metaData.Strategy.GetParent(child);
                Assert.NotEqual(default, parent);

                result = TraversalExtensions.HasParent((dynamic)child, metaData.Strategy);
                Assert.True(result);
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void HasParent_ReturnsFalse_WhenParentIsDefault(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(default);

            bool result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.HasParent((dynamic)root);
                Assert.False(result);
            }

            if (metaData.GetParentFunc != null)
            {
                var parent = metaData.GetParentFunc.Invoke(root);
                Assert.Equal(default, parent);

                result = TraversalExtensions.HasParent((dynamic)root, metaData.GetParentFunc);
                Assert.False(result);
            }

            if (metaData.Strategy != null)
            {
                var parent = metaData.Strategy.GetParent(root);
                Assert.Equal(default, parent);

                result = TraversalExtensions.HasParent((dynamic)root, metaData.Strategy);
                Assert.False(result);
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void HasParent_ThrowsException_WhenNoStrategyCanBeDerivedFromNodeType(NodeMetaData metaData)
        {
            if (metaData.HasImplicitStrategy)
                return;

            var root = metaData.FactoryMethod.Invoke(null);
            var action = () => TraversalExtensions.HasParent((dynamic)root);

            Assert.Throws<InvalidOperationException>(action);
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void HasChildren_ReturnsTrue_WhenNodeHasAnyChildren(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            var child = metaData.FactoryMethod.Invoke(root);

            bool result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.HasChildren((dynamic)root);
                Assert.True(result);
            }

            if (metaData.GetChildrenFunc != null)
            {
                var children = metaData.GetChildrenFunc.Invoke(root);
                Assert.NotEmpty(children);

                result = TraversalExtensions.HasChildren((dynamic)root, metaData.GetChildrenFunc);
                Assert.True(result);
            }

            if (metaData.Strategy != null)
            {
                var children = metaData.Strategy.GetChildren(root);
                Assert.NotEmpty(children);

                result = TraversalExtensions.HasChildren((dynamic)root, metaData.Strategy);
                Assert.True(result);
            }
        }
        
        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void HasChildren_ReturnsFalse_WhenNodeHasNoChilden(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);

            bool result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.HasChildren((dynamic)root);
                Assert.False(result);
            }

            if (metaData.GetChildrenFunc != null)
            {
                var children = metaData.GetChildrenFunc.Invoke(root);
                Assert.Empty(children);

                result = TraversalExtensions.HasChildren((dynamic)root, metaData.GetChildrenFunc);
                Assert.False(result);
            }

            if (metaData.Strategy != null)
            {
                var children = metaData.Strategy.GetChildren(root);
                Assert.Empty(children);

                result = TraversalExtensions.HasChildren((dynamic)root, metaData.Strategy);
                Assert.False(result);
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void HasChildren_ThrowsException_WhenNoStrategyCanBeDerivedFromNodeType(NodeMetaData metaData)
        {
            if (metaData.HasImplicitStrategy)
                return;

            var root = metaData.FactoryMethod.Invoke(null);
            var action = () => TraversalExtensions.HasChildren((dynamic)root);

            Assert.Throws<InvalidOperationException>(action);
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void HasSiblings_ReturnsTrue_WhenNodeHasAtLeastOneSibling(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            var firstChild = metaData.FactoryMethod.Invoke(root);
            var secondChild = metaData.FactoryMethod.Invoke(root);

            bool result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.HasSiblings((dynamic)firstChild);
                Assert.True(result);
            }

            if (metaData.GetParentFunc != null && metaData.GetChildrenFunc != null)
            {
                var parent = metaData.GetParentFunc.Invoke(firstChild);
                Assert.NotEqual(default, parent);

                var siblings = metaData.GetChildrenFunc.Invoke(parent).Where(x => !Equals(x, firstChild));
                Assert.NotEmpty(siblings);

                result = TraversalExtensions.HasSiblings((dynamic)firstChild, metaData.GetParentFunc, metaData.GetChildrenFunc);
                Assert.True(result);
            }

            if (metaData.Strategy != null)
            {
                var parent = metaData.Strategy.GetParent(firstChild);
                Assert.NotEqual(default, parent);

                var siblings = metaData.Strategy.GetChildren(parent).Where(x => !Equals(x, firstChild));
                Assert.NotEmpty(siblings);

                result = TraversalExtensions.HasSiblings((dynamic)firstChild, metaData.Strategy);
                Assert.True(result);
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void HasSiblings_ReturnsFalse_WhenNodeHasNoSiblings(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            var onlyChild = metaData.FactoryMethod.Invoke(root);

            bool result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.HasSiblings((dynamic)onlyChild);
                Assert.False(result);
            }

            if (metaData.GetParentFunc != null && metaData.GetChildrenFunc != null)
            {
                var parent = metaData.GetParentFunc.Invoke(onlyChild);
                Assert.NotEqual(default, parent);

                var siblings = metaData.GetChildrenFunc.Invoke(parent).Where(x => !Equals(x, onlyChild));
                Assert.Empty(siblings);

                result = TraversalExtensions.HasSiblings((dynamic)onlyChild, metaData.GetParentFunc, metaData.GetChildrenFunc);
                Assert.False(result);
            }

            if (metaData.Strategy != null)
            {
                var parent = metaData.Strategy.GetParent(onlyChild);
                Assert.NotEqual(default, parent);

                var siblings = metaData.Strategy.GetChildren(parent).Where(x => !Equals(x, onlyChild));
                Assert.Empty(siblings);

                result = TraversalExtensions.HasSiblings((dynamic)onlyChild, metaData.Strategy);
                Assert.False(result);
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void HasSiblings_ReturnsFalse_WhenNodeIsRoot(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);

            bool result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.HasSiblings((dynamic)root);
                Assert.False(result);
            }

            if (metaData.GetParentFunc != null && metaData.GetChildrenFunc != null)
            {
                var parent = metaData.GetParentFunc.Invoke(root);
                Assert.Equal(default, parent);

                result = TraversalExtensions.HasSiblings((dynamic)root, metaData.GetParentFunc, metaData.GetChildrenFunc);
                Assert.False(result);
            }

            if (metaData.Strategy != null)
            {
                var parent = metaData.Strategy.GetParent(root);
                Assert.Equal(default, parent);

                result = TraversalExtensions.HasSiblings((dynamic)root, metaData.Strategy);
                Assert.False(result);
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void HasSiblings_ThrowsException_WhenNoStrategyCanBeDerivedFromNodeType(NodeMetaData metaData)
        {
            if (metaData.HasImplicitStrategy)
                return;

            var root = metaData.FactoryMethod.Invoke(null);
            var action = () => TraversalExtensions.HasSiblings((dynamic)root);

            Assert.Throws<InvalidOperationException>(action);
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void GetLevel_ReturnsZero_WhenNodeIsRoot(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            int result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.GetLevel((dynamic)root);
                Assert.Equal(0, result);
            }

            if (metaData.GetParentFunc != null)
            {
                result = TraversalExtensions.GetLevel((dynamic)root, metaData.GetParentFunc);
                Assert.Equal(0, result);
            }

            if (metaData.Strategy != null)
            {
                result = TraversalExtensions.GetLevel((dynamic)root, metaData.Strategy);
                Assert.Equal(0, result);
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void GetLevel_ReturnsCorrectLevel_WhenNodeHasAncestors(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            var child = metaData.FactoryMethod.Invoke(root);
            var grandchild = metaData.FactoryMethod.Invoke(child);
            int result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.GetLevel((dynamic)child);
                Assert.Equal(1, result);

                result = TraversalExtensions.GetLevel((dynamic)grandchild);
                Assert.Equal(2, result);
            }

            if (metaData.GetParentFunc != null)
            {
                result = TraversalExtensions.GetLevel((dynamic)child, metaData.GetParentFunc);
                Assert.Equal(1, result);

                result = TraversalExtensions.GetLevel((dynamic)grandchild, metaData.GetParentFunc);
                Assert.Equal(2, result);
            }

            if (metaData.Strategy != null)
            {
                result = TraversalExtensions.GetLevel((dynamic)child, metaData.Strategy);
                Assert.Equal(1, result);

                result = TraversalExtensions.GetLevel((dynamic)grandchild, metaData.Strategy);
                Assert.Equal(2, result);
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void GetLevel_ThrowsException_WhenNoStrategyCanBeDerivedFromNodeType(NodeMetaData metaData)
        {
            if (metaData.HasImplicitStrategy)
                return;

            var root = metaData.FactoryMethod.Invoke(null);
            var action = () => TraversalExtensions.GetLevel((dynamic)root);

            Assert.Throws<InvalidOperationException>(action);
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void GetMaxDepth_ReturnsZero_WhenNodeIsLeaf(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            var child = metaData.FactoryMethod.Invoke(root);
            int result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.GetMaxDepth((dynamic)child);
                Assert.Equal(0, result);
            }

            if (metaData.GetParentFunc != null && metaData.GetChildrenFunc != null)
            {
                result = TraversalExtensions.GetMaxDepth((dynamic)child, metaData.GetParentFunc, metaData.GetChildrenFunc);
                Assert.Equal(0, result);
            }

            if (metaData.Strategy != null)
            {
                result = TraversalExtensions.GetMaxDepth((dynamic)child, metaData.Strategy);
                Assert.Equal(0, result);
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void GetMaxDepth_ReturnsCorrectDepth_WhenNodeHasDescendants(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            var child = metaData.FactoryMethod.Invoke(root);
            var grandchild = metaData.FactoryMethod.Invoke(child);
            int result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.GetMaxDepth((dynamic)root);
                Assert.Equal(2, result);

                result = TraversalExtensions.GetMaxDepth((dynamic)child);
                Assert.Equal(1, result);
            }

            if (metaData.GetParentFunc != null && metaData.GetChildrenFunc != null)
            {
                result = TraversalExtensions.GetMaxDepth((dynamic)root, metaData.GetParentFunc, metaData.GetChildrenFunc);
                Assert.Equal(2, result);

                result = TraversalExtensions.GetMaxDepth((dynamic)child, metaData.GetParentFunc, metaData.GetChildrenFunc);
                Assert.Equal(1, result);
            }

            if (metaData.Strategy != null)
            {
                result = TraversalExtensions.GetMaxDepth((dynamic)root, metaData.Strategy);
                Assert.Equal(2, result);

                result = TraversalExtensions.GetMaxDepth((dynamic)child, metaData.Strategy);
                Assert.Equal(1, result);
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void GetMaxDepth_ThrowsException_WhenNoStrategyCanBeDerivedFromNodeType(NodeMetaData metaData)
        {
            if (metaData.HasImplicitStrategy)
                return;

            var root = metaData.FactoryMethod.Invoke(null);
            var action = () => TraversalExtensions.GetMaxDepth((dynamic)root);

            Assert.Throws<InvalidOperationException>(action);
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void IsChildOf_ReturnsTrue_WhenNodeIsDirectChildOfOther(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            var child = metaData.FactoryMethod.Invoke(root);
            bool result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.IsChildOf((dynamic)child, (dynamic)root);
                Assert.True(result);
            }

            if (metaData.GetParentFunc != null)
            {
                result = TraversalExtensions.IsChildOf((dynamic)child, (dynamic)root, metaData.GetParentFunc);
                Assert.True(result);
            }

            if (metaData.Strategy != null)
            {
                result = TraversalExtensions.IsChildOf((dynamic)child, (dynamic)root, metaData.Strategy);
                Assert.True(result);
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void IsChildOf_ReturnsFalse_WhenNodeIsNotDirectChildOfOther(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            var child = metaData.FactoryMethod.Invoke(root);
            var grandchild = metaData.FactoryMethod.Invoke(child);
            bool result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.IsChildOf((dynamic)grandchild, (dynamic)root);
                Assert.False(result);

                result = TraversalExtensions.IsChildOf((dynamic)root, (dynamic)child);
                Assert.False(result);
            }

            if (metaData.GetParentFunc != null)
            {
                result = TraversalExtensions.IsChildOf((dynamic)grandchild, (dynamic)root, metaData.GetParentFunc);
                Assert.False(result);

                result = TraversalExtensions.IsChildOf((dynamic)root, (dynamic)child, metaData.GetParentFunc);
                Assert.False(result);
            }

            if (metaData.Strategy != null)
            {
                result = TraversalExtensions.IsChildOf((dynamic)grandchild, (dynamic)root, metaData.Strategy);
                Assert.False(result);

                result = TraversalExtensions.IsChildOf((dynamic)root, (dynamic)child, metaData.Strategy);
                Assert.False(result);
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void IsChildOf_ThrowsException_WhenNoStrategyCanBeDerivedFromNodeType(NodeMetaData metaData)
        {
            if (metaData.HasImplicitStrategy)
                return;

            var root = metaData.FactoryMethod.Invoke(null);
            var child = metaData.FactoryMethod.Invoke(root);
            var action = () => TraversalExtensions.IsChildOf((dynamic)child, (dynamic)root);

            Assert.Throws<InvalidOperationException>(action);
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void IsParentOf_ReturnsTrue_WhenNodeIsDirectParentOfOther(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            var child = metaData.FactoryMethod.Invoke(root);
            bool result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.IsParentOf((dynamic)root, (dynamic)child);
                Assert.True(result);
            }

            if (metaData.GetParentFunc != null)
            {
                result = TraversalExtensions.IsParentOf((dynamic)root, (dynamic)child, metaData.GetParentFunc);
                Assert.True(result);
            }

            if (metaData.Strategy != null)
            {
                result = TraversalExtensions.IsParentOf((dynamic)root, (dynamic)child, metaData.Strategy);
                Assert.True(result);
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void IsParentOf_ReturnsFalse_WhenNodeIsNotDirectParentOfOther(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            var child = metaData.FactoryMethod.Invoke(root);
            var grandchild = metaData.FactoryMethod.Invoke(child);
            bool result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.IsParentOf((dynamic)root, (dynamic)grandchild);
                Assert.False(result);

                result = TraversalExtensions.IsParentOf((dynamic)child, (dynamic)root);
                Assert.False(result);
            }

            if (metaData.GetParentFunc != null)
            {
                result = TraversalExtensions.IsParentOf((dynamic)root, (dynamic)grandchild, metaData.GetParentFunc);
                Assert.False(result);

                result = TraversalExtensions.IsParentOf((dynamic)child, (dynamic)root, metaData.GetParentFunc);
                Assert.False(result);
            }

            if (metaData.Strategy != null)
            {
                result = TraversalExtensions.IsParentOf((dynamic)root, (dynamic)grandchild, metaData.Strategy);
                Assert.False(result);

                result = TraversalExtensions.IsParentOf((dynamic)child, (dynamic)root, metaData.Strategy);
                Assert.False(result);
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void IsParentOf_ThrowsException_WhenNoStrategyCanBeDerivedFromNodeType(NodeMetaData metaData)
        {
            if (metaData.HasImplicitStrategy)
                return;

            var root = metaData.FactoryMethod.Invoke(null);
            var child = metaData.FactoryMethod.Invoke(root);
            var action = () => TraversalExtensions.IsParentOf((dynamic)root, (dynamic)child);

            Assert.Throws<InvalidOperationException>(action);
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void IsSiblingOf_ReturnsTrue_WhenNodesShareSameParent(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            var firstChild = metaData.FactoryMethod.Invoke(root);
            var secondChild = metaData.FactoryMethod.Invoke(root);
            bool result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.IsSiblingOf((dynamic)firstChild, (dynamic)secondChild);
                Assert.True(result);

                result = TraversalExtensions.IsSiblingOf((dynamic)secondChild, (dynamic)firstChild);
                Assert.True(result);
            }

            if (metaData.GetParentFunc != null)
            {
                result = TraversalExtensions.IsSiblingOf((dynamic)firstChild, (dynamic)secondChild, metaData.GetParentFunc);
                Assert.True(result);

                result = TraversalExtensions.IsSiblingOf((dynamic)secondChild, (dynamic)firstChild, metaData.GetParentFunc);
                Assert.True(result);
            }

            if (metaData.Strategy != null)
            {
                result = TraversalExtensions.IsSiblingOf((dynamic)firstChild, (dynamic)secondChild, metaData.Strategy);
                Assert.True(result);

                result = TraversalExtensions.IsSiblingOf((dynamic)secondChild, (dynamic)firstChild, metaData.Strategy);
                Assert.True(result);
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void IsSiblingOf_ReturnsFalse_WhenNodesDoNotShareSameParent(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            var child = metaData.FactoryMethod.Invoke(root);
            var grandchild = metaData.FactoryMethod.Invoke(child);
            bool result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.IsSiblingOf((dynamic)child, (dynamic)grandchild);
                Assert.False(result);

                result = TraversalExtensions.IsSiblingOf((dynamic)root, (dynamic)child);
                Assert.False(result);
            }

            if (metaData.GetParentFunc != null)
            {
                result = TraversalExtensions.IsSiblingOf((dynamic)child, (dynamic)grandchild, metaData.GetParentFunc);
                Assert.False(result);

                result = TraversalExtensions.IsSiblingOf((dynamic)root, (dynamic)child, metaData.GetParentFunc);
                Assert.False(result);
            }

            if (metaData.Strategy != null)
            {
                result = TraversalExtensions.IsSiblingOf((dynamic)child, (dynamic)grandchild, metaData.Strategy);
                Assert.False(result);

                result = TraversalExtensions.IsSiblingOf((dynamic)root, (dynamic)child, metaData.Strategy);
                Assert.False(result);
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void IsSiblingOf_ThrowsException_WhenNoStrategyCanBeDerivedFromNodeType(NodeMetaData metaData)
        {
            if (metaData.HasImplicitStrategy)
                return;

            var root = metaData.FactoryMethod.Invoke(null);
            var firstChild = metaData.FactoryMethod.Invoke(root);
            var secondChild = metaData.FactoryMethod.Invoke(root);
            var action = () => TraversalExtensions.IsSiblingOf((dynamic)firstChild, (dynamic)secondChild);

            Assert.Throws<InvalidOperationException>(action);
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void IsDescendantOf_ReturnsTrue_WhenNodeIsDescendantOfOther(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            var child = metaData.FactoryMethod.Invoke(root);
            var grandchild = metaData.FactoryMethod.Invoke(child);
            bool result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.IsDescendantOf((dynamic)child, (dynamic)root);
                Assert.True(result);

                result = TraversalExtensions.IsDescendantOf((dynamic)grandchild, (dynamic)root);
                Assert.True(result);

                result = TraversalExtensions.IsDescendantOf((dynamic)grandchild, (dynamic)child);
                Assert.True(result);
            }

            if (metaData.GetParentFunc != null)
            {
                result = TraversalExtensions.IsDescendantOf((dynamic)child, (dynamic)root, metaData.GetParentFunc);
                Assert.True(result);

                result = TraversalExtensions.IsDescendantOf((dynamic)grandchild, (dynamic)root, metaData.GetParentFunc);
                Assert.True(result);

                result = TraversalExtensions.IsDescendantOf((dynamic)grandchild, (dynamic)child, metaData.GetParentFunc);
                Assert.True(result);
            }

            if (metaData.Strategy != null)
            {
                result = TraversalExtensions.IsDescendantOf((dynamic)child, (dynamic)root, metaData.Strategy);
                Assert.True(result);

                result = TraversalExtensions.IsDescendantOf((dynamic)grandchild, (dynamic)root, metaData.Strategy);
                Assert.True(result);

                result = TraversalExtensions.IsDescendantOf((dynamic)grandchild, (dynamic)child, metaData.Strategy);
                Assert.True(result);
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void IsDescendantOf_ReturnsFalse_WhenNodeIsNotDescendantOfOther(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            var child = metaData.FactoryMethod.Invoke(root);
            var grandchild = metaData.FactoryMethod.Invoke(child);
            bool result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.IsDescendantOf((dynamic)root, (dynamic)child);
                Assert.False(result);

                result = TraversalExtensions.IsDescendantOf((dynamic)child, (dynamic)grandchild);
                Assert.False(result);
            }

            if (metaData.GetParentFunc != null)
            {
                result = TraversalExtensions.IsDescendantOf((dynamic)root, (dynamic)child, metaData.GetParentFunc);
                Assert.False(result);

                result = TraversalExtensions.IsDescendantOf((dynamic)child, (dynamic)grandchild, metaData.GetParentFunc);
                Assert.False(result);
            }

            if (metaData.Strategy != null)
            {
                result = TraversalExtensions.IsDescendantOf((dynamic)root, (dynamic)child, metaData.Strategy);
                Assert.False(result);

                result = TraversalExtensions.IsDescendantOf((dynamic)child, (dynamic)grandchild, metaData.Strategy);
                Assert.False(result);
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void IsDescendantOf_ThrowsException_WhenNoStrategyCanBeDerivedFromNodeType(NodeMetaData metaData)
        {
            if (metaData.HasImplicitStrategy)
                return;

            var root = metaData.FactoryMethod.Invoke(null);
            var child = metaData.FactoryMethod.Invoke(root);
            var action = () => TraversalExtensions.IsDescendantOf((dynamic)child, (dynamic)root);

            Assert.Throws<InvalidOperationException>(action);
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void IsAncestorOf_ReturnsTrue_WhenNodeIsAncestorOfOther(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            var child = metaData.FactoryMethod.Invoke(root);
            var grandchild = metaData.FactoryMethod.Invoke(child);
            bool result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.IsAncestorOf((dynamic)root, (dynamic)child);
                Assert.True(result);

                result = TraversalExtensions.IsAncestorOf((dynamic)root, (dynamic)grandchild);
                Assert.True(result);

                result = TraversalExtensions.IsAncestorOf((dynamic)child, (dynamic)grandchild);
                Assert.True(result);
            }

            if (metaData.GetParentFunc != null)
            {
                result = TraversalExtensions.IsAncestorOf((dynamic)root, (dynamic)child, metaData.GetParentFunc);
                Assert.True(result);

                result = TraversalExtensions.IsAncestorOf((dynamic)root, (dynamic)grandchild, metaData.GetParentFunc);
                Assert.True(result);

                result = TraversalExtensions.IsAncestorOf((dynamic)child, (dynamic)grandchild, metaData.GetParentFunc);
                Assert.True(result);
            }

            if (metaData.Strategy != null)
            {
                result = TraversalExtensions.IsAncestorOf((dynamic)root, (dynamic)child, metaData.Strategy);
                Assert.True(result);

                result = TraversalExtensions.IsAncestorOf((dynamic)root, (dynamic)grandchild, metaData.Strategy);
                Assert.True(result);

                result = TraversalExtensions.IsAncestorOf((dynamic)child, (dynamic)grandchild, metaData.Strategy);
                Assert.True(result);
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void IsAncestorOf_ReturnsFalse_WhenNodeIsNotAncestorOfOther(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            var child = metaData.FactoryMethod.Invoke(root);
            var grandchild = metaData.FactoryMethod.Invoke(child);
            bool result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.IsAncestorOf((dynamic)child, (dynamic)root);
                Assert.False(result);

                result = TraversalExtensions.IsAncestorOf((dynamic)grandchild, (dynamic)child);
                Assert.False(result);
            }

            if (metaData.GetParentFunc != null)
            {
                result = TraversalExtensions.IsAncestorOf((dynamic)child, (dynamic)root, metaData.GetParentFunc);
                Assert.False(result);

                result = TraversalExtensions.IsAncestorOf((dynamic)grandchild, (dynamic)child, metaData.GetParentFunc);
                Assert.False(result);
            }

            if (metaData.Strategy != null)
            {
                result = TraversalExtensions.IsAncestorOf((dynamic)child, (dynamic)root, metaData.Strategy);
                Assert.False(result);

                result = TraversalExtensions.IsAncestorOf((dynamic)grandchild, (dynamic)child, metaData.Strategy);
                Assert.False(result);
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void IsAncestorOf_ThrowsException_WhenNoStrategyCanBeDerivedFromNodeType(NodeMetaData metaData)
        {
            if (metaData.HasImplicitStrategy)
                return;

            var root = metaData.FactoryMethod.Invoke(null);
            var child = metaData.FactoryMethod.Invoke(root);
            var action = () => TraversalExtensions.IsAncestorOf((dynamic)root, (dynamic)child);

            Assert.Throws<InvalidOperationException>(action);
        }

        #endregion

        #region Related Nodes Retrieval

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void Root_ReturnsRootNode_WhenNodeHasAncestors(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            var child = metaData.FactoryMethod.Invoke(root);
            var grandchild = metaData.FactoryMethod.Invoke(child);
            object result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.Root((dynamic)grandchild);
                Assert.Equal(root, result);

                result = TraversalExtensions.Root((dynamic)child);
                Assert.Equal(root, result);
            }

            if (metaData.GetParentFunc != null)
            {
                result = TraversalExtensions.Root((dynamic)grandchild, metaData.GetParentFunc);
                Assert.Equal(root, result);

                result = TraversalExtensions.Root((dynamic)child, metaData.GetParentFunc);
                Assert.Equal(root, result);
            }

            if (metaData.Strategy != null)
            {
                result = TraversalExtensions.Root((dynamic)grandchild, metaData.Strategy);
                Assert.Equal(root, result);

                result = TraversalExtensions.Root((dynamic)child, metaData.Strategy);
                Assert.Equal(root, result);
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void Root_ReturnsSelf_WhenNodeIsRoot(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            object result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.Root((dynamic)root);
                Assert.Equal(root, result);
            }

            if (metaData.GetParentFunc != null)
            {
                result = TraversalExtensions.Root((dynamic)root, metaData.GetParentFunc);
                Assert.Equal(root, result);
            }

            if (metaData.Strategy != null)
            {
                result = TraversalExtensions.Root((dynamic)root, metaData.Strategy);
                Assert.Equal(root, result);
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void Root_ThrowsException_WhenNoStrategyCanBeDerivedFromNodeType(NodeMetaData metaData)
        {
            if (metaData.HasImplicitStrategy)
                return;

            var root = metaData.FactoryMethod.Invoke(null);
            var action = () => TraversalExtensions.Root((dynamic)root);

            Assert.Throws<InvalidOperationException>(action);
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void WithParent_ReturnsNodeAndParent_WhenNodeHasParent(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            var child = metaData.FactoryMethod.Invoke(root);
            IEnumerable<object> result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.WithParent((dynamic)child);
                Assert.Equal(2, result.Count());
                Assert.Equal(child, result.First());
                Assert.Equal(root, result.ElementAt(1));
            }

            if (metaData.GetParentFunc != null)
            {
                result = TraversalExtensions.WithParent((dynamic)child, metaData.GetParentFunc);
                Assert.Equal(2, result.Count());
                Assert.Equal(child, result.First());
                Assert.Equal(root, result.ElementAt(1));
            }

            if (metaData.Strategy != null)
            {
                result = TraversalExtensions.WithParent((dynamic)child, metaData.Strategy);
                Assert.Equal(2, result.Count());
                Assert.Equal(child, result.First());
                Assert.Equal(root, result.ElementAt(1));
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void WithParent_ReturnsOnlyNode_WhenNodeHasNoParent(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            IEnumerable<object> result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.WithParent((dynamic)root);
                Assert.Single(result);
                Assert.Equal(root, result.First());
            }

            if (metaData.GetParentFunc != null)
            {
                result = TraversalExtensions.WithParent((dynamic)root, metaData.GetParentFunc);
                Assert.Single(result);
                Assert.Equal(root, result.First());
            }

            if (metaData.Strategy != null)
            {
                result = TraversalExtensions.WithParent((dynamic)root, metaData.Strategy);
                Assert.Single(result);
                Assert.Equal(root, result.First());
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void WithParent_ThrowsException_WhenNoStrategyCanBeDerivedFromNodeType(NodeMetaData metaData)
        {
            if (metaData.HasImplicitStrategy)
                return;

            var root = metaData.FactoryMethod.Invoke(null);
            var action = () => TraversalExtensions.WithParent((dynamic)root).ToList();

            Assert.Throws<InvalidOperationException>(action);
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void WithChildren_ReturnsNodeAndChildren_WhenNodeHasChildren(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            var firstChild = metaData.FactoryMethod.Invoke(root);
            var secondChild = metaData.FactoryMethod.Invoke(root);
            IEnumerable<object> result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.WithChildren((dynamic)root);
                Assert.Equal(3, result.Count());
                Assert.Equal(root, result.First());
                Assert.Contains(firstChild, result);
                Assert.Contains(secondChild, result);
            }

            if (metaData.GetChildrenFunc != null)
            {
                result = TraversalExtensions.WithChildren((dynamic)root, metaData.GetChildrenFunc);
                Assert.Equal(3, result.Count());
                Assert.Equal(root, result.First());
                Assert.Contains(firstChild, result);
                Assert.Contains(secondChild, result);
            }

            if (metaData.Strategy != null)
            {
                result = TraversalExtensions.WithChildren((dynamic)root, metaData.Strategy);
                Assert.Equal(3, result.Count());
                Assert.Equal(root, result.First());
                Assert.Contains(firstChild, result);
                Assert.Contains(secondChild, result);
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void WithChildren_ReturnsOnlyNode_WhenNodeHasNoChildren(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            var child = metaData.FactoryMethod.Invoke(root);
            IEnumerable<object> result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.WithChildren((dynamic)child);
                Assert.Single(result);
                Assert.Equal(child, result.First());
            }

            if (metaData.GetChildrenFunc != null)
            {
                result = TraversalExtensions.WithChildren((dynamic)child, metaData.GetChildrenFunc);
                Assert.Single(result);
                Assert.Equal(child, result.First());
            }

            if (metaData.Strategy != null)
            {
                result = TraversalExtensions.WithChildren((dynamic)child, metaData.Strategy);
                Assert.Single(result);
                Assert.Equal(child, result.First());
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void WithChildren_ThrowsException_WhenNoStrategyCanBeDerivedFromNodeType(NodeMetaData metaData)
        {
            if (metaData.HasImplicitStrategy)
                return;

            var root = metaData.FactoryMethod.Invoke(null);
            var action = () => TraversalExtensions.WithChildren((dynamic)root).ToList();

            Assert.Throws<InvalidOperationException>(action);
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void Siblings_ReturnsSiblings_WhenNodeHasSiblings(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            var firstChild = metaData.FactoryMethod.Invoke(root);
            var secondChild = metaData.FactoryMethod.Invoke(root);
            var thirdChild = metaData.FactoryMethod.Invoke(root);
            IEnumerable<object> result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.Siblings((dynamic)firstChild);
                Assert.Equal(2, result.Count());
                Assert.DoesNotContain(firstChild, result);
                Assert.Contains(secondChild, result);
                Assert.Contains(thirdChild, result);
            }

            if (metaData.GetParentFunc != null && metaData.GetChildrenFunc != null)
            {
                result = TraversalExtensions.Siblings((dynamic)firstChild, metaData.GetParentFunc, metaData.GetChildrenFunc);
                Assert.Equal(2, result.Count());
                Assert.DoesNotContain(firstChild, result);
                Assert.Contains(secondChild, result);
                Assert.Contains(thirdChild, result);
            }

            if (metaData.Strategy != null)
            {
                result = TraversalExtensions.Siblings((dynamic)firstChild, metaData.Strategy);
                Assert.Equal(2, result.Count());
                Assert.DoesNotContain(firstChild, result);
                Assert.Contains(secondChild, result);
                Assert.Contains(thirdChild, result);
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void Siblings_ReturnsEmpty_WhenNodeHasNoSiblings(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            var onlyChild = metaData.FactoryMethod.Invoke(root);
            IEnumerable<object> result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.Siblings((dynamic)onlyChild);
                Assert.Empty(result);
            }

            if (metaData.GetParentFunc != null && metaData.GetChildrenFunc != null)
            {
                result = TraversalExtensions.Siblings((dynamic)onlyChild, metaData.GetParentFunc, metaData.GetChildrenFunc);
                Assert.Empty(result);
            }

            if (metaData.Strategy != null)
            {
                result = TraversalExtensions.Siblings((dynamic)onlyChild, metaData.Strategy);
                Assert.Empty(result);
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void Siblings_ReturnsEmpty_WhenNodeIsRoot(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            IEnumerable<object> result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.Siblings((dynamic)root);
                Assert.Empty(result);
            }

            if (metaData.GetParentFunc != null && metaData.GetChildrenFunc != null)
            {
                result = TraversalExtensions.Siblings((dynamic)root, metaData.GetParentFunc, metaData.GetChildrenFunc);
                Assert.Empty(result);
            }

            if (metaData.Strategy != null)
            {
                result = TraversalExtensions.Siblings((dynamic)root, metaData.Strategy);
                Assert.Empty(result);
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void Siblings_ThrowsException_WhenNoStrategyCanBeDerivedFromNodeType(NodeMetaData metaData)
        {
            if (metaData.HasImplicitStrategy)
                return;

            var root = metaData.FactoryMethod.Invoke(null);
            var action = () => TraversalExtensions.Siblings((dynamic)root).ToList();

            Assert.Throws<InvalidOperationException>(action);
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void WithSiblings_ReturnsNodeAndSiblings_WhenNodeHasSiblings(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            var firstChild = metaData.FactoryMethod.Invoke(root);
            var secondChild = metaData.FactoryMethod.Invoke(root);
            var thirdChild = metaData.FactoryMethod.Invoke(root);
            IEnumerable<object> result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.WithSiblings((dynamic)firstChild);
                Assert.Equal(3, result.Count());
                Assert.Equal(firstChild, result.First());
                Assert.Contains(secondChild, result);
                Assert.Contains(thirdChild, result);
            }

            if (metaData.GetParentFunc != null && metaData.GetChildrenFunc != null)
            {
                result = TraversalExtensions.WithSiblings((dynamic)firstChild, metaData.GetParentFunc, metaData.GetChildrenFunc);
                Assert.Equal(3, result.Count());
                Assert.Equal(firstChild, result.First());
                Assert.Contains(secondChild, result);
                Assert.Contains(thirdChild, result);
            }

            if (metaData.Strategy != null)
            {
                result = TraversalExtensions.WithSiblings((dynamic)firstChild, metaData.Strategy);
                Assert.Equal(3, result.Count());
                Assert.Equal(firstChild, result.First());
                Assert.Contains(secondChild, result);
                Assert.Contains(thirdChild, result);
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void WithSiblings_ReturnsOnlyNode_WhenNodeHasNoSiblings(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            var onlyChild = metaData.FactoryMethod.Invoke(root);
            IEnumerable<object> result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.WithSiblings((dynamic)onlyChild);
                Assert.Single(result);
                Assert.Equal(onlyChild, result.First());
            }

            if (metaData.GetParentFunc != null && metaData.GetChildrenFunc != null)
            {
                result = TraversalExtensions.WithSiblings((dynamic)onlyChild, metaData.GetParentFunc, metaData.GetChildrenFunc);
                Assert.Single(result);
                Assert.Equal(onlyChild, result.First());
            }

            if (metaData.Strategy != null)
            {
                result = TraversalExtensions.WithSiblings((dynamic)onlyChild, metaData.Strategy);
                Assert.Single(result);
                Assert.Equal(onlyChild, result.First());
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void WithSiblings_ThrowsException_WhenNoStrategyCanBeDerivedFromNodeType(NodeMetaData metaData)
        {
            if (metaData.HasImplicitStrategy)
                return;

            var root = metaData.FactoryMethod.Invoke(null);
            var action = () => TraversalExtensions.WithSiblings((dynamic)root).ToList();

            Assert.Throws<InvalidOperationException>(action);
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void Ancestors_ReturnsAllAncestors_WhenNodeHasAncestors(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            var child = metaData.FactoryMethod.Invoke(root);
            var grandchild = metaData.FactoryMethod.Invoke(child);
            IEnumerable<object> result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.Ancestors((dynamic)grandchild);
                Assert.Equal(2, result.Count());
                Assert.Equal(child, result.First());
                Assert.Equal(root, result.ElementAt(1));
            }

            if (metaData.GetParentFunc != null)
            {
                result = TraversalExtensions.Ancestors((dynamic)grandchild, metaData.GetParentFunc);
                Assert.Equal(2, result.Count());
                Assert.Equal(child, result.First());
                Assert.Equal(root, result.ElementAt(1));
            }

            if (metaData.Strategy != null)
            {
                result = TraversalExtensions.Ancestors((dynamic)grandchild, metaData.Strategy);
                Assert.Equal(2, result.Count());
                Assert.Equal(child, result.First());
                Assert.Equal(root, result.ElementAt(1));
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void Ancestors_ReturnsEmpty_WhenNodeIsRoot(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            IEnumerable<object> result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.Ancestors((dynamic)root);
                Assert.Empty(result);
            }

            if (metaData.GetParentFunc != null)
            {
                result = TraversalExtensions.Ancestors((dynamic)root, metaData.GetParentFunc);
                Assert.Empty(result);
            }

            if (metaData.Strategy != null)
            {
                result = TraversalExtensions.Ancestors((dynamic)root, metaData.Strategy);
                Assert.Empty(result);
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void Ancestors_ThrowsException_WhenNoStrategyCanBeDerivedFromNodeType(NodeMetaData metaData)
        {
            if (metaData.HasImplicitStrategy)
                return;

            var root = metaData.FactoryMethod.Invoke(null);
            var action = () => TraversalExtensions.Ancestors((dynamic)root).ToList();

            Assert.Throws<InvalidOperationException>(action);
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void WithAncestors_ReturnsNodeAndAncestors_WhenNodeHasAncestors(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            var child = metaData.FactoryMethod.Invoke(root);
            var grandchild = metaData.FactoryMethod.Invoke(child);
            IEnumerable<object> result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.WithAncestors((dynamic)grandchild);
                Assert.Equal(3, result.Count());
                Assert.Equal(grandchild, result.First());
                Assert.Equal(child, result.ElementAt(1));
                Assert.Equal(root, result.ElementAt(2));
            }

            if (metaData.GetParentFunc != null)
            {
                result = TraversalExtensions.WithAncestors((dynamic)grandchild, metaData.GetParentFunc);
                Assert.Equal(3, result.Count());
                Assert.Equal(grandchild, result.First());
                Assert.Equal(child, result.ElementAt(1));
                Assert.Equal(root, result.ElementAt(2));
            }

            if (metaData.Strategy != null)
            {
                result = TraversalExtensions.WithAncestors((dynamic)grandchild, metaData.Strategy);
                Assert.Equal(3, result.Count());
                Assert.Equal(grandchild, result.First());
                Assert.Equal(child, result.ElementAt(1));
                Assert.Equal(root, result.ElementAt(2));
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void WithAncestors_ReturnsOnlyNode_WhenNodeIsRoot(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            IEnumerable<object> result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.WithAncestors((dynamic)root);
                Assert.Single(result);
                Assert.Equal(root, result.First());
            }

            if (metaData.GetParentFunc != null)
            {
                result = TraversalExtensions.WithAncestors((dynamic)root, metaData.GetParentFunc);
                Assert.Single(result);
                Assert.Equal(root, result.First());
            }

            if (metaData.Strategy != null)
            {
                result = TraversalExtensions.WithAncestors((dynamic)root, metaData.Strategy);
                Assert.Single(result);
                Assert.Equal(root, result.First());
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void WithAncestors_ThrowsException_WhenNoStrategyCanBeDerivedFromNodeType(NodeMetaData metaData)
        {
            if (metaData.HasImplicitStrategy)
                return;

            var root = metaData.FactoryMethod.Invoke(null);
            var action = () => TraversalExtensions.WithAncestors((dynamic)root).ToList();

            Assert.Throws<InvalidOperationException>(action);
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void Descendants_ReturnsAllDescendants_WhenNodeHasDescendants(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            var child = metaData.FactoryMethod.Invoke(root);
            var grandchild = metaData.FactoryMethod.Invoke(child);
            var secondChild = metaData.FactoryMethod.Invoke(root);
            IEnumerable<object> result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.Descendants((dynamic)root);
                Assert.Equal(3, result.Count());
                Assert.Contains(child, result);
                Assert.Contains(grandchild, result);
                Assert.Contains(secondChild, result);

                result = TraversalExtensions.Descendants((dynamic)child);
                Assert.Single(result);
                Assert.Equal(grandchild, result.First());
            }

            if (metaData.GetChildrenFunc != null)
            {
                result = TraversalExtensions.Descendants((dynamic)root, metaData.GetChildrenFunc);
                Assert.Equal(3, result.Count());
                Assert.Contains(child, result);
                Assert.Contains(grandchild, result);
                Assert.Contains(secondChild, result);

                result = TraversalExtensions.Descendants((dynamic)child, metaData.GetChildrenFunc);
                Assert.Single(result);
                Assert.Equal(grandchild, result.First());
            }

            if (metaData.Strategy != null)
            {
                result = TraversalExtensions.Descendants((dynamic)root, metaData.Strategy);
                Assert.Equal(3, result.Count());
                Assert.Contains(child, result);
                Assert.Contains(grandchild, result);
                Assert.Contains(secondChild, result);

                result = TraversalExtensions.Descendants((dynamic)child, metaData.Strategy);
                Assert.Single(result);
                Assert.Equal(grandchild, result.First());
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void Descendants_ReturnsEmpty_WhenNodeHasNoDescendants(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            var child = metaData.FactoryMethod.Invoke(root);
            IEnumerable<object> result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.Descendants((dynamic)child);
                Assert.Empty(result);
            }

            if (metaData.GetChildrenFunc != null)
            {
                result = TraversalExtensions.Descendants((dynamic)child, metaData.GetChildrenFunc);
                Assert.Empty(result);
            }

            if (metaData.Strategy != null)
            {
                result = TraversalExtensions.Descendants((dynamic)child, metaData.Strategy);
                Assert.Empty(result);
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void Descendants_ThrowsException_WhenNoStrategyCanBeDerivedFromNodeType(NodeMetaData metaData)
        {
            if (metaData.HasImplicitStrategy)
                return;

            var root = metaData.FactoryMethod.Invoke(null);
            var action = () => TraversalExtensions.Descendants((dynamic)root).ToList();

            Assert.Throws<InvalidOperationException>(action);
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void WithDescendants_ReturnsNodeAndAllDescendants_WhenNodeHasDescendants(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            var child = metaData.FactoryMethod.Invoke(root);
            var grandchild = metaData.FactoryMethod.Invoke(child);
            var secondChild = metaData.FactoryMethod.Invoke(root);
            IEnumerable<object> result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.WithDescendants((dynamic)root);
                Assert.Equal(4, result.Count());
                Assert.Equal(root, result.First());
                Assert.Contains(child, result);
                Assert.Contains(grandchild, result);
                Assert.Contains(secondChild, result);
            }

            if (metaData.GetChildrenFunc != null)
            {
                result = TraversalExtensions.WithDescendants((dynamic)root, metaData.GetChildrenFunc);
                Assert.Equal(4, result.Count());
                Assert.Equal(root, result.First());
                Assert.Contains(child, result);
                Assert.Contains(grandchild, result);
                Assert.Contains(secondChild, result);
            }

            if (metaData.Strategy != null)
            {
                result = TraversalExtensions.WithDescendants((dynamic)root, metaData.Strategy);
                Assert.Equal(4, result.Count());
                Assert.Equal(root, result.First());
                Assert.Contains(child, result);
                Assert.Contains(grandchild, result);
                Assert.Contains(secondChild, result);
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void WithDescendants_ReturnsOnlyNode_WhenNodeHasNoDescendants(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            var child = metaData.FactoryMethod.Invoke(root);
            IEnumerable<object> result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.WithDescendants((dynamic)child);
                Assert.Single(result);
                Assert.Equal(child, result.First());
            }

            if (metaData.GetChildrenFunc != null)
            {
                result = TraversalExtensions.WithDescendants((dynamic)child, metaData.GetChildrenFunc);
                Assert.Single(result);
                Assert.Equal(child, result.First());
            }

            if (metaData.Strategy != null)
            {
                result = TraversalExtensions.WithDescendants((dynamic)child, metaData.Strategy);
                Assert.Single(result);
                Assert.Equal(child, result.First());
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void WithDescendants_ThrowsException_WhenNoStrategyCanBeDerivedFromNodeType(NodeMetaData metaData)
        {
            if (metaData.HasImplicitStrategy)
                return;

            var root = metaData.FactoryMethod.Invoke(null);
            var action = () => TraversalExtensions.WithDescendants((dynamic)root).ToList();

            Assert.Throws<InvalidOperationException>(action);
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void Leaves_ReturnsAllLeafNodes_WhenNodeHasSubtree(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            var firstChild = metaData.FactoryMethod.Invoke(root);
            var grandchild = metaData.FactoryMethod.Invoke(firstChild);
            var secondChild = metaData.FactoryMethod.Invoke(root);
            IEnumerable<object> result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.Leaves((dynamic)root);
                Assert.Equal(2, result.Count());
                Assert.Contains(grandchild, result);
                Assert.Contains(secondChild, result);
                Assert.DoesNotContain(root, result);
                Assert.DoesNotContain(firstChild, result);
            }

            if (metaData.GetChildrenFunc != null)
            {
                result = TraversalExtensions.Leaves((dynamic)root, metaData.GetChildrenFunc);
                Assert.Equal(2, result.Count());
                Assert.Contains(grandchild, result);
                Assert.Contains(secondChild, result);
                Assert.DoesNotContain(root, result);
                Assert.DoesNotContain(firstChild, result);
            }

            if (metaData.Strategy != null)
            {
                result = TraversalExtensions.Leaves((dynamic)root, metaData.Strategy);
                Assert.Equal(2, result.Count());
                Assert.Contains(grandchild, result);
                Assert.Contains(secondChild, result);
                Assert.DoesNotContain(root, result);
                Assert.DoesNotContain(firstChild, result);
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void Leaves_ReturnsSelf_WhenNodeIsLeaf(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            var child = metaData.FactoryMethod.Invoke(root);
            IEnumerable<object> result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.Leaves((dynamic)child);
                Assert.Single(result);
                Assert.Equal(child, result.First());
            }

            if (metaData.GetChildrenFunc != null)
            {
                result = TraversalExtensions.Leaves((dynamic)child, metaData.GetChildrenFunc);
                Assert.Single(result);
                Assert.Equal(child, result.First());
            }

            if (metaData.Strategy != null)
            {
                result = TraversalExtensions.Leaves((dynamic)child, metaData.Strategy);
                Assert.Single(result);
                Assert.Equal(child, result.First());
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void Leaves_ThrowsException_WhenNoStrategyCanBeDerivedFromNodeType(NodeMetaData metaData)
        {
            if (metaData.HasImplicitStrategy)
                return;

            var root = metaData.FactoryMethod.Invoke(null);
            var action = () => TraversalExtensions.Leaves((dynamic)root).ToList();

            Assert.Throws<InvalidOperationException>(action);
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void InnerNodes_ReturnsAllInnerDescendantNodes_WhenNodeHasInnerDescendants(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            var child = metaData.FactoryMethod.Invoke(root);
            var grandchild = metaData.FactoryMethod.Invoke(child);
            var greatGrandchild = metaData.FactoryMethod.Invoke(grandchild);
            var secondChild = metaData.FactoryMethod.Invoke(root);
            IEnumerable<object> result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.InnerNodes((dynamic)root);
                Assert.Equal(2, result.Count());
                Assert.Contains(child, result);
                Assert.Contains(grandchild, result);
                Assert.DoesNotContain(root, result);
                Assert.DoesNotContain(greatGrandchild, result);
                Assert.DoesNotContain(secondChild, result);
            }

            if (metaData.GetParentFunc != null && metaData.GetChildrenFunc != null)
            {
                result = TraversalExtensions.InnerNodes((dynamic)root, metaData.GetParentFunc, metaData.GetChildrenFunc);
                Assert.Equal(2, result.Count());
                Assert.Contains(child, result);
                Assert.Contains(grandchild, result);
                Assert.DoesNotContain(root, result);
                Assert.DoesNotContain(greatGrandchild, result);
                Assert.DoesNotContain(secondChild, result);
            }

            if (metaData.Strategy != null)
            {
                result = TraversalExtensions.InnerNodes((dynamic)root, metaData.Strategy);
                Assert.Equal(2, result.Count());
                Assert.Contains(child, result);
                Assert.Contains(grandchild, result);
                Assert.DoesNotContain(root, result);
                Assert.DoesNotContain(greatGrandchild, result);
                Assert.DoesNotContain(secondChild, result);
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void InnerNodes_ReturnsEmpty_WhenNodeHasNoInnerDescendants(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            var child = metaData.FactoryMethod.Invoke(root);
            IEnumerable<object> result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.InnerNodes((dynamic)root);
                Assert.Empty(result);
            }

            if (metaData.GetParentFunc != null && metaData.GetChildrenFunc != null)
            {
                result = TraversalExtensions.InnerNodes((dynamic)root, metaData.GetParentFunc, metaData.GetChildrenFunc);
                Assert.Empty(result);
            }

            if (metaData.Strategy != null)
            {
                result = TraversalExtensions.InnerNodes((dynamic)root, metaData.Strategy);
                Assert.Empty(result);
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void InnerNodes_ThrowsException_WhenNoStrategyCanBeDerivedFromNodeType(NodeMetaData metaData)
        {
            if (metaData.HasImplicitStrategy)
                return;

            var root = metaData.FactoryMethod.Invoke(null);
            var action = () => TraversalExtensions.InnerNodes((dynamic)root).ToList();

            Assert.Throws<InvalidOperationException>(action);
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void WithInnerNodes_ReturnsNodeAndInnerDescendants_WhenNodeHasInnerDescendants(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            var child = metaData.FactoryMethod.Invoke(root);
            var grandchild = metaData.FactoryMethod.Invoke(child);
            var greatGrandchild = metaData.FactoryMethod.Invoke(grandchild);
            var secondChild = metaData.FactoryMethod.Invoke(root);
            IEnumerable<object> result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.WithInnerNodes((dynamic)root);
                Assert.Equal(3, result.Count());
                Assert.Equal(root, result.First());
                Assert.Contains(child, result);
                Assert.Contains(grandchild, result);
                Assert.DoesNotContain(greatGrandchild, result);
                Assert.DoesNotContain(secondChild, result);
            }

            if (metaData.GetParentFunc != null && metaData.GetChildrenFunc != null)
            {
                result = TraversalExtensions.WithInnerNodes((dynamic)root, metaData.GetParentFunc, metaData.GetChildrenFunc);
                Assert.Equal(3, result.Count());
                Assert.Equal(root, result.First());
                Assert.Contains(child, result);
                Assert.Contains(grandchild, result);
                Assert.DoesNotContain(greatGrandchild, result);
                Assert.DoesNotContain(secondChild, result);
            }

            if (metaData.Strategy != null)
            {
                result = TraversalExtensions.WithInnerNodes((dynamic)root, metaData.Strategy);
                Assert.Equal(3, result.Count());
                Assert.Equal(root, result.First());
                Assert.Contains(child, result);
                Assert.Contains(grandchild, result);
                Assert.DoesNotContain(greatGrandchild, result);
                Assert.DoesNotContain(secondChild, result);
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void WithInnerNodes_ReturnsOnlyNode_WhenNodeHasNoInnerDescendants(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            var child = metaData.FactoryMethod.Invoke(root);
            IEnumerable<object> result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.WithInnerNodes((dynamic)root);
                Assert.Single(result);
                Assert.Equal(root, result.First());
            }

            if (metaData.GetParentFunc != null && metaData.GetChildrenFunc != null)
            {
                result = TraversalExtensions.WithInnerNodes((dynamic)root, metaData.GetParentFunc, metaData.GetChildrenFunc);
                Assert.Single(result);
                Assert.Equal(root, result.First());
            }

            if (metaData.Strategy != null)
            {
                result = TraversalExtensions.WithInnerNodes((dynamic)root, metaData.Strategy);
                Assert.Single(result);
                Assert.Equal(root, result.First());
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void WithInnerNodes_ThrowsException_WhenNoStrategyCanBeDerivedFromNodeType(NodeMetaData metaData)
        {
            if (metaData.HasImplicitStrategy)
                return;

            var root = metaData.FactoryMethod.Invoke(null);
            var action = () => TraversalExtensions.WithInnerNodes((dynamic)root).ToList();

            Assert.Throws<InvalidOperationException>(action);
        }

        #endregion

        #region Traverse Methods

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void Traverse_ReturnsAllNodesStartingFromSingleNode(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            var firstChild = metaData.FactoryMethod.Invoke(root);
            var firstGrandchild = metaData.FactoryMethod.Invoke(firstChild);
            var secondChild = metaData.FactoryMethod.Invoke(root);
            IEnumerable<object> result;

            if (metaData.HasImplicitStrategy)
            {
                result = TraversalExtensions.Traverse((dynamic)root);
                Assert.Equal(4, result.Count());
                Assert.Equal(root, result.First());
                Assert.Contains(firstChild, result);
                Assert.Contains(firstGrandchild, result);
                Assert.Contains(secondChild, result);
            }

            if (metaData.GetChildrenFunc != null)
            {
                result = TraversalExtensions.Traverse((dynamic)root, metaData.GetChildrenFunc);
                Assert.Equal(4, result.Count());
                Assert.Equal(root, result.First());
                Assert.Contains(firstChild, result);
                Assert.Contains(firstGrandchild, result);
                Assert.Contains(secondChild, result);
            }

            if (metaData.Strategy != null)
            {
                result = TraversalExtensions.Traverse((dynamic)root, metaData.Strategy);
                Assert.Equal(4, result.Count());
                Assert.Equal(root, result.First());
                Assert.Contains(firstChild, result);
                Assert.Contains(firstGrandchild, result);
                Assert.Contains(secondChild, result);
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void Traverse_RespectsTraversalMode(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            var firstChild = metaData.FactoryMethod.Invoke(root);
            var firstGrandchild = metaData.FactoryMethod.Invoke(firstChild);
            var secondChild = metaData.FactoryMethod.Invoke(root);

            if (metaData.HasImplicitStrategy)
            {
                var depthFirst = TraversalExtensions.Traverse((dynamic)root, TraversalMode.DepthFirst).ToList();
                var breadthFirst = TraversalExtensions.Traverse((dynamic)root, TraversalMode.BreadthFirst).ToList();

                Assert.Equal(root, depthFirst.First());
                Assert.Equal(root, breadthFirst.First());

                Assert.Equal(firstGrandchild, depthFirst.Last());
                Assert.Equal(firstGrandchild, breadthFirst.Last());

                Assert.NotEqual(depthFirst.ElementAt(1), breadthFirst.ElementAt(1));
                Assert.Contains(firstChild, depthFirst);
                Assert.Contains(secondChild, depthFirst);
                Assert.Contains(firstChild, breadthFirst);
                Assert.Contains(secondChild, breadthFirst);
            }

            if (metaData.GetChildrenFunc != null)
            {
                var depthFirst = TraversalExtensions.Traverse((dynamic)root, metaData.GetChildrenFunc, TraversalMode.DepthFirst).ToList();
                var breadthFirst = TraversalExtensions.Traverse((dynamic)root, metaData.GetChildrenFunc, TraversalMode.BreadthFirst).ToList();

                Assert.NotEqual(depthFirst.ElementAt(1), breadthFirst.ElementAt(1));
            }

            if (metaData.Strategy != null)
            {
                var depthFirst = TraversalExtensions.Traverse((dynamic)root, metaData.Strategy, TraversalMode.DepthFirst).ToList();
                var breadthFirst = TraversalExtensions.Traverse((dynamic)root, metaData.Strategy, TraversalMode.BreadthFirst).ToList();

                Assert.NotEqual(depthFirst.ElementAt(1), breadthFirst.ElementAt(1));
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void Traverse_RespectsComparer(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            var firstChild = metaData.FactoryMethod.Invoke(root);
            var secondChild = metaData.FactoryMethod.Invoke(root);
            var rank = new Dictionary<object, int>
            {
                [root] = 0,
                [firstChild] = 2,
                [secondChild] = 1,
            };
            var comparer = Comparer<object>.Create((x, y) => rank[x].CompareTo(rank[y]));

            if (metaData.HasImplicitStrategy)
            {
                var result = TraversalExtensions.Traverse((dynamic)root, comparer).ToList();
                Assert.Equal(root, result[0]);
                Assert.Equal(secondChild, result[1]);
                Assert.Equal(firstChild, result[2]);
            }

            if (metaData.GetChildrenFunc != null)
            {
                var result = TraversalExtensions.Traverse((dynamic)root, metaData.GetChildrenFunc, comparer).ToList();
                Assert.Equal(root, result[0]);
                Assert.Equal(secondChild, result[1]);
                Assert.Equal(firstChild, result[2]);
            }

            if (metaData.Strategy != null)
            {
                var result = TraversalExtensions.Traverse((dynamic)root, metaData.Strategy, comparer).ToList();
                Assert.Equal(root, result[0]);
                Assert.Equal(secondChild, result[1]);
                Assert.Equal(firstChild, result[2]);
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void Traverse_RespectsCustomSelector(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            var firstChild = metaData.FactoryMethod.Invoke(root);
            var secondChild = metaData.FactoryMethod.Invoke(root);

            var selector = new QueueCandidateSelector();

            if (metaData.GetChildrenFunc != null)
            {
                var result = TraversalExtensions.Traverse((dynamic)root, metaData.GetChildrenFunc, selector).ToList();
                Assert.Equal(root, result[0]);
                Assert.Equal(firstChild, result[1]);
                Assert.Equal(secondChild, result[2]);
            }

            if (metaData.Strategy != null)
            {
                var strategySelector = new QueueCandidateSelector();
                var result = TraversalExtensions.Traverse((dynamic)root, metaData.Strategy, strategySelector).ToList();
                Assert.Equal(root, result[0]);
                Assert.Equal(firstChild, result[1]);
                Assert.Equal(secondChild, result[2]);
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void Traverse_ReturnsAllNodesStartingFromMultipleNodes(NodeMetaData metaData)
        {
            var firstRoot = metaData.FactoryMethod.Invoke(null);
            var firstChild = metaData.FactoryMethod.Invoke(firstRoot);
            var secondRoot = metaData.FactoryMethod.Invoke(null);
            var secondChild = metaData.FactoryMethod.Invoke(secondRoot);
            var roots = new List<object> { firstRoot, secondRoot };

            if (metaData.HasImplicitStrategy)
            {
                var result = TraversalExtensions.Traverse((dynamic)roots).ToList();
                Assert.Equal(4, result.Count);
                Assert.Contains(firstRoot, result);
                Assert.Contains(firstChild, result);
                Assert.Contains(secondRoot, result);
                Assert.Contains(secondChild, result);
            }

            if (metaData.GetChildrenFunc != null)
            {
                var result = TraversalExtensions.Traverse((dynamic)roots, metaData.GetChildrenFunc).ToList();
                Assert.Equal(4, result.Count);
                Assert.Contains(firstRoot, result);
                Assert.Contains(firstChild, result);
                Assert.Contains(secondRoot, result);
                Assert.Contains(secondChild, result);
            }

            if (metaData.Strategy != null)
            {
                var result = TraversalExtensions.Traverse((dynamic)roots, metaData.Strategy).ToList();
                Assert.Equal(4, result.Count);
                Assert.Contains(firstRoot, result);
                Assert.Contains(firstChild, result);
                Assert.Contains(secondRoot, result);
                Assert.Contains(secondChild, result);
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void Traverse_InvokesCallback_ForEachVisitedNode(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            var child = metaData.FactoryMethod.Invoke(root);
            var grandchild = metaData.FactoryMethod.Invoke(child);

            if (metaData.HasImplicitStrategy)
            {
                var visited = new List<object>();
                Action<dynamic> action = visited.Add;

                var actionType = typeof(Action<>).MakeGenericType(root.GetType());
                var paramTypes = new Type[] { root.GetType(), actionType };

                var method = typeof(TraversalExtensions)
                    .GetMethod(nameof(TraversalExtensions.Traverse))!
                    .MakeGenericMethod(root.GetType());
                                
                method.Invoke(null, [root, action]);

                Assert.Equal(3, visited.Count);
                Assert.Equal(root, visited.First());
                Assert.Contains(child, visited);
                Assert.Contains(grandchild, visited);
            }

            if (metaData.GetChildrenFunc != null)
            {
                var visited = new List<object>();
                Action<dynamic> action = visited.Add;

                var actionType = typeof(Action<>).MakeGenericType(root.GetType());
                var paramTypes = new Type[] { root.GetType(), metaData.GetChildrenFunc.GetType(), actionType };

                var method = typeof(TraversalExtensions)
                    .GetMethod(nameof(TraversalExtensions.Traverse), paramTypes)!
                    .MakeGenericMethod(root.GetType());

                method.Invoke(null, [root, metaData.GetChildrenFunc, action]);

                Assert.Equal(3, visited.Count);
                Assert.Equal(root, visited.First());
                Assert.Contains(child, visited);
                Assert.Contains(grandchild, visited);
            }

            if (metaData.Strategy != null)
            {
                var visited = new List<object>();
                Action<dynamic> action = visited.Add;

                var actionType = typeof(Action<>).MakeGenericType(root.GetType());
                var paramTypes = new Type[] { root.GetType(), actionType, metaData.Strategy.GetType() };

                var method = typeof(TraversalExtensions)
                    .GetMethod(nameof(TraversalExtensions.Traverse), paramTypes)!
                    .MakeGenericMethod(root.GetType());

                method.Invoke(null, [root, action, metaData.Strategy]);

                Assert.Equal(3, visited.Count);
                Assert.Equal(root, visited.First());
                Assert.Contains(child, visited);
                Assert.Contains(grandchild, visited);
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public async Task TraverseAsync_InvokesCallback_ForEachVisitedNode(NodeMetaData metaData)
        {
            var root = metaData.FactoryMethod.Invoke(null);
            var child = metaData.FactoryMethod.Invoke(root);
            var grandchild = metaData.FactoryMethod.Invoke(child);

            if (metaData.HasImplicitStrategy)
            {
                var visited = new List<object>();
                await TraversalExtensions.TraverseAsync((dynamic)root, (Action<dynamic>)(x => visited.Add(x)));
                Assert.Equal(3, visited.Count);
                Assert.Equal(root, visited.First());
                Assert.Contains(child, visited);
                Assert.Contains(grandchild, visited);
            }

            if (metaData.GetChildrenFunc != null)
            {
                var visited = new List<object>();
                await TraversalExtensions.TraverseAsync((dynamic)root, metaData.GetChildrenFunc, (Action<dynamic>)(x => visited.Add(x)));
                Assert.Equal(3, visited.Count);
                Assert.Equal(root, visited.First());
                Assert.Contains(child, visited);
                Assert.Contains(grandchild, visited);
            }

            if (metaData.Strategy != null)
            {
                var visited = new List<object>();
                await TraversalExtensions.TraverseAsync((dynamic)root, (Action<dynamic>)(x => visited.Add(x)), metaData.Strategy);
                Assert.Equal(3, visited.Count);
                Assert.Equal(root, visited.First());
                Assert.Contains(child, visited);
                Assert.Contains(grandchild, visited);
            }
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public void Traverse_ThrowsException_WhenNoStrategyCanBeDerivedFromNodeType(NodeMetaData metaData)
        {
            if (metaData.HasImplicitStrategy)
                return;

            var root = metaData.FactoryMethod.Invoke(null);
            var action = () => TraversalExtensions.Traverse((dynamic)root).ToList();

            Assert.Throws<InvalidOperationException>(action);
        }

        [Theory]
        [ClassData(typeof(Helpers.TheoryData))]
        public async Task TraverseAsync_ThrowsException_WhenNoStrategyCanBeDerivedFromNodeType(NodeMetaData metaData)
        {
            if (metaData.HasImplicitStrategy)
                return;

            var root = metaData.FactoryMethod.Invoke(null);

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await TraversalExtensions.TraverseAsync((dynamic)root, (Action<object>)(_ => { }));
            });
        }

        private class QueueCandidateSelector : ICandidateSelector<object>
        {
            private readonly Queue<object> _queue = new Queue<object>();

            public bool HasItems => _queue.Count > 0;

            public void Add(object item)
            {
                _queue.Enqueue(item);
            }

            public object Next()
            {
                return _queue.Dequeue();
            }

            public void Clear()
            {
                _queue.Clear();
            }
        }

        #endregion
    }
}