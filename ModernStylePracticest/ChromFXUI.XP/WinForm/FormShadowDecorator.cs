using ChromeFX.Windows.Imports;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChromeFX.WinForm.ShadowForm
{
	/// <summary>
	/// 窗口投影装饰器
	/// </summary>
	public class ShadowFormDecorator : NativeWindow, IShadowForm
	{
		private IntPtr parentWindowHWnd => parentWindow.Handle;
		private Form parentWindow;

		private ShadowFormElement topShadowForm;
		private ShadowFormElement leftShadowForm;
		private ShadowFormElement bottomShadowForm;
		private ShadowFormElement rightShadowForm;

		private WINDOWPOS lastLocation;

		private readonly List<ShadowFormElement> shadows = new List<ShadowFormElement>();
		private Color activeColor = Color.Black;
		private Color inactiveColor = Color.Black;
		private bool isEnabled;
		private bool isFocused = false;
		private bool isWindowMinimized = false;
		private bool isAnimationDelayed = false;

		private bool isInitialized = false;

		private Bitmap[] cachedImages;

		internal Bitmap ActiveBitmapTemplate => cachedImages?[1];
		internal Bitmap InactiveBitmapTemplate => cachedImages?[2];




		public Color ActiveColor
		{
			get
			{
				return activeColor;
			}

			set
			{

				InitializeBitmapCache();


				activeColor = value;

				if (!isInitialized) return;

				foreach (ShadowFormElement sideShadow in shadows)
				{
					sideShadow.UpdateShadow();
				}
			}
		}
		public Color InactiveColor
		{
			get
			{
				return inactiveColor;
			}

			set
			{
				InitializeBitmapCache();


				inactiveColor = value;

				if (!isInitialized) return;

				foreach (ShadowFormElement sideShadow in shadows)
				{
					sideShadow.UpdateShadow();
				}
			}
		}
		public bool IsInitialized => isInitialized;
		public bool IsEnabled => isEnabled;
		public void InitializeShadows()
		{
			topShadowForm = new ShadowFormElement(ShadowFormDockPositon.Top, parentWindowHWnd, this);
			leftShadowForm = new ShadowFormElement(ShadowFormDockPositon.Left, parentWindowHWnd, this);
			bottomShadowForm = new ShadowFormElement(ShadowFormDockPositon.Bottom, parentWindowHWnd, this);
			rightShadowForm = new ShadowFormElement(ShadowFormDockPositon.Right, parentWindowHWnd, this);

			shadows.Add(topShadowForm);
			shadows.Add(leftShadowForm);
			shadows.Add(bottomShadowForm);
			shadows.Add(rightShadowForm);

			User32.ShowWindow(topShadowForm.Handle, ShowWindowStyles.SW_SHOWNOACTIVATE);
			User32.ShowWindow(leftShadowForm.Handle, ShowWindowStyles.SW_SHOWNOACTIVATE);
			User32.ShowWindow(bottomShadowForm.Handle, ShowWindowStyles.SW_SHOWNOACTIVATE);
			User32.ShowWindow(rightShadowForm.Handle, ShowWindowStyles.SW_SHOWNOACTIVATE);


			isInitialized = true;

			AssignHandle(parentWindowHWnd);

			AlignSideShadowToTopMost();

			ActiveColor = activeColor;
			InactiveColor = inactiveColor;
		}
		public void Enable(bool enable)
		{
			if (isEnabled && !enable)
			{
				ShowBorder(false);
				UnregisterEvents();
			}
			else if (!isEnabled && enable)
			{
				RegisterEvents();
				if (parentWindow != null)
				{


					UpdateSizes(parentWindow.Width, parentWindow.Height);


					UpdateLocations(new WINDOWPOS
					{
						x = parentWindow.Left,
						y = parentWindow.Top,
						cx = parentWindow.Width,
						cy = parentWindow.Height,
						flags = (int)SetWindowPosFlags.SWP_SHOWWINDOW
					});

				}
			}

			isEnabled = enable;
		}
		public void SetOwner(IntPtr owner)
		{
			foreach (ShadowFormElement sideShadow in shadows)
			{
				sideShadow.SetOwner(owner);
			}
		}
		public void SetFocus()
		{
			if (!isEnabled) return;
			UpdateFocus(true);
		}
		public void KillFocus()
		{
			if (!isEnabled) return;

			UpdateFocus(false);
		}

		public ShadowFormDecorator(Form window, bool enable = true)
		{
			parentWindow = window;
			isEnabled = enable;

			cachedImages = new Bitmap[3];
#if XP
			cachedImages[0] = ChromFXUI.XP.Properties.Resources.ShadowSharp;
#else
            cachedImages[0] = ChromFXUI.Properties.Resources.ShadowSharp;
#endif
			InitializeBitmapCache();
		}

		private void InitializeBitmapCache()
		{
			var rawImage = cachedImages[0];
			var activeImageCore = cachedImages[1] = (Bitmap)rawImage.Clone();
			var inactiveImageCore = cachedImages[2] = (Bitmap)rawImage.Clone();
			BlendBitmapWithColor(activeImageCore, ActiveColor);
			BlendBitmapWithColor(inactiveImageCore, InactiveColor, 0.6f);
		}

		private void BlendBitmapWithColor(Bitmap source, Color color, float alphaDepth = 1f)
		{
			var rect = new Rectangle(0, 0, source.Width, source.Height);
			if (alphaDepth > 1) alphaDepth = 1;

			var bmp = new LockBitmap(source);
			bmp.LockBits();

			for (var y = rect.Top; y < rect.Bottom; y++)
			{
				for (var x = rect.Left; x < rect.Right; x++)
				{
					var targetColor = bmp.GetPixel(x, y);

					var alpha = Convert.ToByte(targetColor.A * alphaDepth);

					var r = color.R;
					var g = color.G;
					var b = color.B;

					bmp.SetPixel(x, y, Color.FromArgb(alpha, r, g, b));
				}
			}

			bmp.UnlockBits();
		}




		///// <summary>
		///// 启用或禁用窗口大小调整。
		///// </summary>
		///// <param name="enable">True/False</param>
		//public void EnableResize(bool enable)
		//{
		//	foreach (ShadowFormElement sideShadow in shadows)
		//	{
		//		sideShadow.ExternalResizeEnable = enable;
		//	}
		//}

		protected override void WndProc(ref Message m)
		{

			if (!isEnabled || IsDisposed)
			{
				base.WndProc(ref m);
				return;
			}
			var msg = (WindowsMessages)m.Msg;


			switch (msg)
			{

				case WindowsMessages.WM_WINDOWPOSCHANGED:
					lastLocation = (WINDOWPOS)Marshal.PtrToStructure(m.LParam, typeof(WINDOWPOS));
					WindowPosChanged(lastLocation);
					base.WndProc(ref m);
					break;
				case WindowsMessages.WM_ACTIVATEAPP:
					{
						var className = new StringBuilder(256);

						if (m.LParam != IntPtr.Zero && User32.GetClassName(m.LParam, className, className.Capacity) != 0)
						{
							var hWndShadow = m.LParam;
							var name = className.ToString();
							if (name.StartsWith(CONSTS.CLASS_NAME) && isFocused && shadows.Exists(p => p.Handle == hWndShadow))
							{
								return;
							}
						}


						if (m.WParam == Win32.FALSE)
						{
							isFocused = false;
							KillFocus();
						}
						else
						{
							isFocused = true;
							//SetFocus();
						}

					}
					break;

				case WindowsMessages.WM_SIZE:
				
					base.WndProc(ref m);

					Size(m.WParam, m.LParam);

					break;
				default:
					base.WndProc(ref m);
					break;
			}

		}


		private void DestroyShadows()
		{

			CloseShadows();

			parentWindow = null;
		}

		private void RegisterEvents()
		{
			//foreach (ShadowFormElement sideShadow in shadows)
			//{
			//	sideShadow.MouseDown += HandleSideMouseDown;
			//}

			if (parentWindow != null)
			{
				parentWindow.VisibleChanged += HandleWindowVisibleChanged;
			}
		}

		private void HandleWindowVisibleChanged(object sender, EventArgs e)
		{
			ShowBorder(parentWindow.Visible);
		}

		private void UnregisterEvents()
		{
			//foreach (ShadowFormElement sideShadow in shadows)
			//{
			//	sideShadow.MouseDown -= HandleSideMouseDown;
			//}

			if (parentWindow != null)
			{
				parentWindow.VisibleChanged -= HandleWindowVisibleChanged;
			}
		}

		//private void HandleSideMouseDown(object sender, ShadowFormResizeArgs e)
		//{
		//	if (e.Mode == HitTest.HTNOWHERE || e.Mode == HitTest.HTCAPTION)
		//	{
		//		return;
		//	}
		//	User32.PostMessage(parentWindowHWnd, (uint)WindowsMessages.WM_SETFOCUS, IntPtr.Zero, IntPtr.Zero);
		//	User32.SendMessage(parentWindowHWnd, (uint)WindowsMessages.WM_SYSCOMMAND, (IntPtr)e.Mode.ToInt(), IntPtr.Zero);
		//}

		private void CloseShadows()
		{
			foreach (var sideShadow in shadows)
			{
				sideShadow.Close();
			}

			shadows.Clear();

			topShadowForm = null;
			bottomShadowForm = null;
			leftShadowForm = null;
			rightShadowForm = null;
		}

		private void ShowBorder(bool show)
		{
			var action = new Action(() =>
			{

				foreach (ShadowFormElement sideShadow in shadows)
				{
					sideShadow.Show(show);
				}

				if (show)
				{
					isWindowMinimized = false;
				}
			});

			if (show == true && isWindowMinimized)
			{
				if (isAnimationDelayed)
				{
					return;
				}

				isAnimationDelayed = true;
				Task.Factory.StartNew(() =>
				{
					System.Threading.Thread.Sleep(CONSTS.SHOW_BORDER_DELAY);
					if(isAnimationDelayed)
						parentWindow.Invoke(new MethodInvoker(action));

					isAnimationDelayed = false;

				});

			}
			else
			{
				action();

				isAnimationDelayed = false;
			}

		}




		private void UpdateFocus(bool isFocused)
		{
			foreach (ShadowFormElement sideShadow in shadows)
			{
				sideShadow.ParentWindowIsFocused = isFocused;
			}


		}

		private void UpdateSizes(int width, int height)
		{
			foreach (ShadowFormElement sideShadow in shadows)
			{
				sideShadow.SetSize(width, height);
			}
		}

		private void UpdateLocations(WINDOWPOS location)
		{
			foreach (ShadowFormElement sideShadow in shadows)
			{
				sideShadow.SetLocation(location);
			}

			if ((location.flags & (int)SetWindowPosFlags.SWP_HIDEWINDOW) != 0)
			{
				ShowBorder(false);
			}
			else if ((location.flags & (int)SetWindowPosFlags.SWP_SHOWWINDOW) != 0)
			{
				ShowBorder(true);
			}
		}

		private void AlignSideShadowToTopMost()
		{
			if (shadows == null)
			{
				return;
			}

			foreach (ShadowFormElement sideShadow in shadows)
			{
				sideShadow.UpdateZOrder();
			}
		}




		private void WindowPosChanged(WINDOWPOS location)
		{
			if (!isEnabled) return;
			UpdateLocations(location);
		}


		private void Size(IntPtr wParam, IntPtr lParam)
		{
			int width = (int)User32.LoWord(lParam);
			int height = (int)User32.HiWord(lParam);

			if (!isEnabled) return;
			if ((int)wParam == 2 || (int)wParam == 1) // maximized/minimized
			{

				if ((int)wParam == 1)
				{
					isWindowMinimized = true;
				}

				ShowBorder(false);

			}
			else
			{
				var rect = new RECT();

				User32.GetWindowRect(parentWindow.TopLevelControl != null ? parentWindow.TopLevelControl.Handle : parentWindow.Handle, ref rect);

				UpdateSizes(rect.Width, rect.Height);
				ShowBorder(true);

			}
		}



#region Dispose

		private bool _isDisposed;

		/// <summary>
		/// IsDisposed status
		/// </summary>
		public bool IsDisposed
		{
			get { return _isDisposed; }
		}

		/// <summary>
		/// Standard Dispose
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Dispose
		/// </summary>
		/// <param name="disposing">True if disposing, false otherwise</param>
		protected virtual void Dispose(bool disposing)
		{
			if (!_isDisposed)
			{
				if (disposing)
				{
					// release unmanaged resources
				}

				_isDisposed = true;

				DestroyShadows();

				UnregisterEvents();

				this.ReleaseHandle();

				parentWindow = null;
			}
		}

#endregion

	}
}
