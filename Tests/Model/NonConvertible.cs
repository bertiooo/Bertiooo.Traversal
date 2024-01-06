using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Model
{
	public class NonConvertible
	{
		public string? Name { get; set; }

		public NonConvertible? Parent { get; set; }

		public IEnumerable<NonConvertible> Children { get; set; } = Enumerable.Empty<NonConvertible>();
	}
}
