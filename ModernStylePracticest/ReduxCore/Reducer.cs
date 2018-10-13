using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReduxCore
{
    /// <summary>
    /// 分流器
    /// </summary>
    /// <typeparam name="State"></typeparam>
    /// <param name="state"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public delegate State Reducer<State>(State state, Object action);
}
