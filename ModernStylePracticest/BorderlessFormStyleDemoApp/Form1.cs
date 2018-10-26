using ChromFXUI;
using CommunityActions;
using Newtonsoft.Json;
using Packages;
using ReduxCore;
using ReduxStyleUI.XP;

namespace BorderlessFormStyleDemoApp
{
    public partial class Form1 : ReduxStyleForm<AppState>
    {
        public Form1(Package<AppState> store)
            : base(store, Config.BaseUrl + "index.html")//"http://res.app.local/index.html"
        {
            InitializeComponent();

            this.ConfigChartAction();
            this.ConfigCommunityActions();

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

            store.Subscribe((subscription,action) =>
            {
                var state = store.GetState();
                string cmd = string.Format("app.updateData({0})", JsonConvert.SerializeObject(state));
                ExecuteJavascript(cmd);
            });

            Store.Dispatch(new loadCommunityList());
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
