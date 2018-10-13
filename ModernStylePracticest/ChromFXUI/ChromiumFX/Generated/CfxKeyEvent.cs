// Copyright (c) 2014-2017 Wolfgang Borgsmüller
// All rights reserved.
// 
// This software may be modified and distributed under the terms
// of the BSD license. See the License.txt file for details.

// Generated file. Do not edit.


using System;

namespace Chromium {
    /// <summary>
    /// Structure representing keyboard event information.
    /// </summary>
    /// <remarks>
    /// See also the original CEF documentation in
    /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/internal/cef_types.h">cef/include/internal/cef_types.h</see>.
    /// </remarks>
    public sealed class CfxKeyEvent : CfxStructure {

        internal static CfxKeyEvent Wrap(IntPtr nativePtr) {
            if(nativePtr == IntPtr.Zero) return null;
            return new CfxKeyEvent(nativePtr);
        }

        internal static CfxKeyEvent WrapOwned(IntPtr nativePtr) {
            if(nativePtr == IntPtr.Zero) return null;
            return new CfxKeyEvent(nativePtr, CfxApi.KeyEvent.cfx_key_event_dtor);
        }

        public CfxKeyEvent() : base(CfxApi.KeyEvent.cfx_key_event_ctor, CfxApi.KeyEvent.cfx_key_event_dtor) {}
        internal CfxKeyEvent(IntPtr nativePtr) : base(nativePtr) {}
        internal CfxKeyEvent(IntPtr nativePtr, CfxApi.cfx_dtor_delegate cfx_dtor) : base(nativePtr, cfx_dtor) {}

        /// <summary>
        /// The type of keyboard event.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/internal/cef_types.h">cef/include/internal/cef_types.h</see>.
        /// </remarks>
        public CfxKeyEventType Type {
            get {
                int value;
                CfxApi.KeyEvent.cfx_key_event_get_type(nativePtrUnchecked, out value);
                return (CfxKeyEventType)value;
            }
            set {
                CfxApi.KeyEvent.cfx_key_event_set_type(nativePtrUnchecked, (int)value);
            }
        }

        /// <summary>
        /// Bit flags describing any pressed modifier keys. See
        /// CfxEventFlags for values.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/internal/cef_types.h">cef/include/internal/cef_types.h</see>.
        /// </remarks>
        public uint Modifiers {
            get {
                uint value;
                CfxApi.KeyEvent.cfx_key_event_get_modifiers(nativePtrUnchecked, out value);
                return value;
            }
            set {
                CfxApi.KeyEvent.cfx_key_event_set_modifiers(nativePtrUnchecked, value);
            }
        }

        /// <summary>
        /// The Windows key code for the key event. This value is used by the DOM
        /// specification. Sometimes it comes directly from the event (i.e. on
        /// Windows) and sometimes it's determined using a mapping function. See
        /// WebCore/platform/chromium/KeyboardCodes.h for the list of values.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/internal/cef_types.h">cef/include/internal/cef_types.h</see>.
        /// </remarks>
        public int WindowsKeyCode {
            get {
                int value;
                CfxApi.KeyEvent.cfx_key_event_get_windows_key_code(nativePtrUnchecked, out value);
                return value;
            }
            set {
                CfxApi.KeyEvent.cfx_key_event_set_windows_key_code(nativePtrUnchecked, value);
            }
        }

        /// <summary>
        /// The actual key code genenerated by the platform.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/internal/cef_types.h">cef/include/internal/cef_types.h</see>.
        /// </remarks>
        public int NativeKeyCode {
            get {
                int value;
                CfxApi.KeyEvent.cfx_key_event_get_native_key_code(nativePtrUnchecked, out value);
                return value;
            }
            set {
                CfxApi.KeyEvent.cfx_key_event_set_native_key_code(nativePtrUnchecked, value);
            }
        }

        /// <summary>
        /// Indicates whether the event is considered a "system key" event (see
        /// http://msdn.microsoft.com/en-us/library/ms646286(VS.85).aspx for details).
        /// This value will always be false on non-Windows platforms.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/internal/cef_types.h">cef/include/internal/cef_types.h</see>.
        /// </remarks>
        public bool IsSystemKey {
            get {
                int value;
                CfxApi.KeyEvent.cfx_key_event_get_is_system_key(nativePtrUnchecked, out value);
                return 0 != value;
            }
            set {
                CfxApi.KeyEvent.cfx_key_event_set_is_system_key(nativePtrUnchecked, value ? 1 : 0);
            }
        }

        /// <summary>
        /// The character generated by the keystroke.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/internal/cef_types.h">cef/include/internal/cef_types.h</see>.
        /// </remarks>
        public short Character {
            get {
                short value;
                CfxApi.KeyEvent.cfx_key_event_get_character(nativePtrUnchecked, out value);
                return value;
            }
            set {
                CfxApi.KeyEvent.cfx_key_event_set_character(nativePtrUnchecked, value);
            }
        }

        /// <summary>
        /// Same as |character| but unmodified by any concurrently-held modifiers
        /// (except shift). This is useful for working out shortcut keys.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/internal/cef_types.h">cef/include/internal/cef_types.h</see>.
        /// </remarks>
        public short UnmodifiedCharacter {
            get {
                short value;
                CfxApi.KeyEvent.cfx_key_event_get_unmodified_character(nativePtrUnchecked, out value);
                return value;
            }
            set {
                CfxApi.KeyEvent.cfx_key_event_set_unmodified_character(nativePtrUnchecked, value);
            }
        }

        /// <summary>
        /// True if the focus is currently on an editable field on the page. This is
        /// useful for determining if standard key events should be intercepted.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/internal/cef_types.h">cef/include/internal/cef_types.h</see>.
        /// </remarks>
        public int FocusOnEditableField {
            get {
                int value;
                CfxApi.KeyEvent.cfx_key_event_get_focus_on_editable_field(nativePtrUnchecked, out value);
                return value;
            }
            set {
                CfxApi.KeyEvent.cfx_key_event_set_focus_on_editable_field(nativePtrUnchecked, value);
            }
        }

    }
}
