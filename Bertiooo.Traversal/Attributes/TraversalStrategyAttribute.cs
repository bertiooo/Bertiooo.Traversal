using System;
using System.Collections.Generic;
using System.Text;

namespace Bertiooo.Traversal.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class TraversalStrategyAttribute : Attribute
    {
        private readonly Type _type;

        public TraversalStrategyAttribute(Type type) 
        {
            _type = type ?? throw new ArgumentNullException(nameof(type));
        }

        public Type Type => _type;
    }
}
