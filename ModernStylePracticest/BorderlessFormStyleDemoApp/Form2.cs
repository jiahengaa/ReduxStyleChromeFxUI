using Packages;
using ReduxCore;

namespace BorderlessFormStyleDemoApp
{
    public partial class Form2 : ReduxModernBaseForm<AppState>
    {
		public Form2(Package<AppState> store)
			: base(store,"http://res.app.local/PopupWindow.html")
		{
			InitializeComponent();

		}

	}
}
