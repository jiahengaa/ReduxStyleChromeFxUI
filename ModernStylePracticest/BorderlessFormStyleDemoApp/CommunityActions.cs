using CommunityActions;
using Newtonsoft.Json;
using Packages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BorderlessFormStyleDemoApp
{
    public static class CommunityActions
    {
        public static void ConfigCommunityActions(this ReduxModernBaseForm<AppState> chromClient)
        {
            var communityActions = chromClient.GlobalObject.AddObject("communityActions");

            communityActions.AddFunction("handleSearch").Execute += (func, args) =>
            {
                chromClient.Store.Dispatch(new handleSearch());
            };

            communityActions.AddFunction("handlePageSizeChange").Execute += (func, args) =>
            {
                var str = args.Arguments.FirstOrDefault(p => p.IsString);
                var strValue = str.StringValue;
                var pagination = JsonConvert.DeserializeObject<Pagination>(strValue);

                chromClient.Store.Dispatch(new handlePageSizeChange() { pageSize = pagination.pageSize });
            };

            communityActions.AddFunction("handleCurrentChange").Execute += (func, args) =>
            {
                var str = args.Arguments.FirstOrDefault(p => p.IsString);
                var strValue = str.StringValue;
                var index = JsonConvert.DeserializeObject<int>(strValue);

                chromClient.Store.Dispatch(new handleCurrentChange() { current = index });
            };

            communityActions.AddFunction("loadCommunityList").Execute += (func, args) =>
            {
                chromClient.Store.Dispatch(new loadCommunityList());
            };

            communityActions.AddFunction("getCommunityList").Execute += (func, args) =>
            {
                var str = args.Arguments.FirstOrDefault(p => p.IsString);
                var strValue = str.StringValue;
                var pagination = JsonConvert.DeserializeObject<Pagination>(strValue);
                chromClient.Store.Dispatch(new getCommunityList() { pagination = pagination });
            };

            communityActions.AddFunction("deleteCommunity").Execute += (func, args) =>
            {
                var str = args.Arguments.FirstOrDefault(p => p.IsString);
                var strValue = str.StringValue;
                var id = JsonConvert.DeserializeObject<int>(strValue);
                chromClient.Store.Dispatch(new deleteCommunity() { id = id });
            };

            communityActions.AddFunction("resetForm").Execute += (func, args) =>
            {
                chromClient.Store.Dispatch(new resetForm());
            };

            communityActions.AddFunction("addCommunity").Execute += (func, args) =>
            {
                var str = args.Arguments.FirstOrDefault(p => p.IsString);
                var strValue = str.StringValue;
                var communityFrom = JsonConvert.DeserializeObject<CommunityFrom>(strValue);
                chromClient.Store.Dispatch(new addCommunity() { communityFrom = communityFrom });
            };

            communityActions.AddFunction("updateCommunity").Execute += (func, args) =>
            {
                var str = args.Arguments.FirstOrDefault(p => p.IsString);
                var strValue = str.StringValue;
                var editFrom = JsonConvert.DeserializeObject<EditFrom>(strValue);
                chromClient.Store.Dispatch(new updateCommunity() { editFrom = editFrom });
            };
        }
    }
}
