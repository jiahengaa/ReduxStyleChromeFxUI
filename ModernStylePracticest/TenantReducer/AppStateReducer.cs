using Packages;
using ReduxCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CTReducer
{
    public class AppStateReducer : ComposableReducer<AppState>
    {
        public AppStateReducer()
        {
            base.Diverter<TenantState>(state => state.tenant, new TenantsReducer()).
                Diverter<CommunityState>(state=>state.community,new CommunityReducer()).
                Diverter< ChartState>(state=>state.chart,new ChartReducer());
        }
    }
}
