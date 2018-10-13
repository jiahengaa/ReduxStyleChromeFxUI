using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ReduxCore
{
    /// <summary>
    /// 复合分流器
    /// </summary>
    /// <typeparam name="State"></typeparam>
    public class ComposableReducer<State>
    {
        /// <summary>
        /// 属性分流器集
        /// </summary>
        private readonly List<Tuple<FieldInfo, Delegate>> fieldReducers = new List<Tuple<FieldInfo, Delegate>>();
        /// <summary>
        /// 状态初始化
        /// </summary>
        private readonly Func<State> stateInitializer;

        public ComposableReducer()
        {
            stateInitializer = () => default(State);
        }

        public ComposableReducer(Func<State> initializer)
        {
            this.stateInitializer = initializer;
        }
        /// <summary>
        /// 分流器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="composer"></param>
        /// <param name="reducer"></param>
        /// <returns></returns>
        public ComposableReducer<State> Diverter<T>(Expression<Func<State, T>> composer, ElementReducer<T> reducer)
        {
            return Diverter(composer, reducer.Get());
        }
        /// <summary>
        /// 分流器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="composer"></param>
        /// <param name="reducer"></param>
        /// <returns></returns>
        public ComposableReducer<State> Diverter<T>(Expression<Func<State, T>> composer, ComposableReducer<T> reducer)
        {
            return Diverter(composer, reducer.Get());
        }
        /// <summary>
        /// 分流器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="composer"></param>
        /// <param name="reducer"></param>
        /// <returns></returns>
        public ComposableReducer<State> Diverter<T>(Expression<Func<State, T>> composer, Reducer<T> reducer)
        {
            var memberExpr = composer.Body as MemberExpression;
            if (memberExpr == null)
                throw new ArgumentException(string.Format(
                    "Expression '{0}' should be a field.",
                    composer.ToString()));

            var member = (FieldInfo)memberExpr.Member;
            if (member == null)
                throw new ArgumentException(string.Format(
                    "Expression '{0}' should be a constant expression",
                    composer.ToString()));

            fieldReducers.Add(new Tuple<FieldInfo, Delegate>(member, reducer));
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
                var result = action.GetType() == typeof(InitPackageAction) ? stateInitializer() : state;
                foreach (var fieldReducer in fieldReducers)
                {
                    var prevState = action.GetType() == typeof(InitPackageAction)
                        ? null
                        : fieldReducer.Item1.GetValue(state);
                    var newState = fieldReducer.Item2.DynamicInvoke(prevState, action);
                    object boxer = result; //boxing to allow the next line work for both reference and value objects
                    fieldReducer.Item1.SetValue(boxer, newState);
                    result = (State)boxer; // unbox, hopefully not too much performance penalty
                }
                return result;
            };
        }
    }
}
