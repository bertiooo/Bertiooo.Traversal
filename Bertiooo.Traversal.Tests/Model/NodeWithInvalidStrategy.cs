using Bertiooo.Traversal.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Model
{
    [TraversalStrategy(typeof(object))]
    internal class NodeWithInvalidStrategy
    {
    }
}
