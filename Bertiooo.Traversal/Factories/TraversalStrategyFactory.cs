using Bertiooo.Traversal.Adapters;
using Bertiooo.Traversal.Attributes;
using Bertiooo.Traversal.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Bertiooo.Traversal.Factories
{
    public class TraversalStrategyFactory
    {
        private static readonly Lazy<TraversalStrategyFactory> lazy = 
            new Lazy<TraversalStrategyFactory>(() => new TraversalStrategyFactory());

        public static TraversalStrategyFactory Instance => lazy.Value;

        private TraversalStrategyFactory()
        {
        }

        public ITraversalStrategy<T> CreateStrategy<T>()
        {
            var nodeType = typeof(T);
            var traversalType = typeof(ITraversable<T>);

            // Check if type implements ITraversable<T>
            if (traversalType.IsAssignableFrom(nodeType))
            {
                var strategyType = typeof(TraversableTraversalStrategy<>)
                    .MakeGenericType(nodeType);

                var strategy = Activator.CreateInstance(strategyType);
                return (ITraversalStrategy<T>)strategy;
            }

            // Check for TraversalStrategyAttribute
            var traversalStrategyAttr = nodeType.GetCustomAttribute<TraversalStrategyAttribute>();
            if (traversalStrategyAttr != null)
            {
                var strategyType = traversalStrategyAttr.Type;
                
                // Validate that the strategy type implements ITraversalStrategy<T>
                var expectedInterface = typeof(ITraversalStrategy<T>);
                if (!expectedInterface.IsAssignableFrom(strategyType))
                {
                    throw new InvalidOperationException(
                        $"The strategy type '{strategyType.Name}' specified in TraversalStrategyAttribute does not implement ITraversalStrategy<{nodeType.Name}>.");
                }

                var strategy = Activator.CreateInstance(strategyType);
                return (ITraversalStrategy<T>)strategy!;
            }

            // Check for ParentAttribute and ChildrenAttribute on properties
            var properties = nodeType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var parentProperty = properties.FirstOrDefault(p => p.GetCustomAttribute<ParentAttribute>() != null);
            var childrenProperty = properties.FirstOrDefault(p => p.GetCustomAttribute<ChildrenAttribute>() != null);

            if (parentProperty != null || childrenProperty != null)
            {
                // Validate parent property type compatibility
                if (parentProperty != null)
                {
                    var parentPropertyType = parentProperty.PropertyType;
                    // Check if the property type is T or nullable T
                    if (!nodeType.IsAssignableFrom(parentPropertyType) && 
                        !(Nullable.GetUnderlyingType(parentPropertyType) == nodeType))
                    {
                        throw new InvalidOperationException(
                            $"The property '{parentProperty.Name}' marked with [Parent] has type '{parentPropertyType.Name}' which is not compatible with the node type '{nodeType.Name}'. ");
                    }
                }

                // Validate children property type compatibility
                if (childrenProperty != null)
                {
                    var childrenPropertyType = childrenProperty.PropertyType;
                    var expectedChildrenType = typeof(IEnumerable<T>);
                    
                    // Check if the property type implements IEnumerable<T>
                    if (!expectedChildrenType.IsAssignableFrom(childrenPropertyType))
                    {
                        throw new InvalidOperationException(
                            $"The property '{childrenProperty.Name}' marked with [Children] has type '{childrenPropertyType.Name}' which is not compatible with 'IEnumerable<{nodeType.Name}>'. ");
                    }
                }

                Func<T, T?> getParentFunc;
                if (parentProperty != null)
                {
                    getParentFunc = node => (T?)parentProperty.GetValue(node);
                }
                else
                {
                    getParentFunc = node => default;
                }

                Func<T, IEnumerable<T>> getChildrenFunc;
                if (childrenProperty != null)
                {
                    getChildrenFunc = node =>
                    {
                        var value = childrenProperty.GetValue(node);
                        return (IEnumerable<T>?)value ?? Enumerable.Empty<T>();
                    };
                }
                else
                {
                    getChildrenFunc = node => Enumerable.Empty<T>();
                }

                return this.CreateStrategy(getParentFunc, getChildrenFunc);
            }

            throw new InvalidOperationException($"No traversal strategy can be derived from type '{nodeType.Name}'.");
        }

        public ITraversalStrategy<T> CreateStrategy<T>(Func<T, T?> getParentFunc)
        {
            return this.CreateStrategy(getParentFunc, n => Enumerable.Empty<T>());
        }

        public ITraversalStrategy<T> CreateStrategy<T>(Func<T, IEnumerable<T>> getChildrenFunc)
        {
            return this.CreateStrategy(n => default, getChildrenFunc);
        }

        public ITraversalStrategy<T> CreateStrategy<T>(
            Func<T, T?> getParentFunc,
            Func<T, IEnumerable<T>> getChildrenFunc)
        {
            return new DefaultTraversalStrategy<T>(
                getParentFunc,
                getChildrenFunc);
        }
    }
}
