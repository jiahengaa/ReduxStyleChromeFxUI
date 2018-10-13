using ChromFXUI;
using Chromium.Remote;
using Newtonsoft.Json;
using Packages;
using ReduxCore;
using System.Linq;

namespace BorderlessFormStyleDemoApp
{
    public partial class Form1 : ReduxModernBaseForm<AppState>
    {
        public Form1(Package<AppState> store)
            : base(store, "http://res.app.local/index.html")
        {
            InitializeComponent();

            GlobalObject.AddFunction("showDialog").Execute += (_, args) =>
            {
                this.RequireUIThread(() =>
                {
                    var form2 = new Form2(store);
                    form2.ShowDialog(this);
                });
            };

            GlobalObject.AddFunction("showDevTools").Execute += (func, args) => Chromium.ShowDevTools();

            LoadHandler.OnLoadEnd += LoadHandler_OnLoadEnd;

            var communityActions = GlobalObject.AddObject("communityActions");

            communityActions.AddFunction("handleSearch").Execute += (func, args) =>
            {
                store.Dispatch(new CommunityActions.handleSearch());
            };

            communityActions.AddFunction("handlePageSizeChange").Execute += (func, args) =>
            {
                var str = args.Arguments.FirstOrDefault(p => p.IsString);
                var strValue = str.StringValue;
                var pagination = JsonConvert.DeserializeObject<Pagination>(strValue);

                Store.Dispatch(new CommunityActions.handlePageSizeChange() { pageSize = pagination.pageSize });
            };

            communityActions.AddFunction("handleCurrentChange").Execute += (func, args) =>
            {
                var str = args.Arguments.FirstOrDefault(p => p.IsString);
                var strValue = str.StringValue;
                var index = JsonConvert.DeserializeObject<int>(strValue);

                Store.Dispatch(new CommunityActions.handleCurrentChange() { current = index });
            };

            communityActions.AddFunction("loadCommunityList").Execute += (func, args) =>
            {
                Store.Dispatch(new CommunityActions.loadCommunityList());
            };

            communityActions.AddFunction("getCommunityList").Execute += (func, args) =>
            {
                var str = args.Arguments.FirstOrDefault(p => p.IsString);
                var strValue = str.StringValue;
                var pagination = JsonConvert.DeserializeObject<Pagination>(strValue);
                Store.Dispatch(new CommunityActions.getCommunityList() { pagination = pagination });
            };

            communityActions.AddFunction("deleteCommunity").Execute += (func, args) =>
            {
                var str = args.Arguments.FirstOrDefault(p => p.IsString);
                var strValue = str.StringValue;
                var id = JsonConvert.DeserializeObject<int>(strValue);
                Store.Dispatch(new CommunityActions.deleteCommunity() {  id  = id });
            };

            communityActions.AddFunction("resetForm").Execute += (func, args) =>
            {
                Store.Dispatch(new CommunityActions.resetForm());
            };

            communityActions.AddFunction("addCommunity").Execute += (func, args) =>
            {
                var str = args.Arguments.FirstOrDefault(p => p.IsString);
                var strValue = str.StringValue;
                var communityFrom = JsonConvert.DeserializeObject<CommunityFrom>(strValue);
                Store.Dispatch(new CommunityActions.addCommunity() { communityFrom = communityFrom });
            };

            communityActions.AddFunction("updateCommunity").Execute += (func, args) =>
            {
                var str = args.Arguments.FirstOrDefault(p => p.IsString);
                var strValue = str.StringValue;
                var editFrom = JsonConvert.DeserializeObject<EditFrom>(strValue);
                Store.Dispatch(new CommunityActions.updateCommunity() { editFrom = editFrom });
            };

            store.Subscribe(subscription =>
            {
                var state = store.GetState();
                string cmd = string.Format("app.updateData({0})", JsonConvert.SerializeObject(state));
                ExecuteJavascript(cmd);
            });

            Store.Dispatch(new CommunityActions.loadCommunityList());
        }

        private void LoadHandler_OnLoadEnd(object sender, Chromium.Event.CfxOnLoadEndEventArgs e)
        {
            if (e.Frame.IsMain)
            {
                Chromium.ShowDevTools();

                
            }
        }
    }

}
