using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReduxCore
{
    /// <summary>
    /// 邮包状态管理接口
    /// </summary>
    public interface IBasePackage<State>
    {
        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="subscription"></param>
        /// <returns></returns>
        Unsubscribe Subscribe(StateChangedSubscriber<State> subscription);
        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="action"></param>
        void Dispatch(Object action);
        /// <summary>
        /// 获取当前State
        /// </summary>
        /// <returns></returns>
        State GetState();
    }
}
