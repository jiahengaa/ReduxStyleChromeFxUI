// Copyright (c) 2014-2017 Wolfgang Borgsmüller
// All rights reserved.
// 
// This software may be modified and distributed under the terms
// of the BSD license. See the License.txt file for details.

// Generated file. Do not edit.


using System;

namespace Chromium.Remote {
    internal static class CfxStringVisitorRemoteClient {

        static CfxStringVisitorRemoteClient() {
            visit_native = visit;
            visit_native_ptr = System.Runtime.InteropServices.Marshal.GetFunctionPointerForDelegate(visit_native);
        }

        internal static void SetCallback(IntPtr self, int index, bool active) {
            switch(index) {
                case 0:
                    CfxApi.StringVisitor.cfx_string_visitor_set_callback(self, index, active ? visit_native_ptr : IntPtr.Zero);
                    break;
            }
        }

        // visit
        [System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.StdCall, SetLastError = false)]
        private delegate void visit_delegate(IntPtr gcHandlePtr, IntPtr string_str, int string_length);
        private static visit_delegate visit_native;
        private static IntPtr visit_native_ptr;

        internal static void visit(IntPtr gcHandlePtr, IntPtr string_str, int string_length) {
            var call = new CfxStringVisitorVisitRemoteEventCall();
            call.gcHandlePtr = gcHandlePtr;
            call.string_str = string_str;
            call.string_length = string_length;
            call.RequestExecution();
        }

    }
}
