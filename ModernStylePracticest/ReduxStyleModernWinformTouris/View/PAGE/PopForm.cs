using Packages;
using ReduxCore;
using System;

namespace ReduxTraditionalWinformApp
{
    public partial class PopForm : ReduxBaseForm<AppState>
    {

        public PopForm(Package<AppState> store) : base(store)
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            //dataGridView1.DataSource = Store.GetState().Todos;
            //Store.Subscribe((state,action) =>
            //{
            //    dataGridView1.UpdateUI(new Action(() =>
            //    {
            //        dataGridView1.DataSource = null;
            //        dataGridView1.DataSource = state.Todos;
            //    }));
                
            //});
            base.OnLoad(e);
        }
    }
}
