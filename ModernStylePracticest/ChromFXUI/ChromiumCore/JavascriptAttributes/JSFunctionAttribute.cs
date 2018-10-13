using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromFXUI
{
	[AttributeUsage(AttributeTargets.Method, Inherited = true)]
	public class JSFunctionAttribute : JSPropertyAttribute
	{
		//public JSFunctionAttribute(string jsName) : base(jsName) { }
	}
}
