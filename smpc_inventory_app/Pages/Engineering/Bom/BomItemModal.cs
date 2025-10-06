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
using smpc_app.Services.Helpers;

namespace smpc_inventory_app.Pages.Engineering.Bom
{
    public partial class BomItemModal : Form
    {
        private string placeHolderText = "BOM Search...";
        private DataTable Dt { get; set; }
        private Dictionary<string, dynamic> result { get; set; }
        public BomItemModal()
        {
            InitializeComponent();
            InitializeSearchBox();
        }

        private async void GetBomItemList()
        {
            var data = await ItemListBomServices.GetAllAsDatatable();
            Dt = data;
            dg_BomItemList.DataSource = Dt;

            //Always hide item_id column if it exists
            if (dg_BomItemList.Columns.Contains("item_id"))
            {
                dg_BomItemList.Columns["item_id"].Visible = false;
            }
        }

        private void BomItemModal_Load(object sender, EventArgs e)
        {
            Helpers.Loading.ShowLoading(dg_BomItemList, "Fetching data...");

            GetBomItemList();

            Helpers.Loading.HideLoading(dg_BomItemList);
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

        private void InitializeSearchBox()
        {
            txt_search = Helpers.CreateSearchBox(placeHolderText, txt_search_TextChanged);
            this.Controls.Add(txt_search);
        }

        private void txt_search_TextChanged(object sender, EventArgs e)
        {
            string searchText = txt_search.Text.Trim();

            if (string.IsNullOrEmpty(searchText) || searchText == placeHolderText)
            {
                dg_BomItemList.DataSource = Dt;
            }
            else
            {
                var searchedData = Helpers.FilterDataTable(Dt, searchText, "general_name", "item_code", "item_model", "short_desc", "uom_name", "size");
                dg_BomItemList.DataSource = searchedData;
            }
        }
    }
}
