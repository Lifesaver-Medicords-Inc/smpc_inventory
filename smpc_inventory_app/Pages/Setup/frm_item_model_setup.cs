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
using smpc_inventory_app.Services.Setup.Item;
using smpc_inventory_app.Services.Setup;
using smpc_inventory_app.Data;
using smpc_inventory_app.Services.Helpers;
using smpc_inventory_app.Services.Setup.Model.Item;

namespace smpc_inventory_app.Pages.Setup
{
    public partial class frm_item_model_setup : UserControl
    {
        SetupModal modalSetup;
        DataTable items;
        Items records;
        int selectedRecord = 0;
        public frm_item_model_setup()
        {
            InitializeComponent();
        }
        private async void GetData()
        {
            GetItems();
            GetModels();
        }
        private async void GetModels()
        {
            if (string.IsNullOrWhiteSpace(txt_id.Text))
            {
                dg_item_model.DataSource = null;
                return;
            }

            var models = await ItemModelServices.GetAsDatatable();
            DataView dataView = new DataView(models);
            string selectedItemId = records.items[selectedRecord].item_model_id.ToString();
            dataView.RowFilter = $"id = '{selectedItemId}'";

            DataTable filteredData = dataView.ToTable();
            dg_item_model.DataSource = filteredData;
        }

        private async void GetItems()
        {

            var response = await RequestToApi<ApiResponseModel<Items>>.Get(ENUM_ENDPOINT.ITEM);
            records = response.Data;

            items = JsonHelper.ToDataTable(records.items);

            cmb_item.DataSource = items;
            cmb_item.ValueMember = "id";
            cmb_item.DisplayMember = "item_name";

            Bind(true);
        }
        private void Bind(bool isBind = false)
        {
            if (isBind)
            {
                Panel[] pnlList = { pnl_header };
                Helpers.BindControls(pnlList, items, selectedRecord);

                GetModels();
            }
        }

        private async void frm_item_model_setup_Load(object sender, EventArgs e)
        {
            GetData();
            btn_edit.Enabled = false;
            btn_delete.Enabled = false;
        }

        private async void btn_save_Click(object sender, EventArgs e)
        {
            // Get input values
            var data = Helpers.GetControlsValues(pnl_header);
            ApiResponseModel response = new ApiResponseModel();

            // Validate required fields
            string errorMessage =
                string.IsNullOrWhiteSpace(txt_item_code.Text)/* && string.IsNullOrWhiteSpace(cmb_item.Text) && string.IsNullOrWhiteSpace(cmb_item_brand.Text)*/ && string.IsNullOrWhiteSpace(txt_short_desc.Text) ? "Fill all required fields." :
                string.IsNullOrWhiteSpace(cmb_item.Text) ? "Item Name cannot be empty." :
                //string.IsNullOrWhiteSpace(cmb_item_brand.Text) ? "Brand cannot be empty." :
                string.IsNullOrWhiteSpace(txt_short_desc.Text) ? "Catalogue Year cannot be empty." :
                string.IsNullOrWhiteSpace(txt_item_code.Text) ? "Name cannot be empty." : null;

            if (!string.IsNullOrEmpty(errorMessage))
            {
                Helpers.ShowDialogMessage("error", errorMessage);
                return;
            }

            // Insert or update data
            bool isNewRecord = string.IsNullOrWhiteSpace(txt_id.Text);
            if (isNewRecord)
            {
                data.Remove("id");
            }

            response = isNewRecord
                ? await ItemModelServices.Insert(data)
                : await ItemModelServices.Update(data);

            // Handle result
            if (response.Success)
            {
                Helpers.ResetControls(pnl_header);
                GetData();
                BtnToggle(false);
            }

            string message = response.Success
                ? (isNewRecord ? "Item saved successfully." : "Item updated successfully.")
                : (isNewRecord ? "Failed to save item.\n" + response.message : "Failed to update item.\n" + response.message);

            Helpers.ShowDialogMessage(response.Success ? "success" : "error", message);
        }

        private void dg_item_model_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            Panel[] pnlList = { pnl_header };
            DataTable dt = Helpers.ConvertDataGridViewToDataTable(dg_item_model);
            Helpers.BindControls(pnlList, dt, e.RowIndex);

            cmb_item.Text = dg_item_model.Rows[e.RowIndex].Cells["related_name"].Value.ToString();
            /* cmb_item_brand.Text = dg_item_model.Rows[e.RowIndex].Cells["related_brand"].Value.ToString();*/

            btn_edit.Enabled = true;
            btn_delete.Enabled = true;
        }
        private void BtnToggle(bool isEdit)
        {
            btn_new.Visible = !isEdit;
            btn_delete.Visible = !isEdit;
            btn_edit.Visible = !isEdit;

            btn_save.Visible = isEdit;
            btn_close.Visible = isEdit;
            pnl_header.Enabled = isEdit;
            dg_item_model.Enabled = !isEdit;
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            BtnToggle(true);
        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            Helpers.ResetControls(pnl_header);
            BtnToggle(true);
            dg_item_model.ClearSelection();
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            Helpers.ResetControls(pnl_header);
            BtnToggle(false);
        }

        private async void btn_delete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this item?",
                "Confirm Deletion",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                var data = Helpers.GetControlsValues(pnl_header);

                bool isSuccess = await ItemModelServices.Delete(data);

                if (isSuccess)
                {
                    Helpers.ResetControls(pnl_header);
                    Helpers.ShowDialogMessage("success", "Item deleted successfully.");
                    GetData();
                    BtnToggle(false);
                }
                else
                {
                    Helpers.ShowDialogMessage("error", "Failed to delete item.");
                }
            }
        }
        private void cmb_item_name_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btn_add_name_Click(object sender, EventArgs e)
        {
            DataTable dt = CacheData.ItemName.Copy();
            if (dt.Columns["select"] != null)
            {
                dt.Columns.Remove("select");
            }

            modalSetup = new SetupModal("General Name", ENUM_ENDPOINT.ITEM_NAME, dt);
            DialogResult r = modalSetup.ShowDialog();
        }
        private void PopulateFields(DataRow selectedItem)
        {
            if (selectedItem == null) return;

            Bind(true);
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            if (items == null || items.Rows.Count == 0)
            {
                MessageBox.Show("No items available for selection.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Dictionary<string, string> columnMappings = new Dictionary<string, string>
                {
                    { "id", "ID" },
                    { "item_name", "ITEM NAME" },
                    { "short_desc", "SHORT DESC" }
                };

            using (SearchModal searchModal = new SearchModal("Search Items", items, columnMappings))
            {
                if (searchModal.ShowDialog() == DialogResult.OK)
                {
                    int selectedIndex = searchModal.SelectedIndex;

                    if (selectedIndex >= 0)
                    {
                        this.selectedRecord = selectedIndex; 
                        Bind(true); 
                    }
                }
            }
        }



        private void btn_prev_Click(object sender, EventArgs e)
        {
            if (this.selectedRecord > 0)
            {

                this.selectedRecord--;
                Bind(true);
            }
            else
            {
                MessageBox.Show("No record found", "SMPC SOFTWARE", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btn_next_Click(object sender, EventArgs e)
        {
             if (this.items.Rows.Count - 1 > this.selectedRecord)
            {
                this.selectedRecord++;
                Bind(true);
            }
            else
            {
                MessageBox.Show("No record found", "SMPC SOFTWARE", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
