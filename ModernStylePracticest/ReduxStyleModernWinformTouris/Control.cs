using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReduxTraditionalWinformApp
{
    public static class ControlExtension
    {
        public static void UpdateUI(this System.Windows.Forms.Control control,Action updateAction)
        {
            if(control.InvokeRequired)
            {
                control.BeginInvoke(updateAction);
            }
            else
            {
                control.Invoke(updateAction);
            }
        }

        //public static void UpdateUI(this System.Windows.Forms.Control control, Delegate @delegate, params object[] args)
        //{
        //    if (control.InvokeRequired)
        //    {
        //        control.BeginInvoke(@delegate, args);
        //    }
        //    else
        //    {
        //        control.Invoke(@delegate, args);
        //    }
        //}
    }
}
