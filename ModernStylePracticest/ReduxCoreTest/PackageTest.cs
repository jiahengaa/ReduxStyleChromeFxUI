using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using ReduxCore;

using System.Linq;

namespace ReduxCoreTest
{
    [TestClass]
    public class PackageTest
    {
        [TestMethod]
        public void emit_subscriber_during_subscribed()
        {
            var reducer = new ElementReducer<List<int>>(() => new List<int> { 1 });
            var package = new Package<List<int>>(reducer);

            var counter = 0;
            var unsuber = package.Subscribe(state =>
            {
                counter++;
            });

            package.Dispatch(new int());
            package.Dispatch(new int());
            package.Dispatch(new int());

            Assert.AreEqual(3, counter);
            unsuber();

            package.Dispatch(new int());

            Assert.AreEqual(3, counter);
        }
        [TestMethod]
        public void emit_subscriber_with_right_event()
        {
            var TodosReducer = new ElementReducer<List<Todo>>().Process<LoadTodos>((curPackage, action) =>
            {
                return action.Todos;
            }).Process<AddTodoItem>((curPackage, action) =>
            {
                var todos = curPackage;
                todos.Add(action.Item);
                return todos;
            }).Process<DeleteTodoItem>((curPackage, action) =>
            {
                var todos = curPackage;
                todos.RemoveAll(p => p.Id == action.Id);
                return todos;
            }).Process<UpdateTodoItem>((pacakge, action) =>
            {
                var todos = pacakge;
                var updateItem = todos.FirstOrDefault(p => p.Id == action.Item.Id);
                updateItem.Bak = action.Item.Bak;
                updateItem.Content = action.Item.Content;
                return todos;
            });

            var TabReducer = new ElementReducer<Tab>().Process<TabSelect>((curPackage, action) =>
            {
                var tab = action.Selected;
                return tab;
            });

            var reducer = new ComposableReducer<AppPackage>()
            .Diverter(curPackage => curPackage.Todos, TodosReducer)
            .Diverter(curPackage => curPackage.Tab, TabReducer);

            var package = new Package<AppPackage>(reducer);

            package.Middleware(
                curPackage => next => action =>
                {
                    Logger.LogMessage(action.GetType().ToString());
                    next(action);
                }
                );

            package.Dispatch(new LoadTodos()
            {
                Todos = new List<Todo>()
            });

            Logger.LogMessage("广播新增一条待办");
            package.Dispatch(new AddTodoItem()
            {
                Item = new Todo()
                {
                    Id = 1,
                    Bak = "我的第一条待办",
                    Content = "明天飞"
                }
            });

            Assert.AreEqual("明天飞", package.GetState().Todos[0].Content);

            Logger.LogMessage("广播更新一条待办");
            package.Dispatch(new UpdateTodoItem()
            {
                Item = new Todo()
                {
                    Id = 1,
                    Bak = "我的第一条待办",
                    Content = "明天不飞了"
                }
            });
            Assert.AreEqual("明天不飞了", package.GetState().Todos[0].Content);


            Logger.LogMessage("广播删除一条待办");
            package.Dispatch(new DeleteTodoItem()
            {
                Id = 1
            });

            Assert.AreEqual(0, package.GetState().Todos.Count);

            Logger.LogMessage("广播TabSelectedChanged");
            package.Dispatch(new TabSelect()
            {
                Selected = Tab.Completed
            });

            Assert.AreEqual(Tab.Completed, package.GetState().Tab);
        }
    }
}
