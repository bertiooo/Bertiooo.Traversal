using Bertiooo.Traversal.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Model
{
    internal class NodeWithInvalidPropertyAttributes
    {
        [Parent]
        public object? Parent { get; set; }

        [Children]
        public IEnumerable<object> Children { get; set; } = Enumerable.Empty<object>();
    }
}
