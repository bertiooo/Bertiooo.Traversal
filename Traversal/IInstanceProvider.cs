using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bertiooo.Traversal
{
	public interface IInstanceProvider<out T>
	{
		T Instance { get; }
	}
}
