// Copyright (c) 2014-2017 Wolfgang Borgsmüller
// All rights reserved.
// 
// This software may be modified and distributed under the terms
// of the BSD license. See the License.txt file for details.

using System;
using System.Runtime.InteropServices;

namespace Chromium {
    public partial class CfxTime {

        public DateTime ToUniversalTime() {
            return ToUniversalTime(this);
        }

        public static DateTime ToUniversalTime(CfxTime time) {
            return new DateTime(time.Year, time.Month, time.DayOfMonth, time.Hour, time.Minute, time.Second, time.Millisecond, DateTimeKind.Utc);
        }

        public static CfxTime FromUniversalTime(DateTime time) {

            if (time.Kind != DateTimeKind.Utc)
                throw new ArgumentException("time must be of kind DateTimeKind.Utc", "time");

            var r = new CfxTime();
            r.Year = time.Year;
            r.Month = time.Month;
            r.DayOfMonth = time.Day;
            r.DayOfWeek = (int)time.DayOfWeek;
            r.Hour = time.Hour;
            r.Minute = time.Minute;
            r.Second = time.Second;
            r.Millisecond = time.Millisecond;
            return r;
        }
    }
}
