using System;
using System.Collections.Generic;

namespace ReduxCore
{
    /// <summary>
    /// 元子分流器
    /// </summary>
    /// <typeparam name="State"></typeparam>
    public class ElementReducer<State>
    {
        /// <summary>
        /// 操作者
        /// </summary>
        private readonly Dictionary<Type, Delegate> handlers = new Dictionary<Type, Delegate>();
        /// <summary>
        /// 邮包状态初始化事件
        /// </summary>
        private readonly Func<State> stateInitializer;

        public ElementReducer()
        {
            stateInitializer = () => default(State);
        }

        public ElementReducer(Func<State> initializer)
        {
            this.stateInitializer = initializer;
        }
        /// <summary>
        /// 处理分离器上的事件 
        /// </summary>
        /// <typeparam name="Event">事件类型</typeparam>
        /// <param name="handler">处理者</param>
        /// <returns>返回元子分流器</returns>
        public ElementReducer<State> Process<Event>(Func<State, Event, State> handler)
        {
            handlers.Add(typeof(Event), handler);
            return this;
        }
        /// <summary>
        /// 获取当前分流器
        /// </summary>
        /// <returns></returns>
        public Reducer<State> Get()
        {
            return delegate (State state, Object action)
            {
                var prevState = action.GetType() == typeof(InitPackageAction) ? stateInitializer() : state;
                if (handlers.ContainsKey(action.GetType()))
                {
                    var handler = handlers[action.GetType()];
                    return (State)handler.DynamicInvoke(prevState, action);
                }
                return prevState;
            };
        }
    }
}
