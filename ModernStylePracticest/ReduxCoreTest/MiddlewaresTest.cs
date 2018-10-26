using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using ReduxCore;

namespace ReduxCoreTest
{
    [TestClass]
    public class MiddlewaresTest
    {
        public class AddActin
        {

        }
        [TestMethod]
        public void middleware_should_rob_the_store_dispatcher()
        {
            var packageReducerCount = 0;
            var reducer = new ElementReducer<int>().Process<AddActin>((s, e) =>
            {
                packageReducerCount += 1;
                return s;
            });
            var packageStore = new Package<int>(reducer);
            var middlewareCounter = 0;
            packageStore.Middleware(
                store => next => action =>
                {
                    if(action is AddActin)
                    {

                    }
                    middlewareCounter += 1;
                    Assert.AreEqual(1, middlewareCounter);
                    Logger.LogMessage("第一个拦截器开始执行，将执行第二个拦截器");
                    next(action);//执行顺序下个拦截器或最后的执行器
                    middlewareCounter += 1000;
                    Assert.AreEqual(1111, middlewareCounter);
                    Logger.LogMessage("第一个拦截器执行完毕");
                },
                store => next => action =>
                {
                    Logger.LogMessage("第二个拦截器开始执行");
                    middlewareCounter += 10;

                    Assert.AreEqual(11, middlewareCounter);
                    middlewareCounter += 100;
                    Assert.AreEqual(111, middlewareCounter);

                    Logger.LogMessage("第二个拦截器执行完毕");
                }
            );

            packageStore.Dispatch(new AddActin());

            Assert.AreEqual(1111, middlewareCounter);
            Assert.AreEqual(packageReducerCount, 0);
            Logger.LogMessage("reducer未运行，执行器被劫持成功！");
        }
        [TestMethod]
        public void middleware_should_hook_into_dispathching()
        {
            var packageReducerCount = 0;
            var reducer = new ElementReducer<int>().Process<AddActin>((s, e) =>
            {
                packageReducerCount += 1;
                return packageReducerCount;
            });
            var packageStore = new Package<int>(reducer);
            var middlewareCount = 0;
            packageStore.Middleware(
                package => next => action =>
                {
                    Logger.LogMessage("第一个拦截器开始执行，将执行第二个拦截器");
                    middlewareCount += 1;
                    next(action);
                    middlewareCount += 1000;
                    Logger.LogMessage("第一个拦截器执行完毕");
                },
                package => next => action =>
                {
                    Logger.LogMessage("第二个拦截器开始执行");
                    middlewareCount += 10;
                    Logger.LogMessage("执行器开始执行");
                    next(action);
                    Logger.LogMessage("执行器开始完毕");
                    middlewareCount += 100;
                    Logger.LogMessage("第二个拦截器执行完毕");
                }
                );
            packageStore.Dispatch(new AddActin());
            Assert.AreEqual(1111, middlewareCount);
            Assert.AreEqual(1, packageReducerCount);
        }
        [TestMethod]
        public void middleware_should_close_dispatching()
        {
            var packageReducerCount = 0;
            var reducer = new ElementReducer<int>().Process<AddActin>((s, e) =>
            {
                packageReducerCount += 1;
                return packageReducerCount;
            });
            var packageStore = new Package<int>(reducer);
            var middlewareCount = 0;
            packageStore.Middleware(
                package => next => action =>
                {
                    middlewareCount += 1;
                    Logger.LogMessage("拦截器关闭广播");
                },
                package => next => action =>
                {
                    Logger.LogMessage("第二个拦截器开始执行");
                    middlewareCount += 10;
                    Logger.LogMessage("执行器开始执行");
                    next(action);
                    Logger.LogMessage("执行器开始完毕");
                    middlewareCount += 100;
                    Logger.LogMessage("第二个拦截器执行完毕");
                }
                );
            packageStore.Subscribe((i,action) =>
            {
                Logger.LogMessage("获得广播信息");
            });
            packageStore.Dispatch(new AddActin());
            Assert.AreEqual(1, middlewareCount);
            Assert.AreEqual(0, packageReducerCount);
        }
    }
}
