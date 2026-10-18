using Bertiooo.Traversal.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Model
{
    internal class NodeWithPropertyAttributes
    {
        [Parent]
        public NodeWithPropertyAttributes? Parent { get; set; }

        [Children]
        public IEnumerable<NodeWithPropertyAttributes> Children { get; set; } = Enumerable.Empty<NodeWithPropertyAttributes>();
    }
}
