using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ReduxCore.Broadcaster;

namespace ReduxCore
{
    public class Package<State>
    {
        /// <summary>
        /// 获取状态委托
        /// </summary>
        /// <returns></returns>
        public delegate State GetStateDelegate();
        /// <summary>
        /// 异步动作
        /// </summary>
        /// <param name="dispatcher">遣派器</param>
        /// <param name="getState">获取状态委托</param>
        /// <returns>Task</returns>
        public delegate Task AsyncAction(BroadcasterDelegate dispatcher, GetStateDelegate getState);
        public delegate Task<Result> AsyncAction<Result>(BroadcasterDelegate dispatcher, GetStateDelegate getState);
        /// <summary>
        /// 带入参出参异步动作委托
        /// </summary>
        /// <typeparam name="T">入参类型</typeparam>
        /// <typeparam name="Result">出参</typeparam>
        /// <param name="param">入参</param>
        /// <returns></returns>
        public delegate AsyncAction<Result> AsyncActionNeedsParam<T, Result>(T param);
        /// <summary>
        /// 带入参异步动作委托 
        /// </summary>
        /// <typeparam name="T">入参类型</typeparam>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public delegate AsyncAction AsyncActionNeedsParam<T>(T param);

        /// <summary>
        /// 邮包
        /// </summary>
        private readonly BasicPackage package;
        /// <summary>
        /// 拦截器
        /// </summary>
        private MiddlewarePerformer middlewares;

        public Package(ElementReducer<State> rootReducer) : this(rootReducer.Get())
        {
        }

        public Package(ComposableReducer<State> rootReducer) : this(rootReducer.Get())
        {
        }

        public Package(Reducer<State> rootReducer)
        {
            package = new BasicPackage(rootReducer);
            Middleware();
        }
        /// <summary>
        /// 订阅主题
        /// </summary>
        /// <param name="subscription">订阅的主题</param>
        /// <returns>返回可取消订阅对象</returns>
        public Unsubscribe Subscribe(StateChangedSubscriber<State> subscription)
        {
            return package.Subscribe(subscription);
        }
        /// <summary>
        /// 派遣分发器
        /// </summary>
        /// <param name="action"></param>
        public void Dispatch(Object action)
        {
            middlewares(action);
        }
        /// <summary>
        /// 异步动作派遣分发器
        /// </summary>
        /// <typeparam name="Result">异步动作结果类型</typeparam>
        /// <param name="action">异步动作名称</param>
        /// <returns>返回异步动作结果</returns>
        public Task<Result> Dispatch<Result>(AsyncAction<Result> action)
        {
            return action(Dispatch, GetState);
        }
        /// <summary>
        /// 异步动作派遣分发器
        /// </summary>
        /// <param name="action">异步动作名称</param>
        /// <returns>Task任务</returns>
        public Task Dispatch(AsyncAction action)
        {
            return action(Dispatch, GetState);
        }
        /// <summary>
        /// 异步动作派遣分发器，有入参出参
        /// </summary>
        /// <typeparam name="T">传入类型</typeparam>
        /// <typeparam name="Result">传出结果类型</typeparam>
        /// <param name="action">异步动作名称</param>
        /// <returns>异步</returns>
        public AsyncActionNeedsParam<T, Result> asyncAction<T, Result>(
            Func<BroadcasterDelegate, GetStateDelegate, T, Task<Result>> action)
        {
            return invokeParam => (dispatch, getState) => action(dispatch, getState, invokeParam);
        }
        /// <summary>
        /// 异步动作派遣器，有入参无出参
        /// </summary>
        /// <typeparam name="T">入参类型</typeparam>
        /// <param name="action">异步动作</param>
        /// <returns>返回带入参异步动作</returns>
        public AsyncActionNeedsParam<T> asyncActionVoid<T>(
            Func<BroadcasterDelegate, GetStateDelegate, T, Task> action)
        {
            return invokeParam => (dispatch, getState) => action(dispatch, getState, invokeParam);
        }
        /// <summary>
        /// 异步动作派遣器，仅有出参
        /// </summary>
        /// <typeparam name="Result">出参类型</typeparam>
        /// <param name="action">异步动作</param>
        /// <returns></returns>
        public AsyncAction<Result> asyncAction<Result>(
            AsyncAction<Result> action)
        {
            return (dispatch, getState) => action(dispatch, getState);
        }

        /// <summary>
        /// 获取当前状态
        /// </summary>
        /// <returns></returns>
        public State GetState()
        {
            return package.GetState();
        }
        /// <summary>
        /// 拦截
        /// </summary>
        /// <param name="middlewares"></param>
        public void Middleware(params Middleware<State>[] middlewares)
        {
            this.middlewares =
                middlewares.Select(m => m(package))
                    .Reverse()
                    .Aggregate<MiddlewareStream, MiddlewarePerformer>(package.Dispatch, (action, middle) => middle(action));
        }
        /// <summary>
        /// 基础包
        /// </summary>
        private class BasicPackage : IBasePackage<State>
        {
            /// <summary>
            /// 根分流器
            /// </summary>
            private readonly Reducer<State> rootReducer;
            /// <summary>
            /// 订阅选项
            /// </summary>
            private readonly List<StateChangedSubscriber<State>> subscriptions =
                new List<StateChangedSubscriber<State>>();
            /// <summary>
            /// 状态
            /// </summary>
            private State state;
            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="rootReducer"></param>
            public BasicPackage(Reducer<State> rootReducer)
            {
                this.rootReducer = rootReducer;
                state = rootReducer(state, new InitPackageAction());
            }
            /// <summary>
            /// 订阅器
            /// </summary>
            /// <param name="subscription"></param>
            /// <returns></returns>
            public Unsubscribe Subscribe(StateChangedSubscriber<State> subscription)
            {
                subscriptions.Add(subscription);
                return () => { subscriptions.Remove(subscription); };
            }
            /// <summary>
            /// 派遣分发器
            /// </summary>
            /// <param name="action"></param>
            public void Dispatch(Object action)
            {
                state = rootReducer(state, action);
                //此部分改为异步消息分发。

                Parallel.ForEach(subscriptions, new Action<StateChangedSubscriber<State>>((subscribtion) =>
                {
                    subscribtion(state);
                }));

                //foreach (var subscribtion in subscriptions)
                //{
                //    subscribtion(state);
                //}
            }
            /// <summary>
            /// 获取当前包状态
            /// </summary>
            /// <returns></returns>
            public State GetState()
            {
                return state;
            }
        }
    }
}
