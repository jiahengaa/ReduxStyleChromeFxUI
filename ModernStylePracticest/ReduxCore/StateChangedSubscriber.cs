using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReduxCore
{
    /// <summary>
    /// 状态变更订阅器
    /// </summary>
    /// <typeparam name="State"></typeparam>
    /// <param name="state"></param>
    public delegate void StateChangedSubscriber<State>(State state,object action);
}
