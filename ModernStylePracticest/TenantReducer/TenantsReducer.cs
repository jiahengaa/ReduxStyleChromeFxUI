using Packages;
using ReduxCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TenantActions;

namespace CTReducer
{
    public class TenantsReducer: ElementReducer<TenantState>
    {
        public TenantsReducer()
        {
            Process<getTenantList>((state, action) =>
            {
                state.pagination.pageSize = action.pageSize;
                state.pagination.current = action.pageNum;
                state.filter.tenantName = action.tenantName;
               
                //请求后台数据，返回请求到的数据
                return QueryTenantList(state);
            }).Process<removeTenantById>((state,action)=> {
                //删除数据库数据,成功后返回state
                state.tenantData = state.tenantData.Where(p => p.id != action.id.ToString()).ToList();
                return state;
            }).Process<handleCurrentChange>((state, action) => {
                //删除数据库数据,成功后返回state
                state.pagination.current = action.current;

                var list = state.tenantData.Where(t =>
                {
                    return !(!string.IsNullOrEmpty(state.filter.tenantName) && t.communityName.IndexOf(state.filter.tenantName) == -1);
                }).Where((p, index) => index < state.pagination.pageSize * state.pagination.current && index >= state.pagination.pageSize * (state.pagination.current - 1)).ToList();
                state.records = list;
                state.total = list.Count;

                return state;
            }).Process<handlePageSizeChange>((state,action)=> {

                //删除数据库数据,成功后返回state
                state.pagination.pageSize = action.pagination.pageSize;
                return QueryTenantList(state);
            }).Process< resetForm>((state,action)=> {
                state.filter.tenantName = "";

                return QueryTenantList(state);
            }).Process< handleSearch>((state, action) => {
                state.pagination.current = 1;
                return QueryTenantList(state);
            });
        }

        private TenantState QueryTenantList(TenantState state)
        {
            var list = state.tenantData.Where(t =>
            {
                return !(!string.IsNullOrEmpty(state.filter.tenantName) && t.communityName.IndexOf(state.filter.tenantName) == -1);
            }).Where((p, index) => index < state.pagination.pageSize * state.pagination.current && index >= state.pagination.pageSize * (state.pagination.current - 1)).ToList();
            state.records = list;
            state.total = list.Count;
            return state;
        }
    }
}
