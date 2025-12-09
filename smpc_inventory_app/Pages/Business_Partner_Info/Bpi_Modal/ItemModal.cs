
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
            if (e.RowIndex >= 0)
            {
                int item_id = int.Parse(dg_ItemList.Rows[e.RowIndex].Cells["id"].Value.ToString());
                string item_type = dg_ItemList.Rows[e.RowIndex].Cells["item_type"].Value.ToString();
                string item_code = dg_ItemList.Rows[e.RowIndex].Cells["item_code"].Value.ToString();
                string long_description = dg_ItemList.Rows[e.RowIndex].Cells["long_description"].Value.ToString();
                float item_price = float.Parse(dg_ItemList.Rows[e.RowIndex].Cells["item_price"].Value.ToString());



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
        }

        private void dg_ItemList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
