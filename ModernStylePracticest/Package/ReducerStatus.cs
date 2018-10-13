using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Packages
{
    public struct ReducerStatus
    {
        public string type;//warning,info,error
        public string message;
        public string module;//哪一个模块
        public string eventName;//哪个事件名称
    }
}
