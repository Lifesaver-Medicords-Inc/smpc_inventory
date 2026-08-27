
using smpc_inventory_app.Services.Setup.Bpi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace smpc_sales_app.Pages
{
    public partial class ItemModal : Form
    {


        private Dictionary<string, dynamic> result { get; set; }
        public ItemModal()
        {
            InitializeComponent();
        }

        private async void GetItemList()
        {
            var data = await ItemListBpiServices.GetAsDatatable();
            //var dataItemSource = JsonHelper.ToDataTable(data.items);
            dg_ItemList.DataSource = data;
        }

        private void ItemModal_Load(object sender, EventArgs e)
        {
            GetItemList();
        }
        public Dictionary<string, dynamic> GetResult()
        {
            return result;
        }
        private void dgv_itemList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Live crash: NullReferenceException ("DataGridViewCell.Value.get returned
            // null") on the very first .Value.ToString() below. e.RowIndex >= 0 isn't a
            // strong enough guard on its own - dg_ItemList is bound directly to a
            // DataTable (GetItemList above), so it still shows the trailing "add new
            // row" placeholder by default. Clicking that row fires CellClick with a
            // real, non-negative RowIndex, but every cell's Value is genuinely null (no
            // DataRow backs it yet) rather than DBNull - IsNewRow is what actually
            // catches that case. TryParse instead of Parse/direct .ToString() as a
            // second layer, so any other unexpectedly-null cell degrades to a default
            // value instead of crashing the modal.
            if (e.RowIndex < 0 || e.RowIndex >= dg_ItemList.Rows.Count || dg_ItemList.Rows[e.RowIndex].IsNewRow)
                return;

            DataGridViewRow row = dg_ItemList.Rows[e.RowIndex];

            int.TryParse(row.Cells["id"].Value?.ToString(), out int item_id);
            string item_type = row.Cells["item_type"].Value?.ToString() ?? "";
            string item_code = row.Cells["item_code"].Value?.ToString() ?? "";
            string long_description = row.Cells["long_description"].Value?.ToString() ?? "";
            float.TryParse(row.Cells["item_price"].Value?.ToString(), out float item_price);

            Dictionary<string, dynamic> data = new Dictionary<string, dynamic>();
            data.Add("item_id", item_id);
            data.Add("item_type", item_type);
            data.Add("item_code", item_code);
            data.Add("long_description", long_description);
            data.Add("item_price", item_price);

            this.result = data;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void dg_ItemList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
