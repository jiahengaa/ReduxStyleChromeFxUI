using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReduxCore
{
    public class Broadcaster
    {
        /// <summary>
        /// 广播分发器
        /// </summary>
        /// <param name="a"></param>
        public delegate void BroadcasterDelegate(Object a);
    }
}
