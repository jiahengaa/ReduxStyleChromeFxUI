using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReduxCore
{
    /// <summary>
    /// 拦截器
    /// </summary>
    /// <param name="action"></param>
    public delegate void MiddlewarePerformer(object action);
    /// <summary>
    /// 拦截流水线
    /// </summary>
    /// <param name="nextMiddleware"></param>
    /// <returns></returns>
    public delegate MiddlewarePerformer MiddlewareStream(MiddlewarePerformer nextMiddleware);
    /// <summary>
    /// 状态拦截器
    /// </summary>
    /// <typeparam name="State"></typeparam>
    /// <param name="store"></param>
    /// <returns></returns>

    public delegate MiddlewareStream Middleware<State>(IBasePackage<State> store);
}
