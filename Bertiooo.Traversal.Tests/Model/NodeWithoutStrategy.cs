using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Model
{
    public class NodeWithoutStrategy
    {
        public NodeWithoutStrategy? Parent { get; set; }

        public IEnumerable<NodeWithoutStrategy> Children { get; set; } = Enumerable.Empty<NodeWithoutStrategy>();
    }
}
