using Actions;
using Packages;
using ReduxCore;
using System.Collections.Generic;
using System.Linq;

namespace Reducers
{
    public class TodosReducer : ElementReducer<List<Todo>>
    {
        public TodosReducer()
        {
            Process<AddTodoAction>((state, action) =>
            {
                state.Add(action.Todo);
                return state;
            }).Process<DeleteTodoAction>((state, action) =>
            {
                return state.Where((element) => element.Id != action.Id).ToList();
            }).Process<UpdateTodoAction>((state, action) =>
            {
                for (int i = 0; i < state.Count; i++)
                {
                    if (state[i].Id == action.Id)
                    {
                        state[i] = action.UpdatedTodo;
                    }
                }
                return state;
            }).Process<ClearCompletedAction>((state, action) =>
            {
                return state.Where(element => !element.Complete).ToList();
            }).Process<ToggleAllAction>((state, action) =>
            {
                var allComplete = state.Exists(element => !element.Complete);
                List<Todo> todos = state.ToList();
                for (int i = 0; i < todos.Count; i++)
                {
                    todos[i] = new Todo(todos[i].Task, !allComplete, todos[i].Note, todos[i].Id);
                }
                return todos;
            }).Process<TodosLoadedAction>((state, action) =>
            {
                return action.Todos;
            }).Process<TodosNotLoadedAction>((state, action) =>
            {
                return new List<Todo>();
            });
        }
    }
}
