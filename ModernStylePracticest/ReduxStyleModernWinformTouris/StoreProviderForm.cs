using Actions;
using Packages;
using ReduxCore;
using System;
using System.Collections.Generic;

namespace ReduxTraditionalWinformApp
{
    public partial class StoreProviderForm : ReduxBaseForm<AppState>
    {
        public StoreProviderForm(Package<AppState> store) : base(store)
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            Store.Subscribe(state =>
            {
                dataGridView1.UpdateUI(new Action(() =>
                {
                    dataGridView1.DataSource = null;
                    //dataGridView1.DataSource = state.Todos;
                }));
            
            }
            );
            Store.Dispatch(new TodosLoadedAction(new List<Todo>() {
                new Todo("111111"),
                new Todo("222222"),
                new Todo("3333"),
            }));
            base.OnLoad(e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Store.Dispatch(new AddTodoAction(new Todo("aaaa", false, "")));
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
           if( dataGridView1.CurrentRow != null)
            {
                Store.Dispatch(new DeleteTodoAction(dataGridView1.CurrentRow?.Cells["Id"].Value.ToString()));
            }
            
        }

        private void btnPop_Click(object sender, EventArgs e)
        {
            PopForm popPage = new PopForm(Store);
            popPage.Show();
        }
    }


}
