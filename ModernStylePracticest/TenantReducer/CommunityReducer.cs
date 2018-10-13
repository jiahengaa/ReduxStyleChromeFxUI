using CommunityActions;
using Packages;
using ReduxCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CTReducer
{
    public class CommunityReducer:ElementReducer<CommunityState>
    {
        public CommunityReducer()
        {
            Process<loadCommunityList>((state, action) =>
            {
                #region 伪造后台数据请求

                state.communityData = new List<Community>();
                state.communityData.Add(new Community() { id = 0, communityName = "王院", communityNo = 21, keeperName = "老王", message = "ddsdfs", unit = "王府里" });
                state.communityData.Add(new Community() { id = 1, communityName = "丽芙园", communityNo = 23, keeperName = "小李", message = "ddsdfs", unit = "古丽" });
                state.communityData.Add(new Community() { id = 2, communityName = "嘉园年华", communityNo = 3, keeperName = "老照", message = "ddsdfs", unit = "王府里" });
                state.communityData.Add(new Community() { id = 3, communityName = "丹府", communityNo = 4, keeperName = "亲亲", message = "ddsdfs", unit = "王府里" });
                state.communityData.Add(new Community() { id = 4, communityName = "福院", communityNo = 21, keeperName = "踢踢", message = "ddsdfs", unit = "王府里" });
                state.communityData.Add(new Community() { id = 5, communityName = "王院", communityNo = 23, keeperName = "咩咩", message = "ddsdfs", unit = "阳新行" });
                state.communityData.Add(new Community() { id = 6, communityName = "滕王里", communityNo = 21, keeperName = "萌萌", message = "ddsdfs", unit = "王府里" });
                state.communityData.Add(new Community() { id = 7, communityName = "青年城", communityNo = 34, keeperName = "吉吉", message = "ddsdfs", unit = "王府里" });
                state.communityData.Add(new Community() { id = 8, communityName = "王院", communityNo = 34, keeperName = "肥肥", message = "ddsdfs", unit = "天宝" });
                state.communityData.Add(new Community() { id = 9, communityName = "古稀乡", communityNo = 23, keeperName = "苦菊", message = "ddsdfs", unit = "王府里" });
                state.communityData.Add(new Community() { id = 10, communityName = "王院", communityNo = 21, keeperName = "红红", message = "ddsdfs", unit = "王府里" });
                state.communityData.Add(new Community() { id = 11, communityName = "舰厅", communityNo = 2, keeperName = "AK", message = "ddsdfs", unit = "扶摇" });
                state.communityData.Add(new Community() { id = 12, communityName = "科技苑", communityNo = 21, keeperName = "老A", message = "ddsdfs", unit = "王府里" });
                state.communityData.Add(new Community() { id = 13, communityName = "丹府", communityNo = 234, keeperName = "鸡脚", message = "ddsdfs", unit = "王府里" });
                state.communityData.Add(new Community() { id = 14, communityName = "王院", communityNo = 2, keeperName = "鸭蛋", message = "ddsdfs", unit = "王府里" });
                state.communityData.Add(new Community() { id = 15, communityName = "福院", communityNo = 54, keeperName = "老王", message = "ddsdfs", unit = "王府里" });

                #endregion
                return state;
            }).Process<getCommunityList>((state, action) =>
            {
                state.pagination = action.pagination;
                
                return QueryTenantList(state);
            }).Process<resetForm>((state, action) =>
            {
                state.filter.communityName = "";
                return QueryTenantList(state);
            }).Process<deleteCommunity>((state, action) =>
            {
                state.communityData = state.communityData.Where(p => p.id != action.id).ToList();
                return QueryTenantList(state);
            }).Process<handleCurrentChange>((state, action) =>
            {
                state.pagination.current = action.current;

                return QueryTenantList(state);
            }).Process<handlePageSizeChange>((state, action) =>
            {
               
                state.pagination.pageSize = action.pageSize;
                return QueryTenantList(state);
            }).Process<handleSearch>((state, action) =>
            {
                state.pagination.current = 1;
                return QueryTenantList(state);
            }).Process<addCommunity>((state, action) =>
            {
                state.communityData.Add(new Community()
                {
                    communityName = action.communityFrom.communityName,
                    communityNo = new Random().Next(10000),
                    id = new Random().Next(10000),
                    keeperName = action.communityFrom.keeperName,
                    message = action.communityFrom.message,
                    unit = action.communityFrom.unit
                });
                return QueryTenantList(state);
            }).Process<updateCommunity>((state, action) =>
            {
                var editform = state.communityData.FirstOrDefault(p => p.id == Convert.ToInt32(action.editFrom.id));

                editform.communityName = action.editFrom.communityName;
                editform.keeperName = action.editFrom.keeperName;
                editform.message = action.editFrom.message;
                editform.unit = action.editFrom.unit;

                return QueryTenantList(state);
            }); 
        }

        private CommunityState QueryTenantList(CommunityState state)
        {
            var alllist = state.communityData.Where(t =>
            {
                return !(!string.IsNullOrEmpty(state.filter.communityName) && t.communityName.IndexOf(state.filter.communityName) == -1);
            }).ToList();
            var list = alllist.Where((p, index) => index < state.pagination.pageSize * state.pagination.current && index >= state.pagination.pageSize * (state.pagination.current - 1)).ToList();
            state.records = list;
            state.pagination.total = alllist.Count;
            return state;
        }
    }
}
