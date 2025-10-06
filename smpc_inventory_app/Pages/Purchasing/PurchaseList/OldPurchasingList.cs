using smpc_inventory_app.Data;
using smpc_inventory_app.Pages.Purchasing.Modal;
using smpc_inventory_app.Services.Helpers;
using smpc_inventory_app.Services.Setup;
using smpc_inventory_app.Services.Setup.Model.Purchasing;
using smpc_inventory_app.Services.Setup.Purchasing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace smpc_inventory_app.Pages.Purchasing
{
    public partial class OldPurchasingList : UserControl
    {

        public OldPurchasingList()
        {
            InitializeComponent();
        }

        private void PurchasingList_Load(object sender, EventArgs e)
        {
            GetData();
        }
        private async void GetData()
        {
            var data = await SOPurchasingListServices.GetAsDataTable();
            dataGridView1.DataSource = data;
            HideColumns(dataGridView1, "order_detail_ids", "sales_order_nos", "based_ids", "item_id", "qtys", "unit_of_measures");

        }
        private void HideColumns(DataGridView grid, params string[] columnNames)
        {
            foreach (var column in columnNames)
            {
                if (grid.Columns.Contains(column))
                {
                    grid.Columns[column].Visible = false;
                }
            }
        }

       private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
{
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dataGridView1.Columns[e.ColumnIndex].Name == "supplier")
                {
                    DataRowView rowView = dataGridView1.Rows[e.RowIndex].DataBoundItem as DataRowView;
                    if (rowView != null)
                    {
                        DataRow selectedRow = rowView.Row;
                        CanvassSheetModal modal = new CanvassSheetModal(selectedRow);
                        modal.StartPosition = FormStartPosition.CenterParent;
                        modal.ShowDialog();
                    }
                }
                else if (dataGridView1.Columns[e.ColumnIndex].Name == "details")
                {
                    DataRowView rowView = dataGridView1.Rows[e.RowIndex].DataBoundItem as DataRowView;
                    if (rowView != null)
                    {
                        DataRow selectedRow = rowView.Row;
                        ItemDetailsModal modal = new ItemDetailsModal(selectedRow);
                        modal.ShowDialog();
                    }
                }
                else
                {
                    MessageBox.Show("textbox");
                }
                
            }
        }

    }
}
