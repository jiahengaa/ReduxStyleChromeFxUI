using Actions;
using Packages;
using ReduxCore;

namespace Reducers
{
    public class LoadingReducer: ElementReducer<bool>
    {
        public LoadingReducer()
        {
            base.Process<TodosLoadedAction>((state, action) =>
            {
                return false;
            }).Process<TodosNotLoadedAction>((state, action) => {
                return false;
            });
        }
    }
}
