using Packages;
using Reducers;
using ReduxCore;
using System;
using System.Windows.Forms;

namespace ReduxTraditionalWinformApp
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
            Application.ThreadException += Application_ThreadException;
      
            Application.Run(new StoreProviderForm(new Package<AppState>(new AppStateReducer())));
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {

        }
    }
}
