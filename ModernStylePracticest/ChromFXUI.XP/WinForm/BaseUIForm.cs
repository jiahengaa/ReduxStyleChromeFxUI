﻿using ChromeFX.Windows.Imports;
using ChromeFX.WinForm.ShadowForm;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChromeFX.WinForm
{
	public enum ShadowFormType
	{
		GlowShadow,
		DropShadow
	}
	public partial class BaseUIForm : Form
	{
        private static readonly IntPtr WVR_VALIDRECTS = new IntPtr(0x400);
        private static readonly Point minimizedFormLocation = new Point(-32000, -32000);
        private static readonly Point InvalidPoint = new Point(-10000, -10000);
        private static bool suppressDeactivation;
		private static Padding? SavedBorders = null;
		private static readonly object formLayoutChangedEvent = new object();
		private static bool? isDesingerProcess = null;
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		protected static bool IsDesingerProcess
		{
			get
			{
				if (isDesingerProcess == null)
				{
					isDesingerProcess = Process.GetCurrentProcess().ProcessName == "devenv";
				}

				return isDesingerProcess.Value;
			}
		}


		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public static bool SuppressDeactivation
		{
			get { return suppressDeactivation; }
			set
			{
				if (value)
				{
					foreach (Form form in Application.OpenForms)
					{
						var BaseForm = form as BaseUIForm;
						if (BaseForm != null)
							BaseForm.IsActiveSaved = BaseForm.IsActive;
					}
				}
				else
				{
				}
				suppressDeactivation = value;
			}
		}

		private ShadowFormType shadowType = ShadowFormType.GlowShadow;

		private bool isShadowEnabled = true;
		private Rectangle regionRect = Rectangle.Empty;
		private int isInitializing = 0;
		private bool forceInitialized = false;
		private Size minimumClientSize;
		private Size maximumClientSize;
		private Size? minimumSize = null;
		private Size? maximumSize = null;
		private bool isCustomFrameEnabled = false;
		private bool? isEnterSizeMoveMode = null;
		internal bool creatingHandle = false;
		protected IShadowForm shadowDecorator;
		protected bool IsDesignMode => DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime || IsDesingerProcess;
		protected bool CanResize => FormBorderStyle == FormBorderStyle.SizableToolWindow || FormBorderStyle == FormBorderStyle.Sizable;

        #region 属性
        /// <summary>
        /// 设置或获取ChromeFXUI窗体投影的样式
        /// </summary>
        [Category("ChromeFXUI")]
		public ShadowFormType ShadowEffect
		{
			get
			{
				return shadowType;
			}
			set
			{
				if (value == ShadowEffect) return;

				shadowType = value;

				OnShadowEffectChanged();
			}
		}

		private void OnShadowEffectChanged()
		{
			var isInitd = true;
			var isEnabled = true;

			if (shadowDecorator != null)
			{
				isInitd = shadowDecorator.IsInitialized;
				isEnabled = shadowDecorator.IsEnabled;

				shadowDecorator.Dispose();
			}


			if (ShadowEffect == ShadowFormType.DropShadow)
			{
				shadowDecorator = new ShadowFormDecorator(this, isEnabled);
			}
			else
			{
				shadowDecorator = new FormGlowBorderDecorator(this, isEnabled);
			}

			shadowDecorator.InactiveColor = InactiveShadowColor;
			shadowDecorator.ActiveColor = ActiveShadowColor;

			if (isInitd)
			{
				
				shadowDecorator.InitializeShadows();
			}
		}

        /// <summary>
        /// 设置或获取ChromeFXUI在Nonclient模式下是否显示投影
        /// </summary>
        [Category("ChromeFXUI")]
		public bool EnableShadowForm
		{
			get
			{
				if (IsDesignMode)
				{
					return isShadowEnabled;
				}
				else
				{
					return isShadowEnabled && IsBaseUIEnabled;
				}
			}
			set
			{
				isShadowEnabled = value;

				shadowDecorator.Enable(EnableShadowForm);
			}
		}
        /// <summary>
        /// 设置或获取ChromeFXUI窗口边框线条粗细
        /// </summary>
        [Category("ChromeFXUI")]
		public int BorderWidth
		{
			get; set;
		} = 1;

		/// <summary>
		/// 设置或获取非活动状态窗口边框颜色
		/// </summary>
		[Category("ChromeFXUI")]
		public Color InactiveBorderColor
		{
			get;
			set;
		} = ColorTranslator.FromHtml("#AAAAAA");

		/// <summary>
		/// 设置或获取活动状态窗口边框颜色
		/// </summary>
		[Category("ChromeFXUI")]
		public Color ActiveBorderColor
		{
			get;
			set;
		} = ColorTranslator.FromHtml("#1883D7");


		Color activeShadowColor = Color.Black, inactiveShadowColor = Color.Black;

		/// <summary>
		/// 设置或获取活动状态窗口投影颜色
		/// </summary>
		[Category("ChromeFXUI")]
		public Color ActiveShadowColor
		{
			get => activeShadowColor;
			set => activeShadowColor = shadowDecorator.ActiveColor = value;
		}

		/// <summary>
		/// 设置或获取非活动状态窗口投影颜色
		/// </summary>
		[Category("ChromeFXUI")]
		public Color InactiveShadowColor
		{
			get => inactiveShadowColor;
			set => inactiveShadowColor = shadowDecorator.InactiveColor = value;
		}

		#endregion

		public BaseUIForm() :
			this(true)
		{

		}
		public BaseUIForm(bool enableBaseUI)
		{
			isCustomFrameEnabled = enableBaseUI;

			shadowDecorator = (shadowType == ShadowFormType.DropShadow? (IShadowForm)new ShadowFormDecorator(this, false) : (IShadowForm)new FormGlowBorderDecorator(this,false));

			this.BackColor = Color.White;
			if (!IsDesignMode)
			{
				this.minimumClientSize = Size.Empty;
				this.maximumClientSize = Size.Empty;

			}
		}

		#region Window Renderer

		protected bool IsBaseUIEnabled
		{
			get => isCustomFrameEnabled && FormBorderStyle != FormBorderStyle.None;
			set
			{
				if (isCustomFrameEnabled != IsBaseUIEnabled)
				{
					RecreateHandle();
				}

				isCustomFrameEnabled = value;
				if (isCustomFrameEnabled)
				{
					UxTheme.SetWindowTheme(Handle, string.Empty, string.Empty);
				}
				else
				{
					UxTheme.SetWindowTheme(Handle, null, null);

				}

			}
		}


		protected override CreateParams CreateParams => GetCreateParams(base.CreateParams);
		protected virtual CreateParams GetCreateParams(CreateParams par)
		{
			if (IsDisposed || Disposing) return par;

			if (this.creatingHandle && IsFormStateClientSizeSet()/* && isCustomFrameEnabled*/)
			{
				par.Width = Width;
				par.Height = Height;
			}
			return par;
		}
		protected override void CreateHandle()
		{

			//if (!isCustomFrameEnabled)
			//{
			//	base.CreateHandle();
			//	return;
			//}

			var shouldPatchSize = !creatingHandle;
			creatingHandle = true;
			if (shouldPatchSize)
				if (WindowState != FormWindowState.Minimized)
					Size = SizeFromClientSize(ClientSize);
			if (!IsHandleCreated)
				base.CreateHandle();
			this.creatingHandle = false;
		}
		protected override void OnHandleCreated(EventArgs e)
		{

			base.OnHandleCreated(e);


			if (isCustomFrameEnabled)
			{
				User32.DisableProcessWindowsGhosting();
				BeginInvoke(new MethodInvoker(UpdateTheme));
				GetSystemMenu(Handle, true);
				UpdateWindowThemeCore();

			}
			else
			{
				EnableTheme();
			}


		}
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if (!IsDesignMode)
			{
				if (StartPosition == FormStartPosition.CenterParent && Owner != null)
				{
					Location = new Point(Owner.Location.X + Owner.Width / 2 - Width / 2,
					Owner.Location.Y + Owner.Height / 2 - Height / 2);


				}
				else if (StartPosition == FormStartPosition.CenterScreen || (StartPosition == FormStartPosition.CenterParent && Owner == null))
				{
					var currentScreen = Screen.FromPoint(MousePosition);
					Location = new Point(currentScreen.WorkingArea.Left + (currentScreen.WorkingArea.Width / 2 - this.Width / 2), currentScreen.WorkingArea.Top + (currentScreen.WorkingArea.Height / 2 - this.Height / 2));

				}
			}
			OnMinimumClientSizeChanged();
			OnMaximumClientSizeChanged();
			UpdateShadowForm();
			CalcFormBounds();
		}
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			if (IsDesignMode) return;

			var action = new Action(() =>
			{
         
				shadowDecorator.Enable(EnableShadowForm);

				if (IsActive)
				{
                    shadowDecorator.SetFocus();
                }
			});

			Task.Factory.StartNew(() =>
			{
				//System.Threading.Thread.Sleep(180);

				if (InvokeRequired)
				{
					Invoke(new MethodInvoker(action));
				}
				else
				{
					action.Invoke();
				}
			});



		}

		private void UpdateShadowForm()
		{
			if (IsDesignMode) return;

			if (!IsMdiChild && Parent == null)
			{
				shadowDecorator.InitializeShadows();

				if (Owner != null)
				{
					shadowDecorator.SetOwner(Owner.Handle);
				}

			}
		}
		protected Rectangle FormBounds
		{
			get
			{
				if (Handle == IntPtr.Zero) return new Rectangle(0, 0, Bounds.Width, Bounds.Height);
				var r = new RECT();
				User32.GetWindowRect(Handle, ref r);
				return r.ToRectangle();
			}
		}
		protected Padding RealWindowBorders
		{
			get
			{
				var rect = new RECT();

				rect.left = 0;
				rect.right = 0;
				rect.top = 0;
				rect.bottom = 0;

				User32.AdjustWindowRectEx(ref rect, CreateParams.Style, false, CreateParams.ExStyle);

				return new Padding(-rect.left, -rect.top, rect.right, rect.bottom);
			}
		}
		public new Point Location
		{
			get
			{
				if (IsDesignMode)
				{
					return new Point(0, 0);
				}
				else
				{
					return base.Location;
				}
			}
			set
			{
				base.Location = value;
			}
		}
		public new bool TopMost
		{
			get
			{
				return base.TopMost;
			}
			set
			{
				base.TopMost = value;
				//shadowDecorator.TopMost = value;
			}
		}
		internal bool IsActiveSaved { get; set; }
		protected bool IsActive
		{
			get
			{
				BitVector32 bv = (BitVector32)FormStateCoreField.GetValue(this);
				BitVector32.Section s = (BitVector32.Section)FormStateWindowActivatedField.GetValue(this);
				return bv[s] == 1;
			}
		}
		private FieldInfo formStateWindowActivated;
		private FieldInfo FormStateWindowActivatedField
		{
			get
			{
				if (formStateWindowActivated == null)
					formStateWindowActivated = typeof(Form).GetField("FormStateIsWindowActivated", BindingFlags.NonPublic | BindingFlags.Static);
				return formStateWindowActivated;
			}
		}
		protected bool ShouldPatchClientSize { get; set; }
		protected Size SavedClientSize { get; private set; }
		private void PatchClientSize()
		{
			SavedClientSize = ClientSizeFromSize(Size);
			FieldInfo fiWidth = typeof(Control).GetField("clientWidth", BindingFlags.Instance | BindingFlags.NonPublic);
			FieldInfo fiHeight = typeof(Control).GetField("clientHeight", BindingFlags.Instance | BindingFlags.NonPublic);
			fiWidth.SetValue(this, SavedClientSize.Width);
			fiHeight.SetValue(this, SavedClientSize.Height);
		}
		protected override void OnClientSizeChanged(EventArgs e)
		{
			if (ShouldPatchClientSize)
				PatchClientSize();
			base.OnClientSizeChanged(e);
		}
		protected override void OnSizeChanged(EventArgs e)
		{
			if (FormBorderStyle != FormBorderStyle.None && (ControlBox || !string.IsNullOrEmpty(Text)) && WindowState == FormWindowState.Normal)
			{
				PatchClientSize();
			}
			else if (ShouldPatchClientSize && WindowState != FormWindowState.Minimized)
				PatchClientSize();

			CalcFormBounds();

			base.OnSizeChanged(e);
		}
		protected Rectangle formBounds = Rectangle.Empty;
		protected Rectangle prevFormBounds = Rectangle.Empty;
		protected bool isFormPainted = false;
		protected bool boundsUpdated = false;
		protected bool IsRegionPainted { get; private set; }
		protected internal bool IsMinimizedState(Rectangle bounds)
		{
			return WindowState == FormWindowState.Minimized && bounds.Location == minimizedFormLocation;
		}
		protected internal void CalcFormBounds()
		{
			if (IsMdiChild || !IsHandleCreated) return;
			var correctFormBounds = new RECT();
			User32.GetWindowRect(this.Handle, ref correctFormBounds);
			Rectangle currentBounds = correctFormBounds.ToRectangle();
			if (IsMinimizedState(currentBounds)) currentBounds = Rectangle.Empty;
			if (formBounds == currentBounds && (boundsUpdated || !IsRegionPainted))
				return;
			this.isFormPainted = false;
			prevFormBounds = formBounds;
			formBounds = currentBounds;
			if (prevFormBounds != formBounds) boundsUpdated = true;
		}
		private PropertyInfo piLayout = null;
		protected internal bool IsLayoutSuspendedCore
		{
			get
			{
				if (piLayout == null) piLayout = typeof(Control).GetProperty("IsLayoutSuspended", BindingFlags.Instance | BindingFlags.NonPublic);
				if (piLayout != null) return (bool)piLayout.GetValue(this, null);
				return false;
			}
		}
		private FieldInfo fiLayoutSuspendCount = null;
		[Browsable(false)]
		protected internal byte LayoutSuspendCountCore
		{
			get
			{
				if (fiLayoutSuspendCount == null) fiLayoutSuspendCount = typeof(Control).GetField("layoutSuspendCount", BindingFlags.Instance | BindingFlags.NonPublic);
				if (fiLayoutSuspendCount != null) return (byte)fiLayoutSuspendCount.GetValue(this);
				return 1;
			}
		}
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			Size size = new Size(width, height);

			if (isCustomFrameEnabled)
			{
				size = PatchFormSizeInRestoreWindowBoundsIfNecessary(width, height);
				size = CalcPreferredSizeCore(size);
			}

			base.SetBoundsCore(x, y, size.Width, size.Height, specified);
		}
		protected virtual Size CalcPreferredSizeCore(Size size)
		{
			return size;
		}
		protected virtual Size PatchFormSizeInRestoreWindowBoundsIfNecessary(int width, int height)
		{
			if (WindowState == FormWindowState.Normal)
			{
				try
				{
					FieldInfo fiRestoredBoundsSpecified = typeof(Form).GetField("restoredWindowBoundsSpecified", BindingFlags.NonPublic | BindingFlags.Instance);
					BoundsSpecified restoredSpecified = (BoundsSpecified)fiRestoredBoundsSpecified.GetValue(this);
					if ((restoredSpecified & BoundsSpecified.Size) != BoundsSpecified.None)
					{
						FieldInfo fi1 = typeof(Form).GetField("FormStateExWindowBoundsWidthIsClientSize", BindingFlags.NonPublic | BindingFlags.Static),
						fiFormState = typeof(Form).GetField("formStateEx", BindingFlags.NonPublic | BindingFlags.Instance),
						fiBounds = typeof(Form).GetField("restoredWindowBounds", BindingFlags.NonPublic | BindingFlags.Instance);
						if (fi1 != null && fiFormState != null && fiBounds != null)
						{
							Rectangle restoredWindowBounds = (Rectangle)fiBounds.GetValue(this);
							BitVector32.Section bi1 = (BitVector32.Section)fi1.GetValue(this);
							BitVector32 state = (BitVector32)fiFormState.GetValue(this);
							if (state[bi1] == 1)
							{
								width = restoredWindowBounds.Width + BorderWidth * 2;
								height = restoredWindowBounds.Height + BorderWidth * 2;
							}
						}
					}
				}
				catch
				{
				}
			}
			return new Size(width, height);
		}
		protected bool IsInitializing => !forceInitialized && (this.isInitializing != 0 || IsLayoutSuspendedCore);
		public new void SuspendLayout()
		{

			base.SuspendLayout();

			isInitializing++;
		}
		private bool shouldUpdateBaseUIOnResumeLayout = false;
		private void CheckForceBaseUIChangedCore()
		{
			if (this.isInitializing != 0) return;
			if (LayoutSuspendCountCore == 1 && this.shouldUpdateBaseUIOnResumeLayout)
			{
				this.forceInitialized = true;
				try
				{
					OnBaseUIChangedCore();
				}
				finally
				{
					this.forceInitialized = false;
				}
			}
		}
		protected internal void SetLayoutDeferred()
		{
			const int STATE_LAYOUTDEFERRED = 512;
			MethodInfo mi = typeof(Control).GetMethod("SetState", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(int), typeof(bool) }, null);
			mi.Invoke(this, new object[] { STATE_LAYOUTDEFERRED, true });
		}
		protected bool CheckUpdateSkinPainter()
		{
			Padding? savedMargins = null;

			Size savedClientSize = ClientSize;

			if (DesignMode && IsInitializing)
			{
				savedMargins = SavedBorders;
			}

			var needReset = false;

			if (savedMargins != null && !object.Equals(savedMargins.Value, RealWindowBorders))
			{
				ClientSize = savedClientSize;
			}

			if (IsHandleCreated)
			{
				if (!isCustomFrameEnabled)
				{
					EnableTheme();
					needReset = true;
					RecreateHandle();

				}
				else
				{
					DisableTheme();
					ForceRefresh();

				}



			}
			return needReset;
		}
		private bool themeEnabled = true;
		private void EnableTheme()
		{
			if (themeEnabled) return;

			if (IsHandleCreated)
			{
				UxTheme.SetWindowTheme(this.Handle, null, null);
			}

			Region = null;
			this.themeEnabled = true;
		}
		private void DisableTheme()
		{
			if (!ControlBox && !themeEnabled) return;
			if (IsHandleCreated)
			{
				UxTheme.SetWindowTheme(this.Handle, "", "");
			}
			this.themeEnabled = false;
			UpdateWindowThemeCore();
		}
		protected virtual void ForceRefresh()
		{
			Refresh();
		}
		private void UpdateTheme()
		{
			if (isCustomFrameEnabled)
			{
				DisableTheme();
			}
			else
			{
				EnableTheme();
			}
		}
		protected virtual void UpdateWindowThemeCore()
		{
			if (!DesignMode && isCustomFrameEnabled)
			{
				UxTheme.SetWindowTheme(Handle, "", "");
				themeEnabled = false;
			}
		}
		private void OnBaseUIChangedCore()
		{
			if (IsInitializing)
			{
				if (Visible && IsLayoutSuspendedCore)
				{
					SetLayoutDeferred();
				}
				shouldUpdateBaseUIOnResumeLayout = true;
				return;
			}
			this.shouldUpdateBaseUIOnResumeLayout = false;
			bool shouldUpdateSize = CheckUpdateSkinPainter();
			Size clientSize = ClientSize;
			OnMinimumClientSizeChanged();
			OnMaximumClientSizeChanged();
			FieldInfo fiBounds = typeof(Form).GetField("restoredWindowBounds", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo fiBoundsSpec = typeof(Form).GetField("restoredWindowBoundsSpecified", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo fiBounds2 = typeof(Form).GetField("restoreBounds", BindingFlags.NonPublic | BindingFlags.Instance);
			Rectangle restoredWinBounds = (Rectangle)fiBounds.GetValue(this);
			Rectangle restoreBounds = (Rectangle)fiBounds2.GetValue(this);
			BoundsSpecified restoredWinBoundsSpec = (BoundsSpecified)fiBoundsSpec.GetValue(this);
			int frmStateExWindowBoundsWidthIsClientSize, frmStateExWindowBoundsHeightIsClientSize;
			GetFormStateExWindowBoundsIsClientSize(out frmStateExWindowBoundsWidthIsClientSize, out frmStateExWindowBoundsHeightIsClientSize);
			int windowState = SaveFormStateWindowState();
			bool normalState = SaveControlStateNormalState();
			if (shouldUpdateSize)
				Size = SizeFromClientSize(clientSize);
			if ((restoredWinBoundsSpec & BoundsSpecified.Width) != 0 && (restoredWinBoundsSpec & BoundsSpecified.Height) != 0) restoreBounds.Size = SizeFromClientSize(restoredWinBounds.Size);
			if (WindowState != FormWindowState.Normal && IsHandleCreated)
			{
				fiBounds.SetValue(this, restoredWinBounds);
				fiBounds2.SetValue(this, restoreBounds);
				SetFormStateExWindowBoundsIsClientSize(frmStateExWindowBoundsWidthIsClientSize, frmStateExWindowBoundsHeightIsClientSize);
			}
			if (IsMdiChild)
			{
				RestoreFormStateWindowState(windowState);
				RestoreControlStateNormalState(normalState);
			}


		}
        private void SetFormStateExWindowBoundsIsClientSize(int width, int height)
        {
            FieldInfo formStateExInfo = typeof(Form).GetField("formStateEx", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo formStateExWindowBoundsWidthIsClientSizeInfo = typeof(Form).GetField("FormStateExWindowBoundsWidthIsClientSize", BindingFlags.NonPublic | BindingFlags.Static);
            FieldInfo formStateExWindowBoundsHeightIsClientSizeInfo = typeof(Form).GetField("FormStateExWindowBoundsHeightIsClientSize", BindingFlags.NonPublic | BindingFlags.Static);
            BitVector32.Section widthSection = (BitVector32.Section)formStateExWindowBoundsWidthIsClientSizeInfo.GetValue(this);
            BitVector32.Section heightSection = (BitVector32.Section)formStateExWindowBoundsHeightIsClientSizeInfo.GetValue(this);
            BitVector32 formState = (BitVector32)formStateExInfo.GetValue(this);
            formState[widthSection] = width;
            formState[heightSection] = height;
            formStateExInfo.SetValue(this, formState);
        }

        private void RestoreControlStateNormalState(bool isNormal)
		{
			FieldInfo state = typeof(Control).GetField("state", BindingFlags.NonPublic | BindingFlags.Instance);
			int value = (int)state.GetValue(this);
			state.SetValue(this, isNormal ? (value | 0x10000) : (value & (~0x10000)));
		}
        private void GetFormStateExWindowBoundsIsClientSize(out int width, out int height)
        {
            FieldInfo formStateExInfo = typeof(Form).GetField("formStateEx", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo formStateExWindowBoundsWidthIsClientSizeInfo = typeof(Form).GetField("FormStateExWindowBoundsWidthIsClientSize", BindingFlags.NonPublic | BindingFlags.Static);
            FieldInfo formStateExWindowBoundsHeightIsClientSizeInfo = typeof(Form).GetField("FormStateExWindowBoundsHeightIsClientSize", BindingFlags.NonPublic | BindingFlags.Static);
            BitVector32.Section widthSection = (BitVector32.Section)formStateExWindowBoundsWidthIsClientSizeInfo.GetValue(this);
            BitVector32.Section heightSection = (BitVector32.Section)formStateExWindowBoundsHeightIsClientSizeInfo.GetValue(this);
            BitVector32 formState = (BitVector32)formStateExInfo.GetValue(this);
            width = formState[widthSection];
            height = formState[heightSection];
        }
        private int SaveFormStateWindowState()
		{
			FieldInfo formStateWindowState = typeof(Form).GetField("FormStateWindowState", BindingFlags.NonPublic | BindingFlags.Static);
			BitVector32.Section formStateWindowStateSection = ((BitVector32.Section)formStateWindowState.GetValue(this));
			BitVector32 formStateData = (BitVector32)FormStateCoreField.GetValue(this);
			return formStateData[formStateWindowStateSection];
		}
        private bool SaveControlStateNormalState()
        {
            FieldInfo state = typeof(Control).GetField("state", BindingFlags.NonPublic | BindingFlags.Instance);
            return ((int)state.GetValue(this) & 0x10000) != 0;
        }
        private void RestoreFormStateWindowState(int state)
		{
			FieldInfo formStateWindowState = typeof(Form).GetField("FormStateWindowState", BindingFlags.NonPublic | BindingFlags.Static);
			BitVector32.Section formStateWindowStateSection = ((BitVector32.Section)formStateWindowState.GetValue(this));
			BitVector32 formStateData = (BitVector32)FormStateCoreField.GetValue(this);
			formStateData[formStateWindowStateSection] = state;
			FormStateCoreField.SetValue(this, formStateData);
		}
		

		public new void ResumeLayout() { ResumeLayout(true); }
		public new void ResumeLayout(bool performLayout)
		{

			if (this.isInitializing > 0)
				this.isInitializing--;
			if (this.isInitializing == 0)
			{
				CheckForceBaseUIChangedCore();
			}

			base.ResumeLayout(performLayout);

			if (!IsInitializing)
			{
				CheckMinimumSize();
				CheckMaximumSize();
			}
		}
		private void SetRegion(Region region, Rectangle rect)
		{
			if (this.regionRect == rect)
			{
				if (region != null)
					region.Dispose();
				return;
			}
			if (Region != null)
			{
				Region.Dispose();
			}
			Region = region;
			if (object.Equals(region, Region))
				this.regionRect = rect;
		}
		private void CheckMaximumSize()
		{
			if (this.maximumSize == null) return;
			Size msize = (Size)maximumSize;
			if (!msize.IsEmpty)
			{
				if (msize.Width > 0) msize.Width = Math.Max(msize.Width, Size.Width);
				if (msize.Height > 0) msize.Height = Math.Max(msize.Height, Size.Height);
				if (this.minimumSize != null && !this.minimumSize.Value.IsEmpty)
				{
					if (this.maximumSize.Value.Width == this.minimumSize.Value.Width)
						msize.Width = Size.Width;
					if (this.maximumSize.Value.Height == this.minimumSize.Value.Height)
						msize.Height = Size.Height;
				}
			}
			this.maximumSize = null;
			base.MaximumSize = msize;
		}
		private void CheckMinimumSize()
		{
			if (this.minimumSize == null) return;
			Size msize = (Size)minimumSize;
			if (!msize.IsEmpty)
			{
				if (msize.Width > 0) msize.Width = Math.Min(msize.Width, Size.Width);
				if (msize.Height > 0) msize.Height = Math.Min(msize.Height, Size.Height);
				if (this.maximumSize != null && !this.maximumSize.Value.IsEmpty)
				{
					if (this.maximumSize.Value.Width == this.minimumSize.Value.Width)
						msize.Width = Size.Width;
					if (this.maximumSize.Value.Height == this.minimumSize.Value.Height)
						msize.Height = Size.Height;
				}
			}
			this.minimumSize = null;
			base.MinimumSize = msize;
		}
		protected Size ConstrainMinimumClientSize(Size value)
		{
			value.Width = Math.Max(0, value.Width);
			value.Height = Math.Max(0, value.Height);
			return value;
		}
		protected virtual Size ClientSizeFromSize(Size formSize)
		{
			if (formSize == Size.Empty)
				return Size.Empty;
			Size sz = SizeFromClientSize(Size.Empty);
			Size res = new Size(formSize.Width - sz.Width, formSize.Height - sz.Height);
			if (WindowState != FormWindowState.Maximized)
				return res;
			var rect = new RECT(0, 0, res.Width, res.Height);
			
			return new Size(rect.Right, rect.Bottom);
		}
		private bool inScaleControl = false;
		public override Size MinimumSize
		{
			get
			{
				if (inScaleControl && minimumSize.HasValue)
					return minimumSize.Value;
				return base.MinimumSize;
			}
			set
			{
				minimumSize = value;
				if (IsInitializing)
				{
					return;
				}
				Size maxSize = MaximumSize;
				base.MinimumSize = value;
				if (maxSize != MaximumSize)
					MaximumClientSize = ClientSizeFromSize(MaximumSize);
			}
		}
		public override Size MaximumSize
		{
			get
			{
				if (inScaleControl && maximumSize.HasValue)
					return maximumSize.Value;
				return base.MaximumSize;
			}
			set
			{
				maximumSize = value;
				if (IsInitializing)
				{
					return;
				}
				Size minSize = MinimumSize;
				base.MaximumSize = value;
				if (MinimumSize != minSize)
					MinimumClientSize = ClientSizeFromSize(MinimumSize);
			}
		}
		protected override void OnMinimumSizeChanged(EventArgs e)
		{
			base.OnMinimumSizeChanged(e);
			MinimumClientSize = ClientSizeFromSize(MinimumSize);
		}
		private bool InMaximumSizeChanged { get; set; }
		protected override void OnMaximumSizeChanged(EventArgs e)
		{
			if (InMaximumSizeChanged)
				return;
			InMaximumSizeChanged = true;
			try
			{
				base.OnMaximumSizeChanged(e);
				MaximumClientSize = ClientSizeFromSize(MaximumSize);
			}
			finally
			{
				InMaximumSizeChanged = false;
			}
		}
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Size MinimumClientSize
		{
			get { return minimumClientSize; }
			set
			{
				value = ConstrainMinimumClientSize(value);
				if (MinimumClientSize == value) return;
				minimumClientSize = value;
				OnMinimumClientSizeChanged();
			}
		}
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Size MaximumClientSize
		{
			get { return maximumClientSize; }
			set
			{
				if (MaximumClientSize == value) return;
				maximumClientSize = value;
				OnMaximumClientSizeChanged();
			}
		}
		protected virtual void OnMinimumClientSizeChanged()
		{
			if (IsInitializing) return;
			MinimumSize = GetConstrainSize(MinimumClientSize);
		}
		protected virtual void OnMaximumClientSizeChanged()
		{
			if (IsInitializing) return;
			MaximumSize = GetConstrainSize(MaximumClientSize);
		}
		protected virtual Size GetConstrainSize(Size clientSize)
		{
			if (clientSize == Size.Empty) return Size.Empty;
			return SizeFromClientSize(clientSize);
		}
		protected override Size SizeFromClientSize(Size clientSize)
		{

			return CalcSizeFromClientSize(clientSize);

		}
		protected virtual Size CalcSizeFromClientSize(Size client)
		{
			if (isCustomFrameEnabled)
			{
				client.Width += (BorderWidth * 2);
				client.Height += (BorderWidth * 2);
			}
			else
			{
				client.Width = this.Width;
				client.Height = this.Height;
			}


			return client;
		}
		internal bool IsMaximizedBoundsSet
		{
			get { return !MaximumSize.IsEmpty || !MaximizedBounds.IsEmpty; }
		}
		private FieldInfo formStateCoreField;
		private FieldInfo FormStateCoreField
		{
			get
			{
				if (formStateCoreField == null)
					formStateCoreField = typeof(Form).GetField("formState", BindingFlags.NonPublic | BindingFlags.Instance);
				return formStateCoreField;
			}
		}
		private bool IsFormStateClientSizeSet()
		{
			FieldInfo fi1 = typeof(Form).GetField("FormStateSetClientSize", BindingFlags.NonPublic | BindingFlags.Static);
			BitVector32.Section bi1 = (BitVector32.Section)fi1.GetValue(this);
			BitVector32 state = (BitVector32)FormStateCoreField.GetValue(this);
			return state[bi1] == 1;
		}
		protected override void ScaleCore(float x, float y)
		{

			MaximumClientSize = new Size((int)Math.Round(MaximumClientSize.Width * x), (int)Math.Round(MaximumClientSize.Height * y));
			base.ScaleCore(x, y);
			MinimumClientSize = new Size((int)Math.Round(MinimumClientSize.Width * x), (int)Math.Round(MinimumClientSize.Height * y));

		}
		protected override void SetClientSizeCore(int x, int y)
		{
			FieldInfo fiWidth = typeof(Control).GetField("clientWidth", BindingFlags.Instance | BindingFlags.NonPublic);
			FieldInfo fiHeight = typeof(Control).GetField("clientHeight", BindingFlags.Instance | BindingFlags.NonPublic);
			FieldInfo fi1 = typeof(Form).GetField("FormStateSetClientSize", BindingFlags.NonPublic | BindingFlags.Static),
				fiFormState = typeof(Form).GetField("formState", BindingFlags.NonPublic | BindingFlags.Instance);

			if (fiWidth != null && fiHeight != null && fiFormState != null && fi1 != null)
			{


				this.Size = SizeFromClientSize(new Size(x, y));

				fiWidth.SetValue(this, x);
				fiHeight.SetValue(this, y);
				BitVector32.Section bi1 = (BitVector32.Section)fi1.GetValue(this);
				BitVector32 state = (BitVector32)fiFormState.GetValue(this);
				state[bi1] = 1;
				fiFormState.SetValue(this, state);
				this.OnClientSizeChanged(EventArgs.Empty);
				state[bi1] = 0;
				fiFormState.SetValue(this, state);
			}
			else
			{
				base.SetClientSizeCore(x, y);
			}
		}
		protected override Rectangle GetScaledBounds(Rectangle bounds, SizeF factor, BoundsSpecified specified)
		{
			Rectangle rect = base.GetScaledBounds(bounds, factor, specified);
			if (!isCustomFrameEnabled)
				return rect;
			Size sz = SizeFromClientSize(Size.Empty);
			if (!GetStyle(ControlStyles.FixedWidth) && ((specified & BoundsSpecified.Width) != BoundsSpecified.None))
			{
				int clientWidth = bounds.Width - sz.Width;
				rect.Width = ((int)Math.Round((double)(clientWidth * factor.Width))) + sz.Width;
			}
			if (!GetStyle(ControlStyles.FixedHeight) && ((specified & BoundsSpecified.Height) != BoundsSpecified.None))
			{
				int clientHeight = bounds.Height - sz.Height;
				rect.Height = ((int)Math.Round((double)(clientHeight * factor.Height))) + sz.Height;
			}
			return rect;
		}
		SizeF scaleFactor = new SizeF(1f, 1f);
		protected internal SizeF GetScaleFactor()
		{
			return this.scaleFactor;
		}
		protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
		{
			this.scaleFactor = factor;
			this.inScaleControl = true;
			if (!isCustomFrameEnabled)
			{
				base.ScaleControl(factor, specified);
				this.inScaleControl = false;
				return;
			}
			Size minSize = MinimumSize;
			Size maxSize = MaximumSize;
			Size sz = Size.Empty;
			try
			{
				ShouldPatchClientSize = true;
				Size = SizeFromClientSize(ClientSize);
				ShouldPatchClientSize = true;
				sz = SizeFromClientSize(Size.Empty);
				base.ScaleControl(factor, specified);
			}
			finally
			{
				ShouldPatchClientSize = false;
			}
			if (minSize != Size.Empty)
			{
				minSize -= sz;
				minSize = new Size((int)Math.Round(minSize.Width * factor.Width), (int)Math.Round(minSize.Height * factor.Height)) + sz;
			}
			if (maxSize != Size.Empty)
			{
				maxSize -= sz;
				maxSize = new Size((int)Math.Round(maxSize.Width * factor.Width), (int)Math.Round(maxSize.Height * factor.Height)) + sz;
			}
			MinimumSize = minSize;
			MaximumSize = maxSize;
			this.inScaleControl = false;
		}
		protected override void OnLayout(LayoutEventArgs levent)
		{
			if (this.shouldUpdateBaseUIOnResumeLayout)
			{
				CheckForceBaseUIChangedCore();
			}
			base.OnLayout(levent);
			RaiseFormLayoutChanged(levent);
		}
		private int lockFormLayoutEvent = 0;
		private WeakReference delayedLayoutEventArgsRef;
		protected void RaiseFormLayoutChanged(LayoutEventArgs e)
		{
			if (lockFormLayoutEvent != 0)
			{
				delayedLayoutEventArgsRef = new WeakReference(e);
				return;
			}

			((LayoutEventHandler)Events[formLayoutChangedEvent])?.Invoke(this, e);
		}
		protected virtual Region GetDefaultFormRegion(ref Rectangle rect)
		{
			rect = Rectangle.Empty;
			return null;
		}
		private bool clientSizeSet = false;
		private bool isWindowActive;

		protected override void OnStyleChanged(EventArgs e)
		{
			if (isCustomFrameEnabled)
				ShouldPatchClientSize = true;
			SavedClientSize = ClientSize;
			try
			{
				if (this.clientSizeSet)
				{
					ClientSize = ClientSize;
					this.clientSizeSet = false;
				}
			}
			finally
			{
				ShouldPatchClientSize = false;
			}
			base.OnStyleChanged(e);
		}
		#endregion

		#region WindowsMessages Handlers
		protected override void WndProc(ref Message m)
		{
			if (!isCustomFrameEnabled)
			{
				base.WndProc(ref m);
				return;
			}

			var msg = (WindowsMessages)m.Msg;

			var processed = false;

			switch (msg)
            {
                case WindowsMessages.WM_NCACTIVATE:
                    if (m.WParam == Win32.FALSE)
                    {
                        isWindowActive = false;
                        shadowDecorator.KillFocus();
                    }
                    else
                    {
                        //shadowDecorator.SetFocus();
                        isWindowActive = true;

                    }

                    m.Result = Win32.MESSAGE_HANDLED;

                    User32.SendFrameChanged(Handle);
                    processed = true;

                    break;
                case WindowsMessages.WM_ENTERSIZEMOVE:
					isEnterSizeMoveMode = true;
					break;
				
                case WindowsMessages.WM_SHOWWINDOW:
                    BringToFront();
                    Focus();
                    break;
                case WindowsMessages.WM_SIZING:
					if (IsHandleCreated && isEnterSizeMoveMode == true && shadowDecorator.IsEnabled)
					{
						shadowDecorator.Enable(false);
					}
					break;
                case WindowsMessages.WM_EXITSIZEMOVE:
                    isEnterSizeMoveMode = false;

                    if (shadowDecorator != null && EnableShadowForm && !shadowDecorator.IsEnabled)
                    {
                        shadowDecorator.Enable(true);
                    }
                    break;
                case WindowsMessages.WM_SIZE:
					if (IsHandleCreated)
					{
						WmSize(ref m);
					}
					break;
				
				case WindowsMessages.WM_MOVE:
					User32.InvalidateWindow(Handle);
					break;
                case WindowsMessages.WM_ACTIVATEAPP:
                    User32.SendFrameChanged(Handle);
                    break;
               
				case WindowsMessages.WM_NCCALCSIZE:
					NCCalcSize(ref m);
					break;
				case WindowsMessages.WM_NCPAINT:
					if (User32.IsWindowVisible(Handle) && IsHandleCreated)
					{
						NCPaint(ref m);
						m.Result = Win32.MESSAGE_PROCESS;
					}
					processed = true;
					break;
				case WindowsMessages.WM_NCHITTEST:
					processed = NCHitTest(ref m);
					break;
				case WindowsMessages.WM_NCUAHDRAWCAPTION:
				case WindowsMessages.WM_NCUAHDRAWFRAME:
					User32.SendFrameChanged(Handle);
					processed = true;
					break;
				case WindowsMessages.WM_NCMOUSEMOVE:
					User32.SendFrameChanged(Handle);
					break;
			}

			if (!processed)
			{
				base.WndProc(ref m);
			}
		}

		private void WmSize(ref Message m)
		{
			if (WindowState == FormWindowState.Maximized)
			{
				Screen screen = Screen.FromHandle(Handle);
				if (screen == null) return;
				Rectangle bounds = FormBorderStyle == FormBorderStyle.None ? screen.Bounds : screen.WorkingArea;
				Rectangle formBounds = FormBounds;
				if (formBounds.X == -10000 || formBounds.Y == -10000)
					return;
				Rectangle r = new Rectangle(bounds.X - formBounds.X, bounds.Y - formBounds.Y, formBounds.Width - (formBounds.Width - bounds.Width), formBounds.Height - (formBounds.Height - bounds.Height));

				SetRegion(new Region(r), r);
			}
			else if (WindowState == FormWindowState.Minimized)
			{
				SetRegion(null, Rectangle.Empty);
				return;
			}
			else
			{
				Rectangle rect = new Rectangle();
				Region region = GetDefaultFormRegion(ref rect);
				SetRegion(region, rect);
			}
		}

		private void NCPaint(ref Message m)
		{
			var hdc = GetDC(m);

			if (hdc == IntPtr.Zero)
			{
				return;
			}

			using (var g = Graphics.FromHdc(hdc))
			{
				RECT windowRect = new RECT();

				User32.GetWindowRect(Handle, ref windowRect);

				User32.OffsetRect(ref windowRect, -windowRect.left, -windowRect.top);

				RECT clientRect = new RECT();
				User32.GetClientRect(Handle, ref clientRect);
				User32.OffsetRect(ref clientRect, -clientRect.left, -clientRect.top);

				User32.OffsetRect(ref clientRect, BorderWidth, BorderWidth);

				var frameRegion = new Region(windowRect.ToRectangle());

				if (WindowState == FormWindowState.Maximized)
				{
					frameRegion.Exclude(windowRect.ToRectangle());
				}
				else
				{

					frameRegion.Exclude(clientRect.ToRectangle());
				}



				g.FillRegion(new SolidBrush(isWindowActive ? ActiveBorderColor : InactiveBorderColor), frameRegion);
			}

			User32.ReleaseDC(Handle, hdc);
		}

		protected IntPtr GetDC(Message msg)
		{
			IntPtr res = IntPtr.Zero;

			if (msg.Msg == (int)WindowsMessages.WM_NCPAINT)
			{
				int flags = (int)DCX.DCX_CACHE | (int)DCX.DCX_CLIPSIBLINGS | (int)DCX.DCX_WINDOW | (int)DCX.DCX_VALIDATE;

				IntPtr hrgnCopy = IntPtr.Zero;

				if (msg.WParam != Win32.TRUE)
				{
					flags |= (int)DCX.DCX_INTERSECTRGN;
					hrgnCopy = Gdi32.CreateRectRgn(0, 0, 1, 1);
					Gdi32.CombineRgn(hrgnCopy, msg.WParam, IntPtr.Zero, Gdi32.RGN_COPY);
				}

				res = User32.GetDCEx(Handle, hrgnCopy, flags);

				return res;
			}

			return User32.GetWindowDC(Handle);
		}
		private bool NCHitTest(ref Message m)
		{
			if (m.Result == Win32.FALSE)
			{
				var pos = new POINT((int)User32.LoWord(m.LParam), (int)User32.HiWord(m.LParam));
				User32.ScreenToClient(Handle, ref pos);

				var mode = GetSizeMode(pos);


				SetCursor(mode);
				m.Result = (IntPtr)mode;


				return false;
			}

			return true;
		}

		protected HitTest GetSizeMode(POINT point)
		{
			HitTest mode = HitTest.HTCLIENT;

			int x = point.x, y = point.y;

			var CornerAreaSize = Win32.CornerAreaSize;

			if (WindowState == FormWindowState.Normal && CanResize)
			{
				if (x < CornerAreaSize & y < CornerAreaSize)
				{
					mode = HitTest.HTTOPLEFT;
				}
				else if (x < CornerAreaSize & y + CornerAreaSize > this.Height - CornerAreaSize)
				{
					mode = HitTest.HTBOTTOMLEFT;

				}
				else if (x + CornerAreaSize > this.Width - CornerAreaSize & y + CornerAreaSize > this.Height - CornerAreaSize)
				{
					mode = HitTest.HTBOTTOMRIGHT;

				}
				else if (x + CornerAreaSize > this.Width - CornerAreaSize & y < CornerAreaSize)
				{
					mode = HitTest.HTTOPRIGHT;

				}
				else if (x < CornerAreaSize)
				{
					mode = HitTest.HTLEFT;

				}
				else if (x + CornerAreaSize > this.Width - CornerAreaSize)
				{
					mode = HitTest.HTRIGHT;

				}
				else if (y < CornerAreaSize)
				{
					mode = HitTest.HTTOP;

				}
				else if (y + CornerAreaSize > this.Height - CornerAreaSize)
				{
					mode = HitTest.HTBOTTOM;
				}

			}

			return mode;
		}

		protected void SetCursor(HitTest mode)
		{


			IntPtr handle = IntPtr.Zero;

			switch (mode)
			{
				case HitTest.HTTOP:
				case HitTest.HTBOTTOM:
					handle = User32.LoadCursor(IntPtr.Zero, (int)IdcStandardCursors.IDC_SIZENS);
					break;
				case HitTest.HTLEFT:
				case HitTest.HTRIGHT:
					handle = User32.LoadCursor(IntPtr.Zero, (int)IdcStandardCursors.IDC_SIZEWE);
					break;
				case HitTest.HTTOPLEFT:
				case HitTest.HTBOTTOMRIGHT:
					handle = User32.LoadCursor(IntPtr.Zero, (int)IdcStandardCursors.IDC_SIZENWSE);
					break;
				case HitTest.HTTOPRIGHT:
				case HitTest.HTBOTTOMLEFT:
					handle = User32.LoadCursor(IntPtr.Zero, (int)IdcStandardCursors.IDC_SIZENESW);
					break;
			}

			if (handle != IntPtr.Zero)
			{
				User32.SetCursor(handle);
			}
		}

		private void NCCalcSize(ref Message m)
		{


			if (m.WParam != Win32.FALSE)
			{
				var ncsize = (NCCALCSIZE_PARAMS)Marshal.PtrToStructure(m.LParam, typeof(NCCALCSIZE_PARAMS));

				Padding borders;

				if (SavedBorders == null)
				{
					SavedBorders = RealWindowBorders;
				}


				if (FormBorderStyle == FormBorderStyle.None)
				{
					borders = Padding.Empty;
				}
				else
				{
					borders = RealWindowBorders;
				}



				ncsize.rectBeforeMove = ncsize.rectProposed;

				ncsize.rectProposed.top -= borders.Top;

				if (WindowState == FormWindowState.Normal)
				{
					ncsize.rectProposed.right += borders.Right;
					ncsize.rectProposed.bottom += borders.Bottom;
					ncsize.rectProposed.left -= borders.Left;

					ncsize.rectProposed.top += BorderWidth;
					ncsize.rectProposed.right -= BorderWidth;
					ncsize.rectProposed.bottom -= BorderWidth;
					ncsize.rectProposed.left += BorderWidth;
				}
				else if (WindowState == FormWindowState.Maximized)
				{
					ncsize.rectProposed.top += borders.Bottom;
				}




				Marshal.StructureToPtr(ncsize, m.LParam, false);
				m.Result = WVR_VALIDRECTS;

				User32.InvalidateWindow(Handle);

			}

		}

		#endregion

		#region Extended Methods

		[DllImport("USER32.dll")]
		private static extern bool DestroyMenu(IntPtr menu);
		[DllImport("user32.dll", EntryPoint = "GetSystemMenu")]
		private static extern IntPtr GetSystemMenuCore(IntPtr hWnd, bool bRevert);
		[System.Security.SecuritySafeCritical]
		internal static IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert)
		{
			return GetSystemMenuCore(hWnd, bRevert);
		}

		protected bool ShowSystemMenu(Form frm, Point pt)
		{
			const int TPM_LEFTALIGN = 0x0000, TPM_TOPALIGN = 0x0000, TPM_RETURNCMD = 0x0100;
			if (frm == null)
				return false;
			IntPtr menuHandle = GetSystemMenu(frm.Handle, false);
			IntPtr command = User32.TrackPopupMenu(menuHandle, TPM_RETURNCMD | TPM_TOPALIGN | TPM_LEFTALIGN, pt.X, pt.Y, 0, frm.Handle, IntPtr.Zero);
			if (frm.IsDisposed)
				return false;
			User32.PostMessage(frm.Handle, (uint)WindowsMessages.WM_SYSCOMMAND, command, IntPtr.Zero);
			return true;
		}
		#endregion

		#region Dialog
		public new DialogResult ShowDialog(IWin32Window owner)
		{
			return base.ShowDialog(CheckOwner(owner));
		}
		static IWin32Window CheckOwner(IWin32Window owner)
		{
			var form = owner as BaseUIForm;
			if (form != null)
			{
				if (form.Location == InvalidPoint)
				{
					return form.OwnedForms.FirstOrDefault(x => IsAppropriateOwner(x));
				}
			}
			return owner;
		}
		static bool IsAppropriateOwner(Form condidateForm)
		{
			return true;
		}
		#endregion

	}
}
