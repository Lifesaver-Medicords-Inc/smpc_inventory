using smpc_app.Services.Helpers;
using smpc_inventory_app.Services.Setup;
using smpc_inventory_app.Services.Setup.Item;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace smpc_inventory_app.Pages.Setup
{
    public partial class frm_item_pump_type_setup : UserControl
    {
        public frm_item_pump_type_setup()
        {
            InitializeComponent();
        }
        private async void GetData()
        {
            var data = await ItemPumpTypeServices.GetAsDatatable();
            dg_item_pump_type.DataSource = data;
        }
        private void frm_item_pump_type_setup_Load(object sender, EventArgs e)
        {
            GetData();
            btn_edit.Enabled = false;
            btn_delete.Enabled = false;
        }
        private async void btn_save_Click(object sender, EventArgs e)
        {
            // Get input values
            var data = Helpers.GetControlsValues(pnl_input);
            ApiResponseModel response = new ApiResponseModel();

            // Validate required fields
            string errorMessage =
                string.IsNullOrWhiteSpace(txt_code.Text) && string.IsNullOrWhiteSpace(txt_name.Text) ? "Code and Name cannot be empty." :
                string.IsNullOrWhiteSpace(txt_code.Text) ? "Code cannot be empty." :
                string.IsNullOrWhiteSpace(txt_name.Text) ? "Name cannot be empty." : null;

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
                ? await ItemPumpTypeServices.Insert(data)
                : await ItemPumpTypeServices.Update(data);

            // Handle result
            if (response.Success)
            {
                Helpers.ResetControls(pnl_input);
                GetData();
                BtnToggle(false);
            }

            string message = response.Success
                ? (isNewRecord ? "Item saved successfully." : "Item updated successfully.")
                : (isNewRecord ? "Failed to save item.\n" + response.message : "Failed to update item.\n" + response.message);

            Helpers.ShowDialogMessage(response.Success ? "success" : "error", message);
        }


        private void dg_item_pump_type_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            Panel[] pnlList = { pnl_input };
            DataTable dt = Helpers.ConvertDataGridViewToDataTable(dg_item_pump_type);
            Helpers.BindControls(pnlList, dt, e.RowIndex);
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
            pnl_input.Enabled = isEdit;
            dg_item_pump_type.Enabled = !isEdit;
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            BtnToggle(true);
        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            Helpers.ResetControls(pnl_input);
            BtnToggle(true);
            dg_item_pump_type.ClearSelection();
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            Helpers.ResetControls(pnl_input);
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
                var data = Helpers.GetControlsValues(pnl_input);

                bool isSuccess = await ItemPumpTypeServices.Delete(data);

                if (isSuccess)
                {
                    Helpers.ResetControls(pnl_input);
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
    }
}
