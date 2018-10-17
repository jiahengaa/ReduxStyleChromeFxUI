using ChartActions;
using Packages;
namespace BorderlessFormStyleDemoApp
{
    public static class ChartActions
    {
        public static void ConfigChartAction(this ReduxModernBaseForm<AppState> chromClient)
        {
            var chartActions = chromClient.GlobalObject.AddObject("chartActions");

            chartActions.AddFunction("loadCommunityChartData").Execute += (func, args) =>
            {
                chromClient.Store.Dispatch(new loadCommunityChartData());
            };
        }
    }
}
