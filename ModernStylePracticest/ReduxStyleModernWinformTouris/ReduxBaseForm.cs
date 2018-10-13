using ReduxCore;
using System.Windows.Forms;

namespace ReduxTraditionalWinformApp
{
    public class ReduxBaseForm<S> : Form where S : struct
    {
        private Package<S> store;
        public virtual Package<S> Store
        {
            private set
            {
                store = value;
            }
            get
            {
                return store;
            }
        }
        public ReduxBaseForm(Package<S> store)
        {
            Store = store;
        }

        public ReduxBaseForm()
        {

        }
    }
}
