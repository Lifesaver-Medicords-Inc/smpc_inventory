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

namespace smpc_inventory_app.Pages.Purchasing.Modal
{
    public partial class SupplierModal : Form
    {
        private Dictionary<string, dynamic> result { get; set; }
        public SupplierModal()
        {
            InitializeComponent();
        }
        private void ItemModal_Load(object sender, EventArgs e)
        {
            GetSupplierList();
        }
        private async void GetSupplierList()
        {
            var data = await PurchasingListSupplierServices.GetAsDataTable();

            dgv_supplier.DataSource = data;

        }
        public Dictionary<string, dynamic> GetResult()
        {
            return result;
        }

        private void HideColumn(DataGridView grid, params string[] columnNames)
        {
            foreach (string colName in columnNames)
            {
                if (grid.Columns.Contains(colName))
                {
                    grid.Columns[colName].Visible = false;
                }
            }
        }

        private void dgv_supplier_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int supplier_id = int.Parse(dgv_supplier.Rows[e.RowIndex].Cells[0].Value.ToString());
                string supplier = dgv_supplier.Rows[e.RowIndex].Cells[1].Value.ToString();
                string contact_no = dgv_supplier.Rows[e.RowIndex].Cells[2].Value.ToString();
               

                Dictionary<string, dynamic> data = new Dictionary<string, dynamic>();
                data.Add("supplier_id", supplier_id);
                data.Add("supplier", supplier);
                data.Add("contact_no", contact_no);



                this.result = data;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
