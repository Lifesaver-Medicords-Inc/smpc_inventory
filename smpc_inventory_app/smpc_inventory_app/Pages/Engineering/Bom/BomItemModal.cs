using smpc_inventory_app.Services.Setup.Bom;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace smpc_inventory_app.Pages.Engineering.Bom
{
    public partial class BomItemModal : Form
    {
        private Dictionary<string, dynamic> result { get; set; }
        public BomItemModal()
        {
            InitializeComponent();
        }

        private async void GetBomItemList()
        {
            var data = await ItemListBomServices.GetAsDatatable();
            dg_BomItemList.DataSource = data;
        }

        private void BomItemModal_Load(object sender, EventArgs e)
        {
            GetBomItemList();
        }

        public Dictionary<string, dynamic> GetResult()
        {
            return result;
        }

        private void dg_BomItemList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0)
            {
                int item_id = int.Parse(dg_BomItemList.Rows[e.RowIndex].Cells[1].Value.ToString());
                string short_desc = dg_BomItemList.Rows[e.RowIndex].Cells[2].Value.ToString();
                string item_code = dg_BomItemList.Rows[e.RowIndex].Cells[3].Value.ToString();
                string general_name = dg_BomItemList.Rows[e.RowIndex].Cells[4].Value.ToString();
                string item_model = dg_BomItemList.Rows[e.RowIndex].Cells[5].Value.ToString();
                string uom_name = dg_BomItemList.Rows[e.RowIndex].Cells[6].Value.ToString();
                string size = dg_BomItemList.Rows[e.RowIndex].Cells[7].Value.ToString();

                Dictionary<string, dynamic> data = new Dictionary<string, dynamic>();
                data.Add("item_id", item_id);
                data.Add("short_desc", short_desc);
                data.Add("item_code", item_code);
                data.Add("general_name", general_name);
                data.Add("item_model", item_model);
                data.Add("uom_name", uom_name);
                data.Add("size", size);

                this.result = data;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void dg_BomItemList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
