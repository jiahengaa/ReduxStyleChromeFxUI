using Packages;
using ReduxCore;

namespace ReduxTraditionalWinformApp
{
    public partial class TwoForm :ReduxBaseForm<AppState>
    {
        public TwoForm(Package<AppState> store) : base(store)
        {
            InitializeComponent();
        }
    }
}
