using Actions;
using Packages;
using ReduxCore;
using System.Collections.Generic;

namespace Reducers
{
    public class AppStateReducer : ComposableReducer<AppState>
    {
        public AppStateReducer()
        {
            //base.Diverter<AppTab>(state => state.ActiveTab, new TabReducer()).
            //    Diverter<VisibilityFilter>(state => state.ActiveFilter, new VisibilityReducer()).
            //    Diverter<bool>(state => state.IsLoading, new LoadingReducer()).
            //    Diverter<List<Todo>>(state => state.Todos, new TodosReducer());
        }
    }
}
