using Actions;
using Packages;
using ReduxCore;

namespace Reducers
{
    public class VisibilityReducer: ElementReducer<VisibilityFilter>
    {
        public VisibilityReducer()
        {
            base.Process<UpdateFilterAction>((state, action) =>
            {
                return action.NewFileter;
            });
        }
    }
}
