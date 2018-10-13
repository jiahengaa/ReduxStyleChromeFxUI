using Actions;
using Packages;
using ReduxCore;

namespace Reducers
{
    public class TabReducer:ElementReducer<AppTab>
    {
        public TabReducer()
        {
            base.Process<UpdateTabAction>((state, action) =>
            {
                return action.NewTab;
            });
        }
    }
}
