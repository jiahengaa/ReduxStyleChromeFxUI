using Packages;
using System;
using System.Collections.Generic;

namespace Actions
{
    public class ClearCompletedAction { }
    public class ToggleAllAction { }
    public class LoadTodosAction { }
    public class TodosNotLoadedAction { }
    public class TodosLoadedAction
    {
        public List<Todo> Todos { private set; get; }
        public TodosLoadedAction(List<Todo> todos)
        {
            Todos = todos;
        }
    }

    public class UpdateTodoAction
    {
        public String Id { private set; get; }
        public Todo UpdatedTodo { private set; get; }
        public UpdateTodoAction(string id, Todo todo)
        {
            Id = id;
            UpdatedTodo = todo;
        }
    }

    public class DeleteTodoAction
    {
        public string Id { private set; get; }
        public DeleteTodoAction(string id)
        {
            Id = id;
        }
    }

    public class AddTodoAction
    {
        public Todo Todo
        {
            private set;
            get;
        }

        public AddTodoAction(Todo todo)
        {
            Todo = todo;
        }
    }

    public class UpdateFilterAction
    {
        public VisibilityFilter NewFileter { private set; get; }
        public UpdateFilterAction(VisibilityFilter visibilityFilter)
        {
            NewFileter = visibilityFilter;
        }
    }

    public class UpdateTabAction
    {
        public AppTab NewTab { private set; get; }
        public UpdateTabAction(AppTab appTab)
        {
            NewTab = appTab;
        }
    }

}
