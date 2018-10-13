using ChromeFX.Windows.Imports;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ChromeFX.WinForm.ShadowForm
{
	//你不需要知道这里面发生了什么。
	//YOU DO NOT NEED HAVE TO KNOW WHAT IS HAPPEND HERE.
	internal static class CONSTS
	{
		internal const string CLASS_NAME = "ChromeFXShadowFormWindow";
		internal const int SHOW_BORDER_DELAY = 250;

	}
	internal enum ShadowFormDockPositon
	{
		Left = 0,
		Top = 1,
		Right = 2,
		Bottom = 3
	}

	internal delegate void ShadowFormResizeEventHandler(object sender, ShadowFormResizeArgs args);
	internal class ShadowFormResizeArgs : EventArgs
	{
		private readonly ShadowFormDockPositon _side;
		private readonly HitTest _mode;

		public ShadowFormDockPositon Side
		{
			get { return _side; }
		}

		public HitTest Mode
		{
			get { return _mode; }
		}

		internal ShadowFormResizeArgs(ShadowFormDockPositon side, HitTest mode)
		{
			_side = side;
			_mode = mode;
		}
	}

	public interface IShadowForm : IDisposable
	{
		Color ActiveColor { get; set; }
		Color InactiveColor { get; set; }
		bool IsInitialized { get; }
		bool IsEnabled { get; }
		void InitializeShadows();
		void Enable(bool enable);
		void SetOwner(IntPtr owner);
		void SetFocus();
		void KillFocus();

	}
}
