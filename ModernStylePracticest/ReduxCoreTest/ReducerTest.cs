using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using ReduxCore;

namespace ReduxCoreTest
{
    public class LoadTodos
    {
        public List<Todo> Todos;
    }
    public class AddTodoItem
    {
        public Todo Item;
    }

    public class DeleteTodoItem
    {
        public int Id;
    }

    public class UpdateTodoItem
    {
        public Todo Item;
    }

    public class TabSelect
    {
        public Tab Selected;
    }

    public class Todo
    {
        public int Id;
        public string Content;
        public string Bak;
    }

    public enum Tab
    {
        All, Completed, Todo
    }

    public struct AppPackage
    {
        public List<Todo> Todos;
        public Tab Tab;
    }

    [TestClass]
    public class ReducerTest
    {
        [TestMethod]
        public void combine_reducers()
        {
            List<int> a = new List<int>();
            List<int> b = new List<int>();

            for (int i = 1; i <= 10; i++)
            {
                a.Add(i);
                b.Add(i);
            }

            CollectionAssert.AreEqual(a, b);


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
