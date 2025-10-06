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
        private List<int> excludedSupplierIds = new List<int>();
        private string itemId;
        public SupplierModal(List<int> excludedIds, string itemId)
        {
            InitializeComponent();
            excludedSupplierIds = excludedIds ?? new List<int>();
            this.itemId = itemId;
        }
        private void ItemModal_Load(object sender, EventArgs e)
        {
            GetSupplierList();
        }
        private async void GetSupplierList()
        {
            var data = await PurchasingListSupplierServices.GetAsDataTable();

            var filtered = data.Clone(); // clone structure

            foreach (DataRow row in data.Rows)
            {
                if (row["item_ids"] != DBNull.Value)
                {
                    var itemIdsStr = row["item_ids"].ToString();
                    var itemIds = itemIdsStr.Split(',').Select(s => s.Trim());

                    if (itemIds.Contains(itemId))
                    {
                        filtered.ImportRow(row); // include only rows with the itemId
                    }
                }
            }

            dgv_supplier.AutoGenerateColumns = false;
            dgv_supplier.DataSource = filtered;

            // Apply gray background to used suppliers
            foreach (DataGridViewRow row in dgv_supplier.Rows)
            {
                if (row.Cells[0].Value != null && int.TryParse(row.Cells[0].Value.ToString(), out int id))
                {
                    if (excludedSupplierIds.Contains(id))
                    {
                        row.DefaultCellStyle.ForeColor = Color.Gray;
                        row.DefaultCellStyle.BackColor = Color.LightGray;
                        row.ReadOnly = true;
                    }
                }
            }
        }

        public Dictionary<string, dynamic> GetResult()
        {
            return result;
        }
        private void dgv_supplier_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int supplier_id = int.Parse(dgv_supplier.Rows[e.RowIndex].Cells[0].Value.ToString());

                if (excludedSupplierIds.Contains(supplier_id))
                {
                    MessageBox.Show("This supplier is already selected in another row.", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string supplier = dgv_supplier.Rows[e.RowIndex].Cells[1].Value?.ToString() ?? string.Empty;
                string contact_no = dgv_supplier.Rows[e.RowIndex].Cells[2].Value?.ToString() ?? string.Empty;

                int payment_terms_id = 0;
                if (dgv_supplier.Rows[e.RowIndex].Cells[3].Value != null &&
                    int.TryParse(dgv_supplier.Rows[e.RowIndex].Cells[3].Value.ToString(), out int temp))
                {
                    payment_terms_id = temp;
                }

                Dictionary<string, dynamic> data = new Dictionary<string, dynamic>
                {
                    { "supplier_id", supplier_id },
                    { "supplier_name", supplier },
                    { "contact_no", contact_no },
                    { "payment_terms_id", payment_terms_id }
                };

                this.result = data;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

    }
}
