using Packages;
using ReduxCore;
using ReduxStyleUI.XP;

namespace BorderlessFormStyleDemoApp
{
    public partial class Form2 : ReduxStyleForm<AppState>
    { 
        public Form2(Package<AppState> store)
			: base(store,"http://res.app.local/PopupWindow.html")
		{
			InitializeComponent();

		}

	}
}
