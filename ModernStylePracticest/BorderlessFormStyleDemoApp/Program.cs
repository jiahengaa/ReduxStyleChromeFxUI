using ChromFXUI;
using CTReducer;
using Packages;
using ReduxCore;
using System;
using System.Windows.Forms;

namespace BorderlessFormStyleDemoApp
{
    static class Program
	{
		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("zh-CN");
            Config.InitConfig();
            if (Config.IsDebug)
            {
                if (Bootstrap.Load())
                {
                    Application.Run(new Form1(new Package<AppState>(new AppStateReducer())));
                }
                
            }
            else
            {
                if (Bootstrap.Load())
                {
                    Bootstrap.RegisterAssemblyResources(System.Reflection.Assembly.GetExecutingAssembly(), "Root");
                    Bootstrap.RegisterFolderResources(Application.StartupPath);

                    Application.Run(new Form1(new Package<AppState>(new AppStateReducer())));
                }
            }
		}
	}
}
