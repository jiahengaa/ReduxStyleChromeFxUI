using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Chromium.WebBrowser {
	class CefException : Exception {
		internal CefException(string message) : base(message) { }
	}
}


namespace ChromFXUI
{
	using Chromium.WebBrowser;
	class ChromFXUIException : CefException
	{
		internal ChromFXUIException(string message) : base(message) { }
	}
}